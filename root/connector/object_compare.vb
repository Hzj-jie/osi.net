
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

<global_init(global_init_level.runtime_checkers)>
Public Module _object_compare
    Public Const object_compare_undetermined As Int32 = 2
    Private Const object_compare_equal As Int32 = 0
    Private Const object_compare_larger As Int32 = 1
    Private Const object_compare_less As Int32 = -1

    Private Sub init()
        assert(object_compare_less = npos)
        assert(object_compare_larger = 1)
        assert(object_compare_equal = 0)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function object_comparable(ByVal this_is_value_type As Boolean,
                                       ByVal that_is_value_type As Boolean) As Boolean
        Return Not (this_is_value_type OrElse that_is_value_type)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function object_comparable(Of T, T2)() As Boolean
        Return object_comparable(type_info(Of T).is_valuetype, type_info(Of T2).is_valuetype)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function object_comparable(ByVal this As Object, ByVal that As Object) As Boolean
        Return object_comparable(TypeOf this Is ValueType, TypeOf that Is ValueType)
    End Function

    ' Should use next object_compare() function.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function object_compare(ByVal this As Object, ByVal that As Object) As Int32
        If Object.ReferenceEquals(this, that) Then
            Return object_compare_equal
        End If
        If this Is Nothing Then
            Return object_compare_less
        End If
        If that Is Nothing Then
            Return object_compare_larger
        End If
        Return object_compare_undetermined
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function object_compare(ByVal this As Object, ByVal that As Object, ByRef o As Int32) As Boolean
        If Object.ReferenceEquals(this, that) Then
            o = object_compare_equal
            Return True
        End If
        If this Is Nothing Then
            o = object_compare_less
            Return True
        End If
        If that Is Nothing Then
            o = object_compare_larger
            Return True
        End If
        Return False
    End Function
End Module
