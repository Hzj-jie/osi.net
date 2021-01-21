
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock

Public NotInheritable Class slimheapless(Of T)
    Private Class node
        Public ReadOnly v As T
        Public [next] As node

        Public Sub New(ByVal v As T)
            Me.v = v
        End Sub
    End Class

    Private ReadOnly f As node

    Public Sub New()
        f = New node(Nothing)
    End Sub

    Public Sub clear()
        While pop(Nothing)
        End While
    End Sub

    Public Sub emplace(ByVal d As T)
        Dim i As Int32 = 0
        Dim n As node = Nothing
        n = New node(d)
        While True
            n.next = f.next
            If Interlocked.CompareExchange(f.next, n, n.next) Is n.next Then
                Return
            ElseIf should_yield() Then
                [yield]()
            Else
                i += 1
                If i > loops_per_yield Then
                    If [yield]() Then
                        i = 0
                    Else
                        i = loops_per_yield - 1
                    End If
                End If
            End If
        End While
        assert(False)
    End Sub

    Public Function pop() As T
        Dim o As T = Nothing
        If pop(o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Function pop(ByRef d As T) As Boolean
        Dim i As Int32 = 0
        While True
            Dim n As node = Nothing
            n = f.next
            If n Is Nothing Then
                Return False
            End If
            If Interlocked.CompareExchange(f.next, n.next, n) Is n Then
                d = n.v
                Return True
            ElseIf should_yield() Then
                [yield]()
            Else
                i += 1
                If i > loops_per_yield Then
                    If [yield]() Then
                        i = 0
                    Else
                        i = loops_per_yield - 1
                    End If
                End If
            End If
        End While
        assert(False)
        Return False
    End Function

    Public Function pick() As T
        Dim o As T = Nothing
        If pick(o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Function pick(ByRef d As T) As Boolean
        Dim n As node = Nothing
        n = f.next
        If n Is Nothing Then
            Return False
        Else
            d = n.v
            Return True
        End If
    End Function

    Public Sub push(ByVal d As T)
        emplace(copy_no_error(d))
    End Sub

    Public Function empty() As Boolean
        Return f.next Is Nothing
    End Function
End Class
