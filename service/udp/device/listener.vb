
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.device

' Delegates all incoming requests to accepters
Partial Public Class listener
    Inherits dispenser(Of Byte(), IPEndPoint)

    Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16, ByRef o As listener) As Boolean
        Dim r As receiver = Nothing
        If receiver.[New](p, local_port, r) Then
            o = New listener(p, r)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16) As listener
        Dim o As listener = Nothing
        assert([New](p, local_port, o))
        Return o
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByRef o As listener) As Boolean
        Return [New](p, socket_invalid_port, o)
    End Function

    Public Shared Function [New](ByVal p As powerpoint) As listener
        Return [New](p, socket_invalid_port)
    End Function

    Private ReadOnly p As powerpoint

    Private Sub New(ByVal p As powerpoint, ByVal r As receiver)
        MyBase.New(r)
        assert(Not p Is Nothing)
        Me.p = p
        If p.accept_new_connection Then
            AddHandler MyBase.unaccepted, Sub(buff() As Byte, remote As IPEndPoint)
                                              p.udp_dev_manual_device_exporter().inject(
                                                  +(New udp_dev(p, const_array.[New]({remote}), buff)))
                                          End Sub
        End If
        assert(bind())
    End Sub

    Public Sub New(ByVal p As powerpoint, ByVal c As UdpClient)
        Me.New(p, New receiver(c))
    End Sub
End Class
