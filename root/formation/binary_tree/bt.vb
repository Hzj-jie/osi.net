
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Friend Module binary_tree
    Public Const binary_tree_debug As Boolean = False
End Module

Partial Public Class bt(Of T)
    Implements IComparable, IComparable(Of bt(Of T))

    Protected root As node

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function move(ByVal v As bt(Of T)) As bt(Of T)
        If v Is Nothing Then
            Return Nothing
        End If
        Dim r As bt(Of T) = Nothing
        r = New bt(Of T)()
        move_to(v, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Sub move_to(ByVal from As bt(Of T), ByVal [to] As bt(Of T))
#If Not Performance Then
        assert(from IsNot Nothing)
        assert([to] IsNot Nothing)
#End If
        [to].root = from.root
        from.root = Nothing
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function swap(ByVal i As bt(Of T), ByVal j As bt(Of T)) As Boolean
        If i Is Nothing OrElse j Is Nothing Then
            Return False
        End If
        _swap.swap(i.root, j.root)
        Return True
    End Function

    'make sure the tree from root <r> is not a graph
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Sub assert_structure(ByVal r As node, ByVal v As vector(Of node))
        assert(r IsNot Nothing)
        assert(v IsNot Nothing)
        For i As UInt32 = 0 To v.size() - uint32_1
            assert(object_compare(v(i), r) <> 0)
        Next
        v.emplace_back(r)
        If r.has_left_child() Then
            assert_structure(r.left_child(), v)
        End If
        If r.has_right_child() Then
            assert_structure(r.right_child(), v)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Sub assert_structure(ByVal r As node)
        assert(r IsNot Nothing)
        assert_structure(r, New vector(Of node)())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [end]() As iterator
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function rend() As iterator
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function begin() As iterator
        Return If(empty(), [end](), New iterator(root.min()))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function rbegin() As iterator
        Return If(empty(), rend(), New iterator(root.max()))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Return If(empty(), uint32_0, root.subtree_node_count())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function count() As UInt32
        Return size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return root Is Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        root = Nothing
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub preorder_traversal(ByVal s As StringBuilder)
        If root IsNot Nothing Then
            root.preorder_traversal(s)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function preorder_traversal() As String
        If root Is Nothing Then
            Return Nothing
        End If
        Return root.preorder_traversal()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub inorder_traversal(ByVal s As StringBuilder)
        If root IsNot Nothing Then
            root.inorder_traversal(s)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function inorder_traversal() As String
        If root Is Nothing Then
            Return Nothing
        End If
        Return root.inorder_traversal()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub postorder_traversal(ByVal s As StringBuilder)
        If root IsNot Nothing Then
            root.postorder_traversal(s)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function postorder_traversal() As String
        If root Is Nothing Then
            Return Nothing
        End If
        Return root.postorder_traversal()
    End Function

    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT,
                                      ByVal size As Func(Of BTT, UInt32),
                                      ByVal cmp As Func(Of T, T, Int32)) As Int32
#If Not Performance Then
        assert(cmp IsNot Nothing)
#End If
        Dim c As Int32 = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If
#If Not Performance Then
        assert(this IsNot Nothing)
        assert(that IsNot Nothing)
#End If
        If size IsNot Nothing Then
            Dim l As UInt32 = 0
            Dim r As UInt32 = 0
            l = size(this)
            r = size(that)
            If l < r Then
                Return -1
            ElseIf l > r Then
                Return 1
            End If
        End If
        Dim i As iterator = Nothing
        Dim j As iterator = Nothing
        i = this.begin()
        j = that.begin()
        While i <> this.end() AndAlso
                  j <> that.end()
            c = cmp(i.value(), j.value())
            If c <> 0 Then
                Return c
            End If
            i += 1
            j += 1
        End While
        If i = this.end() AndAlso j = that.end() Then
            Return 0
        End If
        If i = this.end() Then
            Return -1
        End If
        If j = that.end() Then
            Return 1
        End If
        assert(False)
        Return npos
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT,
                                      ByVal size As Func(Of BTT, UInt32)) As Int32
        Return compare(Of BTT)(this, that, size, AddressOf _compare.compare)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT,
                                      ByVal cmp As Func(Of T, T, Int32)) As Int32
        Return compare(Of BTT)(this, that, Nothing, cmp)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT) As Int32
        Return compare(Of BTT)(this, that, Nothing, AddressOf _compare.compare)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of bt(Of T))(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal other As bt(Of T)) As Int32 Implements IComparable(Of bt(Of T)).CompareTo
        Return compare(Me, other)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public NotOverridable Overrides Function ToString() As String
        Return preorder_traversal()
    End Function
End Class
