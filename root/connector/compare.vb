
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

' Use following order to compare two unknown types
' - object_compare
' - comparer(Of T, T2)
' (cached) {
'   - T.IComparable(T2) OR
'   - T2.IComparable(T) OR
'   - T.IComparable OR
'   - T2.IComparable OR
'   - Special treatments (Nullable(Of )) OR
'   if T == object {
'     - GetType(T).IComparable(T2) AND
'     - GetType(T).IComparable AND
'   }
'   if T2 == object {
'     - GetType(T2).IComparable(T) AND
'     - GetType(T2).IComparable AND
'   }
' }
Public Module _compare
    Public ReadOnly compare_error_result As Int32 = 0
    Private ReadOnly suppress_compare_error_binder As _
                         binder(Of Func(Of Boolean), suppress_compare_error_binder_protector)

    Sub New()
        suppress_compare_error_binder = New binder(Of Func(Of Boolean), suppress_compare_error_binder_protector)()
        compare_error_result = rnd_int(min_int32, min_int8)
    End Sub

    Private Function suppress_compare_error() As Boolean
        Return suppress_compare_error_binder.has_value() AndAlso
               (+suppress_compare_error_binder)()
    End Function

    Private Sub compare_error(Of T, T2)(ByVal ex As Exception)
        If Not suppress_compare_error() Then
            raise_error(error_type.exclamation,
                        "caught exception when comparing ",
                        GetType(T).FullName(),
                        " with ",
                        GetType(T2).FullName(),
                        ", ex ",
                        ex)
        End If
    End Sub

    Private NotInheritable Class compare_cache(Of T, T2)
        Private Shared ReadOnly c As _do_val_val_ref(Of T, T2, Int32, Boolean)

        Shared Sub New()
            If comparer_for_specific_types(c) Then
                '
            ElseIf type_info(Of T, type_info_operators.is, IComparable(Of T2)).v Then
                c = always_succeed(AddressOf this_to_t2(Of T2))
            ElseIf type_info(Of T2, type_info_operators.is, IComparable(Of T)).v Then
                c = always_succeed(AddressOf that_to_t(Of T))
            ElseIf type_info(Of T, type_info_operators.is, IComparable).v Then
                c = always_succeed(AddressOf this_to_object)
            ElseIf type_info(Of T2, type_info_operators.is, IComparable).v Then
                c = always_succeed(AddressOf that_to_object)
            ElseIf type_info(Of T).is_object OrElse type_info(Of T2).is_object Then
                raise_error(error_type.performance,
                            "compare_cache(Of *, Object) or compare_cache(Of Object, *) impact performance seriously.")
                If type_info(Of T).is_object AndAlso type_info(Of T2).is_object Then
                    c = AddressOf runtime_object_compare
                ElseIf type_info(Of T).is_object Then
                    c = AddressOf runtime_this_compare(Of T2)
                Else
                    assert(type_info(Of T2).is_object)
                    c = AddressOf runtime_that_compare(Of T)
                End If
            Else
                If Not suppress_compare_error() Then
                    raise_error(error_type.exclamation,
                                "caught types do not have IComparable implement, unable to compare ",
                                GetType(T).FullName(),
                                " with ",
                                GetType(T2).FullName())
                End If
            End If
        End Sub

        Private Shared Function always_succeed(ByVal i As Func(Of T, T2, Int32)) _
                                              As _do_val_val_ref(Of T, T2, Int32, Boolean)
            assert(Not i Is Nothing)
            Return Function(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
                       o = i(this, that)
                       Return True
                   End Function
        End Function

        Private Shared Function comparer_for_specific_types(
                                    ByRef o As _do_val_val_ref(Of T, T2, Int32, Boolean)) As Boolean
            If GetType(T).is(GetType(Nullable(Of ))) AndAlso
               GetType(T2).is(Nullable.GetUnderlyingType(GetType(T))) Then
                o = always_succeed(AddressOf this_to_t2(Of T2))
                Return True
            ElseIf GetType(T2).is(GetType(Nullable(Of ))) AndAlso
                   GetType(T).is(Nullable.GetUnderlyingType(GetType(T2))) Then
                o = always_succeed(AddressOf that_to_t(Of T))
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function compare(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
            If c Is Nothing Then
                Return False
            Else
#If NDEBUG Then
                Dim msg As String = Nothing
                msg = strcat("Comparing ",
                             GetType(T).FullName(),
                             " with ",
                             GetType(T2).FullName(),
                             " needs to be specifically handled.")
                Dim o2 As Int32 = 0
                assert(c(this, that, o) = runtime_compare(this, that, o2), msg)
                assert(o = o2, msg)
#End If
                Return c(this, that, o)
            End If
        End Function

        Public Shared Function comparable() As Boolean
            Return Not c Is Nothing
        End Function
    End Class

    Private Function this_to_t2(Of T)(ByVal this As Object, ByVal that As T) As Int32
        Return direct_cast(Of IComparable(Of T))(this).CompareTo(that)
    End Function

    Private Function this_to_object(ByVal this As Object, ByVal that As Object) As Int32
        Return direct_cast(Of IComparable)(this).CompareTo(that)
    End Function

    Private Function that_to_t(Of T)(ByVal this As T, ByVal that As Object) As Int32
        Return -direct_cast(Of IComparable(Of T))(that).CompareTo(this)
    End Function

    Private Function that_to_object(ByVal this As Object, ByVal that As Object) As Int32
        Return -direct_cast(Of IComparable)(that).CompareTo(this)
    End Function

    Private Function runtime_this_to_t2(Of T)(ByVal this As Object, ByVal that As T, ByRef o As Int32) As Boolean
        If TypeOf this Is IComparable(Of T) Then
            o = this_to_t2(this, that)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function runtime_this_to_object(ByVal this As Object, ByVal that As Object, ByRef o As Int32) As Boolean
        If TypeOf this Is IComparable Then
            o = this_to_object(this, that)
            Return True
        ElseIf TypeOf this Is IComparable(Of Object) Then
            o = this_to_t2(this, that)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function runtime_that_to_t(Of T)(ByVal this As T, ByVal that As Object, ByRef o As Int32) As Boolean
        If TypeOf that Is IComparable(Of T) Then
            o = that_to_t(this, that)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function runtime_that_to_object(ByVal this As Object, ByVal that As Object, ByRef o As Int32) As Boolean
        If TypeOf that Is IComparable Then
            o = that_to_object(this, that)
            Return True
        ElseIf TypeOf that Is IComparable(Of Object) Then
            o = that_to_t(this, that)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function runtime_this_compare(Of T)(ByVal this As Object, ByVal that As T, ByRef o As Int32) As Boolean
        Return runtime_this_to_t2(this, that, o) OrElse
               runtime_this_to_object(this, that, o)
    End Function

    Private Function runtime_that_compare(Of T)(ByVal this As T, ByVal that As Object, ByRef o As Int32) As Boolean
        Return runtime_that_to_t(this, that, o) OrElse
               runtime_that_to_object(this, that, o)
    End Function

    Private Function runtime_object_compare(ByVal this As Object, ByVal that As Object, ByRef o As Int32) As Boolean
        Return runtime_this_to_object(this, that, o) OrElse
               runtime_that_to_object(this, that, o)
    End Function

    Private Function runtime_compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        Return runtime_this_to_t2(this, that, o) OrElse
               runtime_that_to_t(this, that, o) OrElse
               runtime_this_to_object(this, that, o) OrElse
               runtime_that_to_object(this, that, o)
    End Function

    Private Function do_non_null_compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        If comparer(Of T, T2).defined() Then
            o = comparer.compare(this, that)
            Return True
        ElseIf compare_cache(Of T, T2).compare(this, that, o) Then
            Return True
        End If
        Return False
    End Function

    Public Function non_null_compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        Try
            Return do_non_null_compare(this, that, o)
        Catch ex As Exception
            compare_error(Of T, T2)(ex)
            Return False
        End Try
    End Function

    Public Function compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        o = object_compare(this, that)
        If o <> object_compare_undetermined Then
            Return True
        End If
        Return non_null_compare(this, that, o)
    End Function

    Public Function comparable(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return compare(this, that, 0)
    End Function

    Public Function comparable(Of T, T2)() As Boolean
        Return compare_cache(Of T, T2).comparable()
    End Function

    Public Function compare(Of T, T2)(ByVal this As T, ByVal that As T2) As Int32
        Dim o As Int32 = 0
        Return If(compare(this, that, o), o, compare_error_result)
    End Function

    Public Function cast_compare(Of T, T2, CompT)(ByVal this As T, ByVal that As T2) As Int32
        Return compare(this, cast(Of CompT)(that))
    End Function

    Public Function compare(Of T, T2 As CompT, CompT)(ByVal this As T, ByVal that As T2) As Int32
        Return compare(this, cast(Of CompT)(that))
    End Function

    Public Function compare(Of T)(ByVal this As T, ByVal that As Object) As Int32
        Return compare(Of T, Object)(this, that)
    End Function

    'speed up
    Public Function compare(ByVal this As Byte, ByVal that As Byte) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Int16, ByVal that As Int16) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Int32, ByVal that As Int32) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Int64, ByVal that As Int64) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As SByte, ByVal that As SByte) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As UInt16, ByVal that As UInt16) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Char, ByVal that As Char) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As UInt32, ByVal that As UInt32) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As UInt64, ByVal that As UInt64) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Single, ByVal that As Single) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Double, ByVal that As Double) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As Decimal, ByVal that As Decimal) As Int32
        Return this.CompareTo(that)
    End Function

    Public Function compare(ByVal this As String, ByVal that As String) As Int32
        Return strcmp(this, that)
    End Function
End Module
