
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector

Public Class obst(Of T)
    Inherits bst(Of T)
    Implements IComparable, IComparable(Of obst(Of T))

    Private ReadOnly create_node As Func(Of T, node)
    Private ReadOnly copy_node As Func(Of node, node)
    Private s As UInt32

    Public Sub New()
        Me.New(AddressOf box.create_node, AddressOf box.copy_node)
    End Sub

    Protected Sub New(ByVal create_node As Func(Of T, node),
                      ByVal copy_node As Func(Of node, node))
        assert(Not create_node Is Nothing)
        assert(Not copy_node Is Nothing)
        Me.create_node = create_node
        Me.copy_node = copy_node
    End Sub

    Protected Sub New(ByVal cmp As Func(Of T, T, Int32))
        Me.New(Function(x As T) As node
                   Return box.create_node(x, cmp)
               End Function,
               Function(x As node) As node
                   Return box.copy_node(x, cmp)
               End Function)
    End Sub

    Public Shared Shadows Function move(ByVal v As obst(Of T)) As obst(Of T)
        If v Is Nothing Then
            Return Nothing
        End If
        Dim r As obst(Of T) = Nothing
        r = New obst(Of T)()
        move_to(v, r)
        Return r
    End Function

    Protected Shared Shadows Sub move_to(ByVal from As obst(Of T), ByVal [to] As obst(Of T))
        assert(Not from Is Nothing)
        assert(Not [to] Is Nothing)
        bst(Of T).move_to(from, [to])
        [to].s = from.s
        from.s = 0
    End Sub

    Public Function clone() As obst(Of T)
        Dim r As obst(Of T) = Nothing
        r = New obst(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Protected Sub clone_to(ByVal from As obst(Of T), ByVal [to] As obst(Of T))
        assert(Not from Is Nothing)
        assert(Not [to] Is Nothing)
        If Not from.empty() Then
            clone_to(from.root, [to].root)
        End If
        [to].s = from.s
    End Sub

    Private Sub clone_to(ByVal [from] As node, ByRef [to] As node)
        [to] = copy_node([from])
        If from.has_left_child Then
            Dim n As node = Nothing
            clone_to(from.left_child(), n)
            [to].replace_left(n)
        End If
        If from.has_right_child() Then
            Dim n As node = Nothing
            clone_to(from.right_child(), n)
            [to].replace_right(n)
        End If
    End Sub

    Public Shadows Function size() As UInt32
        Return s
    End Function

    Public Shadows Function count() As UInt32
        Return size()
    End Function

    Public Shadows Sub clear()
        MyBase.clear()
        s = 0
    End Sub

    Public Function emplace_hint(ByVal it As iterator, ByVal v As T) As pair(Of iterator, Boolean)
        If empty() Then
            root = create_node(v)
            s = 1
            Return pair.emplace_of(New iterator(root), True)
        End If
        Dim n As node = Nothing
        n = If(it.null_or_end(), root, it.node())
        While True
            assert(Not n Is Nothing)
            Dim c As Int32 = 0
            c = n.compare(v)
            If c = 0 Then
                Return pair.emplace_of(New iterator(n), False)
            End If
            If c < 0 Then
                If n.has_right_child() Then
                    n = n.right_child()
                Else
                    s += uint32_1
                    Dim r As node = Nothing
                    r = create_node(v)
                    n.replace_right(r)
                    Return pair.emplace_of(New iterator(r), True)
                End If
            ElseIf c > 0 Then
                If n.has_left_child() Then
                    n = n.left_child()
                Else
                    s += uint32_1
                    Dim r As node = Nothing
                    r = create_node(v)
                    n.replace_left(r)
                    Return pair.emplace_of(New iterator(r), True)
                End If
            End If
        End While
        assert(False)
        Return Nothing
    End Function

    Public Function emplace(ByVal v As T) As pair(Of iterator, Boolean)
        Return emplace_hint(Nothing, v)
    End Function

    Public Function insert_hint(ByVal it As iterator, ByVal v As T) As pair(Of iterator, Boolean)
        Return emplace_hint(it, copy_no_error(v))
    End Function

    Public Function insert(ByVal v As T) As pair(Of iterator, Boolean)
        Return insert_hint(Nothing, v)
    End Function

    Protected Function [erase](ByVal n As node,
                               ByVal select_left As Boolean,
                               ByRef parent_of_removed_node As node) As node
        assert(Not n Is Nothing)
        Dim r As node = Nothing
        r = n.erase(select_left, parent_of_removed_node)
        If n.is_root() Then
            root = r
            If Not root Is Nothing Then
                root.clear_parent()
            End If
        End If
        s -= uint32_1
        Return r
    End Function

    Protected Function [erase](ByVal n As node) As node
        Return [erase](n, rnd_bool(), Nothing)
    End Function

    Public Function [erase](ByVal v As T) As Boolean
        Return [erase](find(v))
    End Function

    Public Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() Then
            Return False
        Else
            [erase](it.node())
            Return True
        End If
    End Function

    Protected Shared Shadows Function compare(Of OBSTT As obst(Of T)) _
                                             (ByVal this As OBSTT,
                                              ByVal that As OBSTT,
                                              ByVal cmp As Func(Of T, T, Int32)) As Int32
        Return bt(Of T).compare(this,
                                that,
                                Function(x As obst(Of T)) As UInt32
                                    assert(Not x Is Nothing)
                                    Return x.size()
                                End Function,
                                cmp)
    End Function

    Public Shadows Function CompareTo(ByVal other As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of obst(Of T))(other, False))
    End Function

    Public Shadows Function CompareTo(ByVal other As obst(Of T)) As Int32 _
                                     Implements IComparable(Of obst(Of T)).CompareTo
        Return compare(Me, other, AddressOf connector.compare)
    End Function
End Class
