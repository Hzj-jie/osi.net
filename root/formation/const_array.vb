
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Public Module _const_array
    <Extension()> Public Function null_or_empty(Of T, SIZE As _int64)(ByVal this As const_array(Of T, SIZE)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function
End Module

Public NotInheritable Class const_array
    Public Shared Function [of](Of T)(ByVal v() As T) As const_array(Of T)
        Return New const_array(Of T)(v)
    End Function

    Public Shared Function elements(Of T)(ByVal ParamArray v() As T) As const_array(Of T)
        assert(array_size(v) > 1 OrElse Not type_info(Of T).is_array)
        Return New const_array(Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function compare(Of T, __SIZE As _int64)(ByVal this As const_array(Of T, __SIZE),
                                                           ByVal that As const_array(Of T, __SIZE)) As Int32
        If this Is Nothing AndAlso that Is Nothing Then
            Return 0
        End If
        If this Is Nothing Then
            Return -1
        End If
        If that Is Nothing Then
            Return 1
        End If
        Return this.CompareTo(that)
    End Function

    Public Shared Function alloc_of(Of T)(ByVal f As Func(Of T), ByVal size As UInt32) As const_array(Of T)
        assert(Not f Is Nothing)
        assert(size > 0)
        assert(size <= max_int32)
        Dim r() As T = Nothing
        ReDim r(CInt(size - uint32_1))
        For i As Int32 = 0 To CInt(size) - 1
            r(i) = f()
        Next
        Return [of](r)
    End Function

    Public Shared Function repeat_of(Of T)(ByVal v As T, ByVal size As UInt32) As const_array(Of T)
        Return alloc_of(Function() As T
                            Return v
                        End Function,
                        size)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class const_array(Of T, __SIZE As _int64)
    Implements ICloneable,
               ICloneable(Of const_array(Of T, __SIZE)),
               IComparable,
               IComparable(Of const_array(Of T, __SIZE)),
               IEquatable(Of const_array(Of T, __SIZE))

    Private Shared ReadOnly _size As Int64
    Protected ReadOnly v() As T

    Shared Sub New()
        _size = +alloc(Of __SIZE)()
    End Sub

    Protected Sub New()
        Me.New(_size)
    End Sub

    Protected Sub New(ByVal size As Int64)
        assert(size > 0)
        assert(size = _size OrElse _size = npos)
        ReDim v(CInt(size) - 1)
    End Sub

    <copy_constructor>
    Public Sub New(ByVal v() As T)
        assert(array_size(v) = _size OrElse _size = npos)
        Me.v = v
    End Sub

    Protected Shared Function move(Of R As const_array(Of T, __SIZE))(ByVal i As R) As R
        If i Is Nothing Then
            Return Nothing
        End If

        Dim a() As T = Nothing
        ReDim a(CInt(i.size()) - 1)
        Dim o As R = Nothing
        o = copy_constructor(Of R).invoke(a)
        arrays.copy(o.v, i.v)
        arrays.clear(i.v)
        Return o
    End Function

    Public Shared Function move(ByVal i As const_array(Of T, __SIZE)) As const_array(Of T, __SIZE)
        Return move(Of const_array(Of T, __SIZE))(i)
    End Function

    Default Public ReadOnly Property data(ByVal i As UInt32) As T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
#If DEBUG Then
            assert(i < size())
#End If
            Return v(CInt(i))
        End Get
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        If _size < 0 Then
            Return array_size(v)
        End If
        Return CUInt(_size)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function as_array() As T()
        Dim r() As T = Nothing
        ReDim r(CInt(size() - uint32_1))
        arrays.copy(r, v)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this As const_array(Of T, __SIZE)) As T()
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.as_array()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this() As T) As const_array(Of T, __SIZE)
        If isemptyarray(this) Then
            Return Nothing
        End If
        Return New const_array(Of T, __SIZE)(this)
    End Operator

    Protected Function clone(Of R As const_array(Of T, __SIZE))() As R
        Return copy_constructor(Of R).invoke(deep_clone(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As const_array(Of T, __SIZE) Implements ICloneable(Of const_array(Of T, __SIZE)).Clone
        Return clone(Of const_array(Of T, __SIZE))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of const_array(Of T, __SIZE))().from(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal other As const_array(Of T, __SIZE)) As Int32 _
            Implements IComparable(Of const_array(Of T, __SIZE)).CompareTo
        If other Is Nothing Then
            Return 1
        End If

        Return memcmp(Me.v, other.v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal other As const_array(Of T, __SIZE)) As Boolean _
            Implements IEquatable(Of const_array(Of T, __SIZE)).Equals
        Return CompareTo(other) = 0
    End Function

    Public NotOverridable Overrides Function Equals(ByVal other As Object) As Boolean
        Return Equals(cast(Of const_array(Of T, __SIZE)()).from(other, False))
    End Function
End Class

Public Class const_array(Of T)
    Inherits const_array(Of T, _NPOS)
    Implements ICloneable(Of const_array(Of T)), ICloneable

    Public Sub New(ByVal size As Int64)
        MyBase.New(size)
    End Sub

    <copy_constructor>
    Public Sub New(ByVal v() As T)
        MyBase.New(v)
    End Sub

    Public Shared Shadows Function move(ByVal i As const_array(Of T)) As const_array(Of T)
        Return const_array(Of T, _NPOS).move(Of const_array(Of T))(i)
    End Function

    Protected Shadows Function clone(Of R As const_array(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As const_array(Of T) Implements ICloneable(Of const_array(Of T)).Clone
        Return clone(Of const_array(Of T))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Widening Operator CType(ByVal this() As T) As const_array(Of T)
        If isemptyarray(this) Then
            Return Nothing
        End If

        Return New const_array(Of T)(this)
    End Operator
End Class
