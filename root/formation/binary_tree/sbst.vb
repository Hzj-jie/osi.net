
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Class sbst(Of T)
    Inherits obst(Of T)

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        MyBase.New()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub New(ByVal cmp As Func(Of T, T, Int32))
        MyBase.New(cmp)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal v As sbst(Of T)) As sbst(Of T)
        If v Is Nothing Then
            Return Nothing
        End If
        Dim r As sbst(Of T) = Nothing
        r = New sbst(Of T)()
        move_to(v, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function clone() As sbst(Of T)
        Dim r As sbst(Of T) = Nothing
        r = New sbst(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    'the implementation here is a little bit different to the common description
    'when in zig-zig situation
    'http://en.wikipedia.org/wiki/Splay_tree
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub splay(ByVal n As node)
        assert(Not n Is Nothing)
        While Not n.is_root()
            If n.is_left_subtree() Then
                n = n.parent().right_rotate()
            Else
                assert(n.is_right_subtree())
                n = n.parent().left_rotate()
            End If
        End While
        root = n
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function splay(ByVal r As pair(Of iterator, Boolean)) As pair(Of iterator, Boolean)
        assert(Not r Is Nothing)
        splay(r.first.node())
        Return pair.emplace_of(New iterator(root), r.second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function emplace_hint(ByVal it As iterator, ByVal v As T) As pair(Of iterator, Boolean)
        Return splay(MyBase.emplace_hint(it, v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function emplace(ByVal v As T) As pair(Of iterator, Boolean)
        Return splay(MyBase.emplace(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function insert_hint(ByVal it As iterator, ByVal v As T) As pair(Of iterator, Boolean)
        Return splay(MyBase.insert_hint(it, v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function insert(ByVal v As T) As pair(Of iterator, Boolean)
        Return splay(MyBase.insert(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal v As T) As Boolean
        Return [erase](find(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal it As iterator) As Boolean
        If it = [end]() Then
            Return False
        End If
        splay(it.node())
        MyBase.[erase](root)
        Return True
    End Function
End Class
