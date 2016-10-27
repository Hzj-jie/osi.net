
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class dispenser_test
    Private Class accepter
        Inherits dispenser(Of Int32, Int32).accepter

        Public ReadOnly q As qless(Of Int32)
        Private ReadOnly remote As Int32

        Public Sub New(ByVal remote As Int32)
            Me.remote = remote
            Me.q = _new(Me.q)
            AddHandler MyBase.received, AddressOf received
        End Sub

        Public NotOverridable Overrides Function accept(ByVal remote As Int32) As Boolean
            Return Me.remote = remote
        End Function

        Private Shadows Sub received(ByVal data As Int32, ByVal remote As Int32)
            assert_equal(Me.remote, remote)
            Me.q.emplace(data)
        End Sub
    End Class
End Class
