
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device

Partial Public Class delegator_datagram_adapter
    Private Class delegator_indicator
        Implements sync_indicator

        Private ReadOnly u As delegator

        Public Sub New(ByVal u As delegator)
            assert(Not u Is Nothing)
            Me.u = u
        End Sub

        Public Function indicate(ByRef pending As Boolean) As Boolean Implements sync_indicator.indicate
            Dim r As Int32 = 0
            r = u.buffered_bytes()
            If r < 0 Then
                Return False
            Else
                pending = (r > 0)
                Return True
            End If
        End Function
    End Class
End Class
