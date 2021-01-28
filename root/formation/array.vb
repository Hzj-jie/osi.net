
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class array
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal v() As T) As array(Of T)
        Return New array(Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function elements(Of T)(ByVal ParamArray v() As T) As array(Of T)
        assert(array_size(v) > 1 OrElse Not type_info(Of T).is_array)
        Return New array(Of T)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
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

Public NotInheritable Class array(Of T)
    Inherits const_array(Of T)
    Implements ICloneable(Of array(Of T)), ICloneable

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal size As Int64)
        MyBase.New(size)
    End Sub

    <copy_constructor>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal v() As T)
        MyBase.New(v)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal i As array(Of T)) As array(Of T)
        Return const_array(Of T).move(Of array(Of T))(i)
    End Function

    Default Public Shadows Property data(ByVal i As UInt32) As T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            Return MyBase.data(i)
        End Get
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Set(ByVal v As T)
            assert(i < size())
            Me.v(CInt(i)) = v
        End Set
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function as_array() As T()
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As array(Of T) Implements ICloneable(Of array(Of T)).Clone
        Return MyBase.clone(Of array(Of T))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Widening Operator CType(ByVal this As array(Of T)) As T()
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.as_array()
    End Operator
End Class
