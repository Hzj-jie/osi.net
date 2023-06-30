
Imports System.Net.Sockets

Public Class socket_peek_1_behavior_test
    Inherits socket_receive_behavior_test(Of socket_peek_1_behavior_test)

    Public Sub New()
        MyBase.New(1, SocketFlags.Peek, False)
    End Sub
End Class
