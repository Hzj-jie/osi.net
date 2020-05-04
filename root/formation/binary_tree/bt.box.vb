
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class bt(Of T)
    Protected Friend Class box
        Public ReadOnly v As T

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Protected Sub New(ByVal v As T)
            Me.v = v
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Function create_node(ByVal v As T, ByVal cmp As Func(Of T, T, Int32)) As node
            Return New node(New box(v), cmp)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Function copy_node(ByVal n As node, ByVal cmp As Func(Of T, T, Int32)) As node
            assert(Not n Is Nothing)
            Return create_node(n.value(), cmp)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Function create_node(ByVal v As T) As node
            Return New node(New box(v))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Function copy_node(ByVal n As node) As node
            assert(Not n Is Nothing)
            Return create_node(n.value())
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Overrides Function ToString() As String
            Return Convert.ToString(v)
        End Function
    End Class

    Protected Friend Class weighted_box
        Inherits box

        Public left_weight As UInt32
        Public right_weight As UInt32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal v As T)
            MyBase.New(v)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Shadows Function create_node(ByVal v As T, ByVal cmp As Func(Of T, T, Int32)) As node
            Return New node(New weighted_box(v), cmp)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Shadows Function create_node(ByVal v As T) As node
            Return New node(New weighted_box(v))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Overrides Function ToString() As String
            Return strcat("[", left_weight, ", ", right_weight, "] - ", v)
        End Function
    End Class

    Protected Friend Class heighted_box
        Inherits box

        Public left_height As UInt32
        Public right_height As UInt32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal v As T, ByVal left_height As UInt32, ByVal right_height As UInt32)
            MyBase.New(v)
            Me.left_height = left_height
            Me.right_height = right_height
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal v As T)
            Me.New(v, 0, 0)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal n As node)
            Me.New(n.value(), n.heighted_box().left_height, n.heighted_box().right_height)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Shadows Function create_node(ByVal v As T, ByVal cmp As Func(Of T, T, Int32)) As node
            Return New node(New heighted_box(v), cmp)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Shadows Function copy_node(ByVal n As node, ByVal cmp As Func(Of T, T, Int32)) As node
            assert(Not n Is Nothing)
            Return New node(New heighted_box(n), cmp)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Shadows Function create_node(ByVal v As T) As node
            Return New node(New heighted_box(v))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Shadows Function copy_node(ByVal n As node) As node
            assert(Not n Is Nothing)
            Return New node(New heighted_box(n))
        End Function

        'make sure all the height recorded in heighted_box are correct
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Sub assert_height(ByVal r As node)
            assert(Not r Is Nothing)
            assert(r.left_subtree_height() = r.heighted_box().left_height)
            assert(r.right_subtree_height() = r.heighted_box().right_height)
            If r.has_left_child() Then
                assert_height(r.left_child())
            End If
            If r.has_right_child() Then
                assert_height(r.right_child())
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Sub debug_assert_height(ByVal r As node)
            If binary_tree_debug Then
                assert_height(r)
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Overrides Function ToString() As String
            Return strcat("[", left_height, ", ", right_height, "] - ", v)
        End Function
    End Class

    Protected Friend Class range_box
        Inherits box

        Public ReadOnly min As Int64
        Public ReadOnly max As Int64
        Public ReadOnly median As Int64

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal min As Int64, ByVal max As Int64, ByVal v As T)
            MyBase.New(v)
            Me.min = min
            Me.max = max
            Me.median = (min >> 1) + (max >> 1) + If(min.odd() AndAlso max.odd(), 1, 0)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Overrides Function ToString() As String
            Return strcat("[", min, ", (", median, "), ", max, "] - ", v)
        End Function
    End Class
End Class
