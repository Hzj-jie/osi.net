
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Module _const_array
    <Extension()> Public Function null_or_empty(Of T)(ByVal this As const_array(Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function
End Module

Public NotInheritable Class const_array
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal v() As T) As const_array(Of T)
        Return New const_array(Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function elements(Of T)(ByVal ParamArray v() As T) As const_array(Of T)
#If Not Performance Then
        assert(array_size(v) > 1 OrElse Not type_info(Of T).is_array)
#End If
        Return New const_array(Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function compare(Of T)(ByVal this As const_array(Of T),
                                         ByVal that As const_array(Of T)) As Int32
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function alloc_of(Of T)(ByVal f As Func(Of T), ByVal size As UInt32) As const_array(Of T)
#If Not Performance Then
        assert(Not f Is Nothing)
        assert(size > 0)
        assert(size <= max_int32)
#End If
        Dim r(CInt(size - uint32_1)) As T
        For i As Int32 = 0 To CInt(size) - 1
            r(i) = f()
        Next
        Return [of](r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function repeat_of(Of T)(ByVal v As T, ByVal size As UInt32) As const_array(Of T)
        Return alloc_of(Function() As T
                            Return v
                        End Function,
                        size)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class const_array(Of T)
    Implements ICloneable,
               ICloneable(Of const_array(Of T)),
               IComparable,
               IComparable(Of const_array(Of T)),
               IEquatable(Of const_array(Of T))
    Protected ReadOnly v() As T

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub New(ByVal size As Int64)
#If Not Performance Then
        assert(size > 0)
#End If
        ReDim v(CInt(size) - 1)
    End Sub

    <copy_constructor>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal v() As T)
        Me.v = v
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Function move(Of R As const_array(Of T))(ByVal i As R) As R
        If i Is Nothing Then
            Return Nothing
        End If

        Dim a(CInt(i.size()) - 1) As T
        Dim o As R = copy_constructor(Of R).invoke(a)
        arrays.copy(o.v, i.v)
        arrays.clear(i.v)
        Return o
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function move(ByVal i As const_array(Of T)) As const_array(Of T)
        Return move(Of const_array(Of T))(i)
    End Function

    Default Public ReadOnly Property at(ByVal i As UInt32) As T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
#If Not Performance Then
            assert(i < size())
#End If
            Return v(CInt(i))
        End Get
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Return array_size(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function as_array() As T()
        If empty() Then
            Dim e(-1) As T
            Return e
        End If
        Dim r(CInt(size() - uint32_1)) As T
        arrays.copy(r, v)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As const_array(Of T)) As T()
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.as_array()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this As const_array(Of T)) As T()
        Return +this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this() As T) As const_array(Of T)
        If isemptyarray(this) Then
            Return Nothing
        End If
        Return New const_array(Of T)(this)
    End Operator

    Protected Function clone(Of R As const_array(Of T))() As R
        Return copy_constructor(Of R).invoke(deep_clone(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As const_array(Of T) Implements ICloneable(Of const_array(Of T)).Clone
        Return clone(Of const_array(Of T))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of const_array(Of T))().from(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal other As const_array(Of T)) As Int32 _
            Implements IComparable(Of const_array(Of T)).CompareTo
        If other Is Nothing Then
            Return 1
        End If

        Return memcmp(Me.v, other.v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal other As const_array(Of T)) As Boolean _
            Implements IEquatable(Of const_array(Of T)).Equals
        Return CompareTo(other) = 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public NotOverridable Overrides Function Equals(ByVal other As Object) As Boolean
        Return Equals(cast(Of const_array(Of T)()).from(other, False))
    End Function

    Public NotOverridable Overrides Function GetHashCode() As Int32
        Return v.hash()
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return v.to_string(size())
    End Function
End Class
