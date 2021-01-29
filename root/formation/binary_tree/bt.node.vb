
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class bt(Of T)
    Partial Protected Friend Class node
        Private ReadOnly cmp As Func(Of T, T, Int32)
        Private ReadOnly b As box
        Private ReadOnly hb As heighted_box
        Private ReadOnly wb As weighted_box
        Private ReadOnly rb As range_box
        Private l As node
        Private r As node
        Private p As node

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal b As box)
            Me.New(b, AddressOf connector.compare)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal b As box, ByVal cmp As Func(Of T, T, Int32))
#If Not Performance Then
            assert(Not cmp Is Nothing)
#End If
            Me.cmp = cmp
            Me.b = b
            Me.hb = TryCast(b, heighted_box)
            Me.wb = TryCast(b, weighted_box)
            Me.rb = TryCast(b, range_box)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Function left_linked(ByVal n As node) As Boolean
            Return (n Is Nothing AndAlso
                    Not has_left_child()) OrElse
                   (Not n Is Nothing AndAlso
                    has_left_child() AndAlso
                    object_compare(left_child(), n) = 0 AndAlso
                    assert(left_child().has_parent()) AndAlso
                    assert(object_compare(left_child().parent(), Me) = 0))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Function right_linked(ByVal n As node) As Boolean
            Return (n Is Nothing AndAlso
                    Not has_right_child()) OrElse
                   (Not n Is Nothing AndAlso
                    has_right_child() AndAlso
                    object_compare(right_child(), n) = 0 AndAlso
                    assert(right_child().has_parent()) AndAlso
                    assert(object_compare(right_child().parent(), Me) = 0))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Function parent_linked(ByVal n As node) As Boolean
            Return (n Is Nothing AndAlso
                    Not has_parent()) OrElse
                   (Not n Is Nothing AndAlso
                    has_parent() AndAlso
                    object_compare(parent(), n) = 0 AndAlso
                    assert((n.has_left_child() AndAlso
                              object_compare(n.left_child(), Me) = 0) OrElse
                             (n.has_right_child() AndAlso
                              object_compare(n.right_child(), Me) = 0)))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub append_to_min(ByVal n As node)
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            min().replace_left(n)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub append_to_max(ByVal n As node)
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            max().replace_right(n)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub replace_left(ByVal n As node)
            l = n
            If Not n Is Nothing Then
                n.p = Me
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub replace_right(ByVal n As node)
            r = n
            If Not n Is Nothing Then
                n.p = Me
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub clear_left()
            l = Nothing
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub clear_right()
            r = Nothing
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub clear_parent()
            p = Nothing
        End Sub

        'return the min node of the subtree rooted by this node
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function min() As node
            Dim n As node = Me
            While n.has_left_child()
                n = n.left_child()
            End While
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            Return n
        End Function

        'return the max node of the subtree rooted by this node
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function max() As node
            Dim n As node = Me
            While n.has_right_child()
                n = n.right_child()
            End While
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            Return n
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub replace_parent_subtree(ByVal c As node)
            If has_parent() Then
                If is_left_subtree() Then
                    parent().replace_left(c)
                Else
#If Not Performance Then
                    assert(is_right_subtree())
#End If
                    parent().replace_right(c)
                End If
            ElseIf Not c Is Nothing Then
                c.clear_parent()
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub clear_parent_subtree()
            If Not has_parent() Then
                Return
            End If
            If is_left_subtree() Then
                parent().clear_left()
            Else
#If Not Performance Then
                assert(is_right_subtree())
#End If
                parent().clear_right()
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function replace_by(ByVal a As node, ByVal b As node) As node
#If Not Performance Then
            assert(Not a Is Nothing)
            assert(Not b Is Nothing)
            If binary_tree_debug Then
                If a.has_left_child() Then
                    assert(a.left_child().max().compare(b) < 0)
                End If
                If a.has_right_child() Then
                    assert(a.right_child().min().compare(b) > 0)
                End If
            End If
#End If
            b.replace_left(a.left_child())
            b.replace_right(a.right_child())
            a.replace_parent_subtree(b)

            b.debug_assert_structure()

            Return b
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function replace_by_left_subtree(ByVal a As node) As node
#If Not Performance Then
            assert(Not a Is Nothing)
            assert(a.has_left_child())
            assert(Not a.has_right_child() OrElse Not a.left_child().has_right_child())
#End If
            Dim b As node = a.left_child()
            a.replace_parent_subtree(b)
            If a.has_right_child() Then
                b.replace_right(a.right_child())
            End If

            b.debug_assert_structure()

            Return b
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function replace_by_right_subtree(ByVal a As node) As node
#If Not Performance Then
            assert(Not a Is Nothing)
            assert(a.has_right_child())
            assert(Not a.has_left_child() OrElse Not a.right_child().has_left_child())
#End If
            Dim b As node = a.right_child()
            a.replace_parent_subtree(b)
            If a.has_left_child() Then
                b.replace_left(a.left_child())
            End If

            b.debug_assert_structure()

            Return b
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function replace_by_left_max(ByVal a As node,
                                                    ByRef parent_of_replaced_node As node) As node
#If Not Performance Then
            assert(Not a Is Nothing)
            assert(a.has_left_child())
#End If
            Dim b As node = Nothing
            If a.left_child().has_right_child() Then
                b = a.left_child().max()
#If Not Performance Then
                assert(Not b Is Nothing)
                assert(Not b.has_right_child())
                assert(object_compare(b, a.left_child()) <> 0)
                assert(b.has_parent())
                assert(b.is_right_subtree())
#End If
                parent_of_replaced_node = b.parent()
                If b.has_left_child() Then
                    b.replace_by_left_subtree()
                Else
                    b.parent().clear_right()
                End If
                b = a.replace_by(b)
            Else
                parent_of_replaced_node = a.parent()
                b = a.replace_by_left_subtree()
            End If
            b.debug_assert_structure()
            If Not parent_of_replaced_node Is Nothing Then
                parent_of_replaced_node.debug_assert_structure()
            End If

            Return b
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function replace_by_right_min(ByVal a As node,
                                                     ByRef parent_of_replaced_node As node) As node
#If Not Performance Then
            assert(Not a Is Nothing)
            assert(a.has_right_child())
#End If
            Dim b As node = Nothing
            If a.right_child().has_left_child() Then
                b = a.right_child().min()
#If Not Performance Then
                assert(Not b Is Nothing)
                assert(Not b.has_left_child())
                assert(object_compare(b, a.right_child()) <> 0)
                assert(b.has_parent())
                assert(b.is_left_subtree())
#End If
                parent_of_replaced_node = b.parent()
                If b.has_right_child() Then
                    b.replace_by_right_subtree()
                Else
                    b.parent().clear_left()
                End If
                b = a.replace_by(b)
            Else
                parent_of_replaced_node = a.parent()
                b = a.replace_by_right_subtree()
            End If

            b.debug_assert_structure()
            If Not parent_of_replaced_node Is Nothing Then
                parent_of_replaced_node.debug_assert_structure()
            End If

            Return b
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function [erase](ByVal n As node,
                                        ByVal select_left As Boolean,
                                        ByRef parent_of_removed_node As node) As node
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            Dim r As node = Nothing
            If n.has_left_child() AndAlso n.has_right_child() Then
                If select_left Then
                    r = n.replace_by_left_max(parent_of_removed_node)
                Else
                    r = n.replace_by_right_min(parent_of_removed_node)
                End If
            Else
                If n.has_left_child() Then
                    r = n.left_child()
                ElseIf n.has_right_child() Then
                    r = n.right_child()
                Else
                    r = Nothing
                End If
                If n.has_parent() Then
                    parent_of_removed_node = n.parent()
                    n.replace_parent_subtree(r)
                Else
                    parent_of_removed_node = Nothing
                End If
            End If

            If Not r Is Nothing Then
                r.debug_assert_structure()
            End If
            If Not parent_of_removed_node Is Nothing Then
                parent_of_removed_node.debug_assert_structure()
            End If

            Return r
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function revise_parent_subtree_height() As Boolean
#If Not Performance Then
            assert(has_parent())
#End If
            If is_left_subtree() Then
                Return parent().revise_left_subtree_height()
            End If
#If Not Performance Then
            assert(is_right_subtree())
#End If
            Return parent().revise_right_subtree_height()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function revise_or_clear_left_subtree_height() As Boolean
            Dim n As UInt32 = 0
            n = If(has_left_child(), left_child().boxed_subtree_height(), uint32_0)
            If boxed_left_subtree_height() = n Then
                Return False
            End If
            heighted_box().left_height = n
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function revise_or_clear_right_subtree_height() As Boolean
            Dim n As UInt32 = 0
            n = If(has_right_child(), right_child().boxed_subtree_height(), uint32_0)
            If boxed_right_subtree_height() = n Then
                Return False
            End If
            heighted_box().right_height = n
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function revise_left_subtree_height() As Boolean
#If Not Performance Then
            assert(has_left_child())
#End If
            Return revise_or_clear_left_subtree_height()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function clear_left_subtree_height() As Boolean
#If Not Performance Then
            assert(Not has_left_child())
#End If
            Return revise_or_clear_left_subtree_height()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function revise_right_subtree_height() As Boolean
#If Not Performance Then
            assert(has_right_child())
#End If
            Return revise_or_clear_right_subtree_height()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function clear_right_subtree_height() As Boolean
#If Not Performance Then
            assert(Not has_right_child())
#End If
            Return revise_or_clear_right_subtree_height()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Sub begin_child_node(ByVal s As StringBuilder)
#If Not Performance Then
            assert(Not s Is Nothing)
#End If
            s.Append("{")
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Sub seperate_child_node(ByVal s As StringBuilder)
#If Not Performance Then
            assert(Not s Is Nothing)
#End If
            s.Append(", ")
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Sub end_child_node(ByVal s As StringBuilder)
#If Not Performance Then
            assert(Not s Is Nothing)
#End If
            s.Append("}")
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub preorder_traversal(ByVal s As StringBuilder)
#If Not Performance Then
            assert(Not s Is Nothing)
#End If
            s.Append(Convert.ToString(Me))
            begin_child_node(s)
            If has_left_child() Then
                left_child().preorder_traversal(s)
            End If
            seperate_child_node(s)
            If has_right_child() Then
                right_child().preorder_traversal(s)
            End If
            end_child_node(s)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function preorder_traversal() As String
            Dim s As StringBuilder = New StringBuilder()
            preorder_traversal(s)
            Return Convert.ToString(s)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub inorder_traversal(ByVal s As StringBuilder)
#If Not Performance Then
            assert(Not s Is Nothing)
#End If
            begin_child_node(s)
            If has_left_child() Then
                left_child().inorder_traversal(s)
            End If
            end_child_node(s)
            s.Append(Convert.ToString(Me))
            begin_child_node(s)
            If has_right_child() Then
                right_child().inorder_traversal(s)
            End If
            end_child_node(s)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function inorder_traversal() As String
            Dim s As StringBuilder = New StringBuilder()
            inorder_traversal(s)
            Return Convert.ToString(s)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub postorder_traversal(ByVal s As StringBuilder)
#If Not Performance Then
            assert(Not s Is Nothing)
#End If
            begin_child_node(s)
            If has_left_child() Then
                left_child().postorder_traversal(s)
            End If
            seperate_child_node(s)
            If has_right_child() Then
                right_child().postorder_traversal(s)
            End If
            end_child_node(s)
            s.Append(Convert.ToString(Me))
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function postorder_traversal() As String
            Dim s As StringBuilder = New StringBuilder()
            postorder_traversal(s)
            Return Convert.ToString(s)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Overrides Function ToString() As String
            Return strcat("(", box(), ")")
        End Function
    End Class
End Class
