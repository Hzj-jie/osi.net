
#Const cached_compare = True
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.delegates
Imports osi.root.constants

Public Module _compare
    Public Const object_compare_undetermined As Int32 = 2
    Private Const object_compare_equal As Int32 = 0
    Private Const object_compare_larger As Int32 = 1
    Private Const object_compare_less As Int32 = -1
    Public ReadOnly compare_error_result As Int32 = 0
    Private ReadOnly suppress_compare_error_binder As  _
                         binder(Of Func(Of Boolean), suppress_compare_error_binder_protector)
    Private ReadOnly specifically_treated_types() As Type

    Sub New()
        suppress_compare_error_binder = New binder(Of Func(Of Boolean), suppress_compare_error_binder_protector)()
        assert(object_compare_less = npos)
        assert(object_compare_larger = 1)
        assert(object_compare_equal = 0)
        compare_error_result = rnd_int(min_int32, object_compare_less)
        assert(compare_error_result < 0 AndAlso
                 compare_error_result <> object_compare_less AndAlso
                 compare_error_result <> object_compare_equal AndAlso
                 compare_error_result <> object_compare_larger)
        ReDim specifically_treated_types(0)
        specifically_treated_types(0) = GetType(Nullable(Of ))
    End Sub

    Private Function suppress_compare_error() As Boolean
        Return suppress_compare_error_binder.has_value() AndAlso
               (+suppress_compare_error_binder)()
    End Function

    Private Function object_comparable(ByVal this_is_value_type As Boolean,
                                       ByVal that_is_value_type As Boolean) As Boolean
        Return Not (this_is_value_type AndAlso that_is_value_type)
    End Function

    Public Function object_comparable(Of T, T2)() As Boolean
        Return object_comparable(is_valuetype(Of T), is_valuetype(Of T2))
    End Function

    Public Function object_comparable(ByVal this As Object, ByVal that As Object) As Boolean
        Return object_comparable(TypeOf this Is ValueType, TypeOf that Is ValueType)
    End Function

    Public Function object_compare(ByVal this As Object, ByVal that As Object) As Int32
        If Object.ReferenceEquals(this, that) Then
            Return object_compare_equal
        ElseIf this Is Nothing Then
            Return object_compare_less
        ElseIf that Is Nothing Then
            Return object_compare_larger
        Else
            Return object_compare_undetermined
        End If
    End Function

    Private Function this_to_t2(Of T)(ByVal this As Object, ByVal that As T) As Int32
        Return cast(Of IComparable(Of T))(this).CompareTo(that)
    End Function

    Private Function this_to_object(ByVal this As Object, ByVal that As Object) As Int32
        Return cast(Of IComparable)(this).CompareTo(that)
    End Function

    Private Function that_to_t(Of T)(ByVal this As T, ByVal that As Object) As Int32
        Return -cast(Of IComparable(Of T))(that).CompareTo(this)
    End Function

    Private Function that_to_object(ByVal this As Object, ByVal that As Object) As Int32
        Return -cast(Of IComparable)(that).CompareTo(this)
    End Function

    Private Function runtime_compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        Try
            If TypeOf this Is IComparable(Of T2) Then
                o = this_to_t2(this, that)
                Return True
            ElseIf TypeOf that Is IComparable(Of T) Then
                o = that_to_t(this, that)
                Return True
            ElseIf TypeOf this Is IComparable Then
                o = this_to_object(this, that)
                Return True
            ElseIf TypeOf that Is IComparable Then
                o = that_to_object(this, that)
                Return True
            ElseIf comparer(Of T, T2).defined() Then
                o = comparer(Of T, T2).compare(this, that)
                Return True
            Else
                If Not suppress_compare_error() Then
                    raise_error(error_type.exclamation,
                                  "caught instances do not have IComparable implement or is nothing, ",
                                  "unable to compare ",
                                  this.GetType().FullName(), " with ", that.GetType().FullName())
                End If
                Return False
            End If
        Catch ex As Exception
            compare_error(Of T, T2)(ex)
            Return False
        End Try
    End Function

    Private Sub compare_error(Of T, T2)(ByVal ex As Exception)
        If Not TypeOf ex Is ThreadAbortException AndAlso Not suppress_compare_error() Then
            raise_error(error_type.exclamation,
                          "caught exception when comparing ",
                          GetType(T).FullName(), " with ", GetType(T2).FullName(),
                          ", ex ", ex.Message, ", callstack ", ex.StackTrace())
        End If
    End Sub

#If cached_compare Then
    Private Structure compare_cache(Of T, T2)
        Private Shared ReadOnly use_object_compare As Boolean
        Private Shared ReadOnly use_runtime_compare As Boolean
        Private Shared ReadOnly contains_object As Boolean
        Private Shared ReadOnly c As Func(Of T, T2, Int32)
        Private Shared ca As Boolean?

        Shared Sub New()
            use_object_compare = connector.object_comparable(Of T, T2)()
            'special treatment for Nullable<>, GetType(Nullable(Of Int32)) does not implemented IComparable(Of Int32),
            'but an instance of Nullable(Of Int32) has IComparable(Of Int32) implemented.
            'this type is specific handled by CLI, so it also need to be taken care here
            'say Nullable<int> can only compare with int,
            'do not need to take care about object, since it does not have IComparable implemented.
            use_runtime_compare = False
            For i As Int32 = 0 To array_size(specifically_treated_types) - 1
                If GetType(T).is(specifically_treated_types(i)) OrElse
                   GetType(T2).is(specifically_treated_types(i)) Then
                    use_runtime_compare = True
                    Exit For
                End If
            Next
            contains_object = type_info(Of T).is_object OrElse type_info(Of T2).is_object
            If Not use_runtime_compare Then
                If istype(Of T, IComparable(Of T2))() Then
                    c = AddressOf this_to_t2
                ElseIf istype(Of T2, IComparable(Of T))() Then
                    c = AddressOf that_to_t
                ElseIf istype(Of T, IComparable)() Then
                    c = AddressOf this_to_object
                ElseIf istype(Of T2, IComparable)() Then
                    c = AddressOf that_to_object
                ElseIf comparer(Of T, T2).defined() Then
                    c = comparer(Of T, T2).ref()
                ElseIf contains_object Then
                    'contains object does not mean we need to use_runtime_compare,
                    'T or T2 may implement IComparable or IComparable(Of Object)
                    use_runtime_compare = True
                Else
                    If Not suppress_compare_error() Then
                        raise_error(error_type.exclamation,
                                      "caught types do not have IComparable implement, ",
                                      "unable to compare ",
                                      GetType(T).FullName(), " with ", GetType(T2).FullName())
                    End If
                    c = Nothing
                End If
            End If
        End Sub

        Private Shared Function object_comparable(ByVal this As T, ByVal that As T2) As Boolean
            Return use_object_compare OrElse
                   (contains_object AndAlso connector.object_comparable(this, that))
        End Function

        Private Shared Function comparable_cache_usable() As Boolean
            Return Not contains_object AndAlso
                   ca.HasValue()
        End Function

        Private Shared Function cached_compare(ByVal this As T,
                                               ByVal that As T2,
                                               ByRef o As Int32,
                                               ByVal check_only As Boolean) As Boolean
            'the Nullable<> could be null, which is not expected since it's ValueType
            'but the Nullable<> has been specifically treated as use_runtime_compare,
            'so it will not hit the following line
            assert(Not this Is Nothing AndAlso Not that Is Nothing)
            If c Is Nothing Then
                Return False
            ElseIf Not comparable_cache_usable() OrElse
                   ca.Value() Then
                If comparable_cache_usable() AndAlso
                   check_only Then
                    Return assert(ca.Value())
                Else
                    Try
                        o = c(this, that)
                        ca = True
                        Return True
                    Catch ex As Exception
                        compare_error(Of T, T2)(ex)
                        ca = False
                        Return False
                    End Try
                End If
            Else
                Return False
            End If
        End Function

        Private Shared Function compare(ByVal this As T,
                                        ByVal that As T2,
                                        ByRef o As Int32,
                                        ByVal check_only As Boolean) As Boolean
            If object_comparable(this, that) Then
                Dim cmp As Int32 = 0
                cmp = object_compare(this, that)
                If cmp <> object_compare_undetermined Then
                    o = cmp
                    Return True
                End If
            End If

#If DEBUG Then
            If Not use_runtime_compare Then
                Dim c1 As Int32 = 0
                Dim c2 As Int32 = 0
                assert(cached_compare(this, that, c1, False) = runtime_compare(this, that, c2) AndAlso
                       c1 = c2,
                       "need specific treatment to compare type ",
                       GetType(T).FullName(),
                       " with ",
                       GetType(T2).FullName())
            End If
#End If
            'special treatment for object,
            'since say int, it is comparable with int as object, but not comparable with long as object
            If use_runtime_compare Then
                Return runtime_compare(this, that, o)
            Else
                Return cached_compare(this, that, o, check_only)
            End If
        End Function

        Public Shared Function compare(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
            Return compare(this, that, o, False)
        End Function

        Public Shared Function comparable(ByVal this As T, ByVal that As T2) As Boolean
            Return compare(this, that, Nothing, True)
        End Function
    End Structure

    Public Function compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        Return compare_cache(Of T, T2).compare(this, that, o)
    End Function

    Public Function comparable(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return compare_cache(Of T, T2).comparable(this, that)
    End Function
#Else
    Public Function compare(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Int32) As Boolean
        If object_comparable(this, that) Then
            Dim c As Int32 = 0
            c = object_compare(this, that)
            If c <> object_compare_undetermined Then
                o = c
                Return True
            End If
        End If
        assert(Not this Is Nothing AndAlso Not that Is Nothing)
        Return runtime_compare(this, that, o)
    End Function

    Public Function comparable(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return compare(this, that, Nothing)
    End Function
#End If

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
        Return String.Compare(this, that)
    End Function

    Public Function equals(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return compare(this, that) = 0
    End Function

    Public Function equals(Of T)(ByVal this As T, ByVal that As T) As Boolean
        Return compare(Of T, T)(this, that) = 0
    End Function

    Public Function equals(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return compare(Of T)(this, that) = 0
    End Function

    Private Function operatorEqualImpl(Of T, T2)(ByVal this As T, ByVal that As T2, _
                                                 ByVal compare As Func(Of T, T2, Int32)) As Boolean
        assert(Not compare Is Nothing, "compare is a nothing compareDelegate.")
        If this Is Nothing AndAlso that Is Nothing Then
            Return True
        ElseIf this Is Nothing OrElse that Is Nothing Then
            Return False
        Else
            Return compare(this, that) = 0
        End If
    End Function

    'save a call to compare(Of T,T2), and IComparable(Of Object)
    Public Function operatorEqual(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return operatorEqualImpl(this, that, AddressOf compare(Of T))
    End Function

    Public Function operatorUnequal(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return Not operatorEqual(this, that)
    End Function

    Public Function operatorEqual(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return operatorEqualImpl(this, that, AddressOf compare(Of T, T2))
    End Function

    Public Function operatorUnequal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return Not operatorEqual(this, that)
    End Function

    Private Function operatorLessImpl(Of T, T2)(ByVal this As T, ByVal that As T2, _
                                                ByVal compare As Func(Of T, T2, Int32)) As Boolean
        assert(Not compare Is Nothing)
        If this Is Nothing AndAlso that Is Nothing Then
            Return False
        ElseIf this Is Nothing Then
            Return True
        ElseIf that Is Nothing Then
            Return False
        Else
            Return compare(this, that) < 0
        End If
    End Function

    Public Function operatorLess(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return operatorLessImpl(this, that, AddressOf compare(Of T))
    End Function

    Public Function operatorMore(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return Not operatorLess(this, that)
    End Function

    Public Function operatorLess(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return operatorLessImpl(this, that, AddressOf compare(Of T, T2))
    End Function

    Public Function operatorMore(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return Not operatorLess(this, that)
    End Function
End Module
