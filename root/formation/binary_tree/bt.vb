
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports cc = osi.root.connector

Friend Module binary_tree
    Public Const binary_tree_debug As Boolean = False
End Module

Partial Public Class bt(Of T)
    Implements IComparable, IComparable(Of bt(Of T))

    Protected root As node

    Public Shared Function move(ByVal v As bt(Of T)) As bt(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As bt(Of T) = Nothing
            r = New bt(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Protected Shared Sub move_to(ByVal from As bt(Of T), ByVal [to] As bt(Of T))
        assert(Not from Is Nothing)
        assert(Not [to] Is Nothing)
        [to].root = from.root
        from.root = Nothing
    End Sub

    'make sure the tree from root <r> is not a graph
    Private Shared Sub assert_structure(ByVal r As node, ByVal v As vector(Of node))
        assert(Not r Is Nothing)
        assert(Not v Is Nothing)
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

    Protected Shared Sub assert_structure(ByVal r As node)
        assert(Not r Is Nothing)
        Dim v As vector(Of node) = Nothing
        v = New vector(Of node)()
        assert_structure(r, v)
    End Sub

    Public Function [end]() As iterator
        Return iterator.end
    End Function

    Public Function rend() As iterator
        Return iterator.end
    End Function

    Public Function begin() As iterator
        Return If(empty(), [end](), New iterator(root.min()))
    End Function

    Public Function rbegin() As iterator
        Return If(empty(), rend(), New iterator(root.max()))
    End Function

    Public Function size() As UInt32
        Return If(empty(), uint32_0, root.subtree_node_count())
    End Function

    Public Function count() As UInt32
        Return size()
    End Function

    Public Function empty() As Boolean
        Return root Is Nothing
    End Function

    Public Sub clear()
        root = Nothing
    End Sub

    Public Sub preorder_traversal(ByVal s As StringBuilder)
        If Not root Is Nothing Then
            root.preorder_traversal(s)
        End If
    End Sub

    Public Function preorder_traversal() As String
        If root Is Nothing Then
            Return Nothing
        Else
            Return root.preorder_traversal()
        End If
    End Function

    Public Sub inorder_traversal(ByVal s As StringBuilder)
        If Not root Is Nothing Then
            root.inorder_traversal(s)
        End If
    End Sub

    Public Function inorder_traversal() As String
        If root Is Nothing Then
            Return Nothing
        Else
            Return root.inorder_traversal()
        End If
    End Function

    Public Sub postorder_traversal(ByVal s As StringBuilder)
        If Not root Is Nothing Then
            root.postorder_traversal(s)
        End If
    End Sub

    Public Function postorder_traversal() As String
        If root Is Nothing Then
            Return Nothing
        Else
            Return root.postorder_traversal()
        End If
    End Function

    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT,
                                      ByVal size As Func(Of BTT, UInt32),
                                      ByVal cmp As Func(Of T, T, Int32)) As Int32
        assert(Not cmp Is Nothing)
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c = object_compare_undetermined Then
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            If Not size Is Nothing Then
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
                If c = 0 Then
                    i += 1
                    j += 1
                Else
                    Return c
                End If
            End While
            If i = this.end() AndAlso j = that.end() Then
                Return 0
            ElseIf i = this.end() Then
                Return -1
            ElseIf j = that.end() Then
                Return 1
            Else
                assert(False)
                Return npos
            End If
        Else
            Return c
        End If
    End Function

    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT,
                                      ByVal size As Func(Of BTT, UInt32)) As Int32
        Return compare(Of BTT)(this, that, size, AddressOf cc.compare)
    End Function

    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT,
                                      ByVal cmp As Func(Of T, T, Int32)) As Int32
        Return compare(Of BTT)(this, that, Nothing, cmp)
    End Function

    Protected Shared Function compare(Of BTT As bt(Of T)) _
                                     (ByVal this As BTT,
                                      ByVal that As BTT) As Int32
        Return compare(Of BTT)(this, that, Nothing, AddressOf cc.compare)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of bt(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal other As bt(Of T)) As Int32 Implements IComparable(Of bt(Of T)).CompareTo
        Return compare(Me, other)
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return preorder_traversal()
    End Function
End Class
