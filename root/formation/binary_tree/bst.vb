
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class bst(Of T)
    Inherits bt(Of T)

    Public Shared Shadows Function move(ByVal v As bst(Of T)) As bst(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As bst(Of T) = Nothing
            r = New bst(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Function find(ByVal v As T) As iterator
        If empty() Then
            Return [end]()
        Else
            Dim n As node = Nothing
            n = root
            While True
                Dim c As Int32 = 0
                c = n.compare(v)
                If c = 0 Then
                    Return New iterator(n)
                ElseIf c < 0 Then
                    If n.has_right_child() Then
                        n = n.right_child()
                    Else
                        Return [end]()
                    End If
                Else
                    assert(c > 0)
                    If n.has_left_child() Then
                        n = n.left_child()
                    Else
                        Return [end]()
                    End If
                End If
            End While
            assert(False)
            Return Nothing
        End If
    End Function

    Public Function lower_bound(ByVal v As T) As iterator
        If empty() Then
            Return [end]()
        Else
            Dim n As node = Nothing
            n = root
            Dim l As node = Nothing
            While True
                assert(Not n Is Nothing)
                Dim c As Int32 = 0
                c = n.compare(v)
                If c < 0 Then
                    assert(Not isdebugmode() OrElse
                             l Is Nothing OrElse
                             n.compare(l) > 0)
                    l = n
                    If n.has_right_child() Then
                        n = n.right_child()
                    Else
                        Return New iterator(n)
                    End If
                ElseIf c > 0 Then
                    If n.has_left_child() Then
                        n = n.left_child()
                    ElseIf l Is Nothing Then
                        Return [end]()
                    Else
                        Return New iterator(l)
                    End If
                Else
                    Return New iterator(n)
                End If
            End While
            assert(False)
            Return Nothing
        End If
    End Function

    Public Function upper_bound(ByVal v As T) As iterator
        If empty() Then
            Return [end]()
        Else
            Dim n As node = Nothing
            n = root
            Dim l As node = Nothing
            While True
                assert(Not n Is Nothing)
                Dim c As Int32 = 0
                c = n.compare(v)
                If c <= 0 Then
                    If n.has_right_child() Then
                        n = n.right_child()
                    ElseIf l Is Nothing Then
                        Return [end]()
                    Else
                        Return New iterator(l)
                    End If
                Else
                    assert(c > 0)
                    assert(Not isdebugmode() OrElse
                             l Is Nothing OrElse
                             n.compare(l) < 0)
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
        End If
    End Function
End Class
