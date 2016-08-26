
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device

' Delegates all incoming requests to accepters
Partial Public Class listener
    Private ReadOnly d As dispenser(Of Byte(), IPEndPoint)

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
        assert(Not r Is Nothing)
        Me.d = New dispenser(Of Byte(), IPEndPoint)(r)
    End Sub

    ' Test only
    Public Sub New(ByVal c As UdpClient)
        Me.New(New receiver(c))
    End Sub

    Public Function attach(ByVal accepter As dispenser(Of Byte(), IPEndPoint).accepter) As Boolean
        Return d.attach(accepter)
    End Function

    Public Function detach(ByVal accepter As dispenser(Of Byte(), IPEndPoint).accepter) As Boolean
        Return d.detach(accepter)
    End Function
End Class
