
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.device

Partial Public Class dispenser_test
    Private Class accepter
        Implements dispenser(Of Int32, Int32).accepter

        Public ReadOnly q As qless(Of Int32)
        Private ReadOnly remote As Int32

        Public Sub New(ByVal remote As Int32)
            Me.remote = remote
            Me.q = _new(Me.q)
        End Sub

        Public Function accept(ByVal remote As Int32) As Boolean Implements dispenser(Of Int32, Int32).accepter.accept
            Return Me.remote = remote
        End Function

        Public Sub received(ByVal data As Int32,
                            ByVal remote As Int32) Implements dispenser(Of Int32, Int32).accepter.received
            assert_equal(Me.remote, remote)
            Me.q.emplace(data)
        End Sub
    End Class
End Class
