
Imports System.Net.Sockets
Imports osi.root.connector

Public Class client_checker
    Private Shared ReadOnly ms As Int64
    Private ReadOnly c As TcpClient
    Private ReadOnly p As powerpoint
    Private last_poll_alive_ms As Int64

    Shared Sub New()
        ms = constants.interval_ms.connection_check_interval
    End Sub

    Public Sub New(ByVal c As TcpClient, ByVal p As powerpoint)
        assert(Not c Is Nothing)
        assert(Not p Is Nothing)
        Me.c = c
        Me.p = p
        Me.last_poll_alive_ms = nowadays.milliseconds()
    End Sub

    Public Function check() As Boolean
        If Not c.alive() Then
            Return False
        Else
            Dim nms As Int64 = 0
            nms = nowadays.milliseconds()
            If nms - last_poll_alive_ms >= ms Then
                If Not free_poll_alive(c, p) Then
                    Return False
                End If
                last_poll_alive_ms = nms
            End If
            Return True
        End If
    End Function
End Class
