
Imports osi.root.template
Imports osi.root.formation
Imports osi.service.transmitter

Public Class mock_dev_int
    Inherits mock_dev_int(Of _false, _false)

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of Int32),
                      ByVal receive_pump As slimqless2_event_sync_T_pump(Of Int32))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Shadows Function the_other_end() As mock_dev_int
        Return New mock_dev_int(receive_pump, send_pump)
    End Function
End Class

Public Class mock_dev_int(Of RANDOM_SEND_FAILURE As _boolean, RANDOM_RECEIVE_FAILURE As _boolean)
    Inherits mock_dev_T(Of Int32, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of Int32),
                      ByVal receive_pump As slimqless2_event_sync_T_pump(Of Int32))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Shadows Function the_other_end() As mock_dev_int(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
        Return New mock_dev_int(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)(receive_pump, send_pump)
    End Function

    Protected Overrides Function equal(ByVal x As Int32, ByVal y As Int32) As Boolean
        Return x = y
    End Function
End Class
