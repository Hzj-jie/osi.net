
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.selector

Partial Public NotInheritable Class dispenser_test
    Private Class accepter
        Inherits dispenser(Of Int32, Int32).accepter

        Public ReadOnly q As New qless(Of Int32)()
        Private ReadOnly remote As Int32

        Public Sub New(ByVal remote As Int32)
            Me.remote = remote
            AddHandler MyBase.received, AddressOf received
        End Sub

        Public NotOverridable Overrides Function accept(ByVal remote As Int32) As Boolean
            Return Me.remote = remote
        End Function

        Private Shadows Sub received(ByVal data As Int32, ByVal remote As Int32)
            assertion.equal(Me.remote, remote)
            Me.q.emplace(data)
        End Sub
    End Class
End Class
