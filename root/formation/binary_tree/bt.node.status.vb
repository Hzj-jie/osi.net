
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class bt(Of T)
    Partial Protected Friend Class node
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function box() As box
            Return b
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_heighted_box() As Boolean
            Return Not hb Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function heighted_box() As heighted_box
#If Not Performance Then
            assert(is_heighted_box())
#End If
            Return hb
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_weighted_box() As Boolean
            Return Not wb Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function weighted_box() As weighted_box
#If Not Performance Then
            assert(is_weighted_box())
#End If
            Return wb
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_range_box() As Boolean
            Return Not rb Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function range_box() As range_box
#If Not Performance Then
            assert(is_range_box())
#End If
            Return rb
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_boxed(ByVal b As box) As Boolean
            Return object_compare(Me.b, b) = 0
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function value() As T
            Return b.v
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function left_child() As node
            Return l
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function has_left_child() As Boolean
            Return Not l Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function right_child() As node
            Return r
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function has_right_child() As Boolean
            Return Not r Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function parent() As node
            Return p
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function has_parent() As Boolean
            Return Not is_root()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_root() As Boolean
            Return p Is Nothing
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_leaf() As Boolean
            Return Not has_left_child() AndAlso
                   Not has_right_child()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_parent_of(ByVal n As node) As Boolean
            Return Not n Is Nothing AndAlso
                   (left_linked(n) OrElse
                    right_linked(n))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function parent_is(ByVal n As node) As Boolean
            Return Not n Is Nothing AndAlso
                   parent_linked(n)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_left_subtree_of(ByVal n As node) As Boolean
            Return parent_is(n) AndAlso
                   assert(Not n Is Nothing) AndAlso
                   n.left_linked(Me)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_left_subtree() As Boolean
            Return has_parent() AndAlso
                   is_left_subtree_of(parent())
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_right_subtree_of(ByVal n As node) As Boolean
            Return parent_is(n) AndAlso
                   assert(Not n Is Nothing) AndAlso
                   n.right_linked(Me)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_right_subtree() As Boolean
            Return has_parent() AndAlso
                   is_right_subtree_of(parent())
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub debug_assert_structure()
#If Not Performance Then
            If binary_tree_debug Then
                assert_structure()
            End If
#End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub assert_structure()
            assert(left_linked(left_child()))
            assert(right_linked(right_child()))
            assert(parent_linked(parent()))
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_isolated() As Boolean
            Return Not has_parent() AndAlso
                   Not has_left_child() AndAlso
                   Not has_right_child()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function left_child_count() As UInt32
            Return If(has_left_child(), left_child().subtree_node_count(), uint32_0)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function right_child_count() As UInt32
            Return If(has_right_child(), right_child().subtree_node_count(), uint32_0)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function subtree_node_count() As UInt32
            Return left_child_count() + right_child_count() + uint32_1
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function left_subtree_height() As UInt32
            Return If(has_left_child(), left_child().subtree_height(), uint32_0)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function right_subtree_height() As UInt32
            Return If(has_right_child(), right_child().subtree_height(), uint32_0)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function subtree_height() As UInt32
            Return _minmax.max(left_subtree_height(), right_subtree_height()) + uint32_1
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Function boxed_left_subtree_height() As UInt32
            Return heighted_box().left_height
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Function boxed_right_subtree_height() As UInt32
            Return heighted_box().right_height
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Function boxed_subtree_height() As UInt32
            Return _minmax.max(boxed_left_subtree_height(), boxed_right_subtree_height()) + uint32_1
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function balance_factor() As Int32
            If binary_tree_debug Then
                assert(boxed_left_subtree_height() = left_subtree_height())
                assert(boxed_right_subtree_height() = right_subtree_height())
            End If
            Return CInt(boxed_left_subtree_height()) - CInt(boxed_right_subtree_height())
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function range_min() As Int64
            Return range_box().min
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function range_max() As Int64
            Return range_box().max
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function median() As Int64
            Return range_box().median
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function left_child_range_min() As Int64
            Return range_min()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function left_child_range_max() As Int64
            Return median()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function right_child_range_min() As Int64
            Return median() + 1
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function right_child_range_max() As Int64
            Return range_max()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function cover(ByVal i As Int64) As Boolean
            Return i >= range_min() AndAlso i <= range_max()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function cover(ByVal min As Int64, ByVal max As Int64) As Boolean
            Return min <= max AndAlso
                   range_min() <= min AndAlso
                   range_max() >= max
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_covered_by(ByVal min As Int64, ByVal max As Int64) As Boolean
            Return min <= max AndAlso
                   min <= range_min() AndAlso
                   max >= range_max()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function equal(ByVal min As Int64, ByVal max As Int64) As Boolean
            Return min = range_min() AndAlso max = range_max()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function subnodes_equal() As Boolean
            assert(has_left_child())
            assert(has_right_child())
            Return left_child().compare(right_child().value()) = 0
        End Function
    End Class
End Class
