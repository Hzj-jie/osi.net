
Imports osi.root.constants
Imports osi.root.connector

Partial Public MustInherit Class expiration_controller
    Private NotInheritable Class ms
        Inherits expiration_controller

        Private Shadows ReadOnly ms As Int64
        Private start_ms As Int64

        Public Sub New(ByVal ms As Int64)
            Me.ms = ms
            start_ms = npos
        End Sub

        Public Overrides Function expired() As Boolean
            If ms >= 0 Then
                If start_ms = npos Then
                    atomic.eva(start_ms, nowadays.milliseconds())
                End If
                Return nowadays.milliseconds() - start_ms > ms
            Else
                Return True
            End If
        End Function
    End Class
End Class
