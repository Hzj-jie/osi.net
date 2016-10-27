
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.commander
Imports osi.service.transmitter
Imports osi.tests.service.transmitter

<type_Attribute()>
Public Class mock_herald
    Inherits mock_herald(Of _false, _false)

    Shared Sub New()
        type_attribute.of(Of mock_herald)().forward_from(Of mock_herald(Of _false, _false))()
    End Sub

    Private Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of command),
                    ByVal receive_pump As slimqless2_event_sync_T_pump(Of command))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Shadows Function the_other_end() As mock_herald
        Return New mock_herald(receive_pump, send_pump)
    End Function
End Class

<type_attribute()>
Public Class mock_herald(Of RANDOM_SEND_FAILURE As _boolean, RANDOM_RECEIVE_FAILURE As _boolean)
    Inherits mock_dev_T(Of command, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
    Implements herald

    Shared Sub New()
        type_attribute.of(Of mock_herald(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE))().forward_from(
                Of mock_dev_T(Of piece, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE))()
    End Sub

    Protected Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of command),
                      ByVal receive_pump As slimqless2_event_sync_T_pump(Of command))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Sub New()
        Me.New(new_pump(), new_pump())
    End Sub

    Public Shadows Function the_other_end() As mock_herald(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
        Return New mock_herald(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)(receive_pump, send_pump)
    End Function
End Class
