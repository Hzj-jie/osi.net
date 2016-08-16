
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

' Delegates all outgoing requests
Public Class speaker
    Private ReadOnly c As UdpClient

    Public Shared Function [New](ByVal p As powerpoint, ByRef o As speaker) As Boolean
        Dim c As UdpClient = Nothing
        If udp_clients.[New](p, c) Then
            o = New speaker(c)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint) As speaker
        Dim o As speaker = Nothing
        assert([New](p, o))
        Return o
    End Function

    ' Test only
    Public Sub New(ByVal c As UdpClient)
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Function send(ByVal remote As IPEndPoint,
                         ByVal b() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32,
                         Optional ByVal sent As pointer(Of UInt32) = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim p As piece = Nothing
                                  If piece.create(b, offset, count, p) Then
                                      b = p.export(count)
                                      ec = c.send(b, count, remote, sent)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
