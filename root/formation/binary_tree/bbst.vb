
Option Explicit On
Option Infer Off
Option Strict On

#Const use_rebalance = False
Imports osi.root.connector

'just the AVL tree
Public Class bbst(Of T)
    Inherits obst(Of T)

    Public Sub New()
        MyBase.New(AddressOf heighted_box.create_node, AddressOf heighted_box.copy_node)
    End Sub

    Protected Sub New(ByVal cmp As Func(Of T, T, Int32))
        MyBase.New(Function(x As T) As node
                       Return heighted_box.create_node(x, cmp)
                   End Function,
                   Function(x As node) As node
                       Return heighted_box.copy_node(x, cmp)
                   End Function)
    End Sub

    Public Shared Shadows Function move(ByVal v As bbst(Of T)) As bbst(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As bbst(Of T) = Nothing
            r = New bbst(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As bbst(Of T)
        Dim r As bbst(Of T) = Nothing
        r = New bbst(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Private Shared Function left_rotate(ByVal n As node) As node
        assert(Not n Is Nothing)
        Dim l As node = Nothing
        l = n.left_rotate()
        n.revise_or_clear_right_subtree_height()
        l.revise_left_subtree_height()
        Return l
    End Function

    Private Shared Function right_rotate(ByVal n As node) As node
        assert(Not n Is Nothing)
        Dim r As node = Nothing
        r = n.right_rotate()
        n.revise_or_clear_left_subtree_height()
        r.revise_right_subtree_height()
        Return r
    End Function

    Private Shared Sub debug_assert_balance(ByVal n As node)
        If binary_tree_debug Then
            assert_balance(n)
        End If
    End Sub

    Private Shared Sub assert_balance(ByVal n As node)
        assert(Not n Is Nothing)
        assert(Math.Abs(n.balance_factor()) <= 1)
        If n.has_left_child() Then
            assert_balance(n.left_child())
        End If
        If n.has_right_child() Then
            assert_balance(n.right_child())
        End If
    End Sub

    Private Shared Sub rebalance(ByVal n As node, ByRef root As node)
        assert(Not n Is Nothing)
        While Not n.is_root()
            assert(Not n Is Nothing)
            n.debug_assert_structure()
            Dim p As node = Nothing
            p = n.parent()
            assert(Not p Is Nothing)
            If n.is_left_subtree() Then
                p.revise_left_subtree_height()
            Else
                assert(n.is_right_subtree())
                p.revise_right_subtree_height()
            End If
            If p.balance_factor() = 2 Then
                assert(p.has_left_child())
                Dim l As node = Nothing
                l = p.left_child()
                If l.balance_factor() = -1 Then
                    left_rotate(l)
                End If
                n = right_rotate(p)
                debug_assert_balance(n)
            ElseIf p.balance_factor() = -2 Then
                assert(p.has_right_child())
                Dim r As node = Nothing
                r = p.right_child()
                If r.balance_factor() = 1 Then
                    right_rotate(r)
                End If
                n = left_rotate(p)
                debug_assert_balance(n)
            Else
                assert(Math.Abs(p.balance_factor()) <= 1)
                n = p
                debug_assert_balance(n)
            End If
        End While
        root = n
        root.clear_parent()
        heighted_box.debug_assert_height(root)
        debug_assert_balance(root)
    End Sub

    Private Shared Sub rebalance2(ByVal n As node, ByRef root As node)
        assert(Not n Is Nothing)
        n.debug_assert_structure()
        Dim c As node = Nothing
        While True
            assert(Not n Is Nothing)
            n.debug_assert_structure()
            If Not c Is Nothing Then
                If Not c.revise_parent_subtree_height() Then
                    Exit While
                End If
            End If
            If n.balance_factor() = 2 Then
                assert(n.has_left_child())
                Dim l As node = Nothing
                l = n.left_child()
                If l.balance_factor() = -1 Then
                    left_rotate(l)
                End If
                n = right_rotate(n)
                debug_assert_balance(n)
            ElseIf n.balance_factor() = -2 Then
                assert(n.has_right_child())
                Dim r As node = Nothing
                r = n.right_child()
                If r.balance_factor() = 1 Then
                    right_rotate(r)
                End If
                n = left_rotate(n)
                debug_assert_balance(n)
            Else
                assert(Math.Abs(n.balance_factor()) <= 1)
                debug_assert_balance(n)
            End If

            assert(Not n Is Nothing)
            If n.has_parent() Then
                c = n
                n = n.parent()
            Else
                root = n
                root.clear_parent()
                Exit While
            End If
        End While
        assert(Not root Is Nothing)
        heighted_box.debug_assert_height(root)
        debug_assert_balance(root)
    End Sub

    Private Sub rebalance(ByVal n As node)
        rebalance(n, root)
    End Sub

    Private Sub rebalance2(ByVal n As node)
        rebalance2(n, root)
    End Sub

    Private Sub rebalance(ByVal r As pair(Of iterator, Boolean))
        assert(Not r Is Nothing)
        If r.second Then
#If use_rebalance Then
            rebalance(r.first.node())
#Else
            If r.first.node().has_parent() AndAlso r.first.node().revise_parent_subtree_height() Then
                rebalance2(r.first.node().parent())
            End If
#End If
        End If
    End Sub

    Public Shadows Function emplace_hint(ByVal it As iterator, ByVal v As T) As pair(Of iterator, Boolean)
        Dim r As pair(Of iterator, Boolean) = Nothing
        r = MyBase.emplace_hint(it, v)
        rebalance(r)
        Return r
    End Function

    Public Shadows Function emplace(ByVal v As T) As pair(Of iterator, Boolean)
        Dim r As pair(Of iterator, Boolean) = Nothing
        r = MyBase.emplace(v)
        rebalance(r)
        Return r
    End Function

    Public Shadows Function insert_hint(ByVal it As iterator, ByVal v As T) As pair(Of iterator, Boolean)
        Dim r As pair(Of iterator, Boolean) = Nothing
        r = MyBase.insert_hint(it, v)
        rebalance(r)
        Return r
    End Function

    Public Shadows Function insert(ByVal v As T) As pair(Of iterator, Boolean)
        Dim r As pair(Of iterator, Boolean) = Nothing
        r = MyBase.insert(v)
        rebalance(r)
        Return r
    End Function

    Public Shadows Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() Then
            Return False
        Else
            Dim p As node = Nothing
            Dim r As node = Nothing
            r = MyBase.[erase](it.node(), it.node().balance_factor() > 0, p)
            If Not r Is Nothing Then
                r.revise_or_clear_left_subtree_height()
                r.revise_or_clear_right_subtree_height()
            End If
            If Not p Is Nothing Then
#If use_rebalance Then
                'do not need to return whether the removed node is left / right of the p
                p.revise_or_clear_left_subtree_height()
                p.revise_or_clear_right_subtree_height()
                If p.has_left_child() Then
                    rebalance(p.left_child())
                ElseIf p.has_right_child() Then
                    rebalance(p.right_child())
                Else
                    rebalance(p)
                End If
#Else
                If p.revise_or_clear_left_subtree_height() OrElse
                   p.revise_or_clear_right_subtree_height() Then
                    rebalance2(p)
                End If
#End If
            End If
            Return True
        End If
    End Function

    Public Shadows Function [erase](ByVal v As T) As Boolean
        Return [erase](find(v))
    End Function
End Class
