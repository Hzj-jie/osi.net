
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device

' Delegates all incoming requests to accepters
Partial Public Class listener
    Inherits dispenser(Of Byte(), IPEndPoint)

    Public Shared Function [New](ByVal p As powerpoint, ByRef o As listener) As Boolean
        Dim r As receiver = Nothing
        If receiver.[New](p, r) Then
            o = New listener(r)
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint) As listener
        Dim o As listener = Nothing
        assert([New](p, o))
        Return o
    End Function

    Private Sub New(ByVal r As receiver)
        MyBase.New(r)
    End Sub

    ' Test only
    Public Sub New(ByVal c As UdpClient)
        Me.New(New receiver(c))
    End Sub
End Class
