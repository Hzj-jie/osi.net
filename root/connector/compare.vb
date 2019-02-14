
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

' TODO: Move to type_info.
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

    Sub New()
        compare_error_result = rnd_int(min_int32, min_int8)
    End Sub

    Private Sub compare_error(Of T, T2)(ByVal ex As Exception)
        If Not is_suppressed.compare_error() Then
            raise_error(error_type.exclamation,
                        "caught exception when comparing ",
                        type_info(Of T).fullname,
                        " with ",
                        type_info(Of T2).fullname,
                        ", ex ",
                        ex)
        End If
    End Sub

    Private NotInheritable Class compare_cache(Of T, T2)
        Private Shared ReadOnly always_fail As _do_val_val_ref(Of T, T2, Int32, Boolean)
        Private Shared ReadOnly c As _do_val_val_ref(Of T, T2, Int32, Boolean)

        Shared Sub New()
            always_fail = Function(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
                              Return False
                          End Function
            If comparer_for_specific_types(c) Then
                '
            ElseIf type_info(Of T, type_info_operators.is, IComparable(Of T2)).v Then
                c = always_succeed(AddressOf this_to_t2(Of T2))
            ElseIf type_info(Of T2, type_info_operators.is, IComparable(Of T)).v Then
                c = always_succeed(AddressOf that_to_t(Of T))
            ElseIf type_info(Of T, type_info_operators.is, IComparable).v Then
                If use_restricted_compare_to_object(Of T)() Then
                    c = AddressOf this_to_object_with_same_type(Of T)
                Else
                    c = always_succeed(AddressOf this_to_object)
                End If
            ElseIf type_info(Of T2, type_info_operators.is, IComparable).v Then
                If use_restricted_compare_to_object(Of T2)() Then
                    c = AddressOf that_to_object_with_same_type(Of T2)
                Else
                    c = always_succeed(AddressOf that_to_object)
                End If
            ElseIf type_info(Of T).is_object OrElse type_info(Of T2).is_object Then
                raise_error(error_type.performance,
                            "compare_cache(Of *, Object) or compare_cache(Of Object, *) ",
                            "impact performance seriously.")
                If type_info(Of T).is_object AndAlso type_info(Of T2).is_object Then
                    c = AddressOf runtime_object_compare
                ElseIf type_info(Of T).is_object Then
                    c = AddressOf runtime_this_compare(Of T2)
                Else
                    assert(type_info(Of T2).is_object)
                    c = AddressOf runtime_that_compare(Of T)
                End If
            Else
                If Not is_suppressed.compare_error() Then
                    raise_error(error_type.exclamation,
                                "caught types do not have IComparable implement, unable to compare ",
                                type_info(Of T).fullname,
                                " with ",
                                type_info(Of T2).fullname)
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
            ' Nullable<T> implements IComparable<T> in runtime.
            If type_info(Of T).is_nullable AndAlso
               type_info(Of T, type_info_operators.is, T2).v Then
                o = always_succeed(AddressOf this_to_object)
                Return True
            ElseIf type_info(Of T).is_nullable Then
                If GetType(T2).is(Nullable.GetUnderlyingType(GetType(T))) Then
                    o = always_succeed(AddressOf this_to_t2(Of T2))
                    Return True
                ElseIf type_info(Of T2).is_object Then
                    Return False
                Else
                    o = always_fail
                    Return True
                End If
            ElseIf type_info(Of T2).is_nullable Then
                If GetType(T).is(Nullable.GetUnderlyingType(GetType(T2))) Then
                    o = always_succeed(AddressOf that_to_t(Of T))
                    Return True
                ElseIf type_info(Of T).is_object Then
                    Return False
                Else
                    o = always_fail
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        Public Shared Function compare(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
            If c Is Nothing Then
                Return False
            Else
#If DEBUG Then
                Dim msg As String = Nothing
                msg = strcat("Comparing ",
                             type_info(Of T).fullName,
                             " with ",
                             type_info(Of T2).fullName,
                             " needs to be specifically handled.")
                Dim o2 As Int32 = 0
                Dim r As Boolean = False
                r = c(this, that, o)
                If Not r Then
                    assert(r = runtime_compare(this, that, o2), msg)
                    assert(o = o2, msg)
                End If
                Return r
#Else
                Return c(this, that, o)
#End If
            End If
        End Function

        Public Shared Function comparable() As Boolean
            Return Not c Is Nothing
        End Function
    End Class

    Private Function use_restricted_compare_to_object(Of T)() As Boolean
        Dim result As Boolean = False
        result = type_info(Of T).is_primitive OrElse type_info(Of T, type_info_operators.is, String).v
        If result Then
            assert(type_info(Of T, type_info_operators.is, IComparable).v)
        End If
        Return result
    End Function

    Private Function this_to_t2(Of T)(ByVal this As Object, ByVal that As T) As Int32
        Return direct_cast(Of IComparable(Of T))(this).CompareTo(that)
    End Function

    Private Function this_to_object(ByVal this As Object, ByVal that As Object) As Int32
        Return direct_cast(Of IComparable)(this).CompareTo(that)
    End Function

    Private Function this_to_object_with_same_type(Of T) _
                                                  (ByVal this As T, ByVal that As Object, ByRef o As Int32) As Boolean
        If that.GetType().is(Of T)() Then
            o = this_to_object(this, that)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function that_to_t(Of T)(ByVal this As T, ByVal that As Object) As Int32
        Return -direct_cast(Of IComparable(Of T))(that).CompareTo(this)
    End Function

    Private Function that_to_object(ByVal this As Object, ByVal that As Object) As Int32
        Return -direct_cast(Of IComparable)(that).CompareTo(this)
    End Function

    Private Function that_to_object_with_same_type(Of T) _
                                                  (ByVal this As Object, ByVal that As T, ByRef o As Int32) As Boolean
        If this.GetType().is(Of T)() Then
            o = that_to_object(this, that)
            Return True
        Else
            Return False
        End If
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
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return type_comparer.compare(this.GetType(), GetType(T), this, that, o) OrElse
               runtime_this_to_t2(this, that, o) OrElse
               runtime_this_to_object(this, that, o)
    End Function

    Private Function runtime_that_compare(Of T)(ByVal this As T, ByVal that As Object, ByRef o As Int32) As Boolean
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return type_comparer.compare(GetType(T), that.GetType(), this, that, o) OrElse
               runtime_that_to_t(this, that, o) OrElse
               runtime_that_to_object(this, that, o)
    End Function

    Private Function runtime_object_compare(ByVal this As Object, ByVal that As Object, ByRef o As Int32) As Boolean
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return type_comparer.compare(this.GetType(), that.GetType(), this, that, o) OrElse
               runtime_this_to_object(this, that, o) OrElse
               runtime_that_to_object(this, that, o)
    End Function

    Public Function runtime_compare(ByVal this As Object, ByVal that As Object) As Int32
        Dim o As Int32 = 0
        o = object_compare(this, that)
        If o <> object_compare_undetermined Then
            Return o
        End If
        assert(runtime_this_to_object(this, that, o) OrElse
               runtime_that_to_object(this, that, o))
        Return o
    End Function

    Private Function runtime_compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        Return type_comparer.compare(GetType(T), GetType(T2), this, that, o) OrElse
               runtime_this_to_t2(this, that, o) OrElse
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
        Return object_comparable(Of T, T2)() OrElse
               comparer(Of T, T2).defined() OrElse
               compare_cache(Of T, T2).comparable()
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
