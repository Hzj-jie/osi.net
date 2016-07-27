
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.commander
Imports osi.service.device
Imports osi.tests.service.device

<type_attribute()>
Public Class mock_herald
    Inherits mock_dev_T(Of command)
    Implements herald

    Shared Sub New()
        type_attribute.of(Of mock_herald).set(transmitter.[New]().with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(ByVal send_q As qless2(Of command), ByVal receive_q As qless2(Of command))
        MyBase.New(send_q, receive_q)
    End Sub

    Public Shadows Function the_other_end() As herald
        Return MyBase.the_other_end(Function(x, y) New mock_herald(x, y))
    End Function
End Class
