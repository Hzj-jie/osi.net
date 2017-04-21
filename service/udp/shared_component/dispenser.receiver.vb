
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.formation

Public NotInheritable Class receiver
    Inherits udp_bytes_dev

    Public Sub New(ByVal d As ref_instance(Of UdpClient))
        MyBase.New(d.assert_getter())
    End Sub
End Class
