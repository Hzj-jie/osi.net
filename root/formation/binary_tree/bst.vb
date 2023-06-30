
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Class bst(Of T)
    Inherits bt(Of T)

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal v As bst(Of T)) As bst(Of T)
        If v Is Nothing Then
            Return Nothing
        End If
        Dim r As bst(Of T) = Nothing
        r = New bst(Of T)()
        move_to(v, r)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function find(ByVal v As T) As iterator
        If empty() Then
            Return [end]()
        End If
        Dim n As node = Nothing
        n = root
        While True
            Dim c As Int32 = 0
            c = n.compare(v)
            If c = 0 Then
                Return New iterator(n)
            End If
            If c < 0 Then
                If n.has_right_child() Then
                    n = n.right_child()
                Else
                    Return [end]()
                End If
            Else
#If Not Performance Then
                assert(c > 0)
#End If
                If n.has_left_child() Then
                    n = n.left_child()
                Else
                    Return [end]()
                End If
            End If
        End While
        assert(False)
        Return Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function lower_bound(ByVal v As T) As iterator
        If empty() Then
            Return [end]()
        End If
        Dim n As node = Nothing
        n = root
        Dim l As node = Nothing
        While True
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            Dim c As Int32 = 0
            c = n.compare(v)
            If c = 0 Then
                Return New iterator(n)
            End If
            If c < 0 Then
#If Not Performance Then
                assert(l Is Nothing OrElse n.compare(l) > 0)
#End If
                l = n
                If n.has_right_child() Then
                    n = n.right_child()
                Else
                    Return New iterator(n)
                End If
            Else
#If Not Performance Then
                assert(c > 0)
#End If
                If n.has_left_child() Then
                    n = n.left_child()
                ElseIf l Is Nothing Then
                    Return [end]()
                Else
                    Return New iterator(l)
                End If
            End If
        End While
        assert(False)
        Return Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function upper_bound(ByVal v As T) As iterator
        If empty() Then
            Return [end]()
        End If
        Dim n As node = Nothing
        n = root
        Dim l As node = Nothing
        While True
#If Not Performance Then
            assert(Not n Is Nothing)
#End If
            Dim c As Int32 = n.compare(v)
            If c <= 0 Then
                If n.has_right_child() Then
                    n = n.right_child()
                ElseIf l Is Nothing Then
                    Return [end]()
                Else
                    Return New iterator(l)
                End If
            Else
#If Not Performance Then
                assert(c > 0)
                assert(l Is Nothing OrElse n.compare(l) < 0)
#End If
                l = n
                If n.has_left_child() Then
                    n = n.left_child()
                Else
                    Return New iterator(n)
                End If
            End If
        End While
        assert(False)
        Return Nothing
    End Function
End Class
