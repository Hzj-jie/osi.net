
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.transmitter

<type_attribute()>
Public Class mock_dev_T(Of T)
    Inherits mock_dev_T(Of T, _false, _false)

    Shared Sub New()
        type_attribute.of(Of mock_dev_T(Of T))().forward_from(Of mock_dev_T(Of T, _false, _false))()
    End Sub

    Private Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of T),
                    ByVal receive_pump As slimqless2_event_sync_T_pump(Of T))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Shadows Function the_other_end() As mock_dev_T(Of T)
        Return New mock_dev_T(Of T)(receive_pump, send_pump)
    End Function
End Class

<type_attribute()>
Public Class mock_dev_T(Of T, _RANDOM_SEND_FAILURE As _boolean, _RANDOM_RECEIVE_FAILURE As _boolean)
    Implements dev_T(Of T)

    Private Shared ReadOnly random_send_failure As Boolean
    Private Shared ReadOnly random_receive_failure As Boolean

    Shared Sub New()
        type_attribute.of(Of mock_dev_T(Of T, _RANDOM_SEND_FAILURE, _RANDOM_RECEIVE_FAILURE))().set(
                trait.[New]() _
                    .with_transmit_mode(trait.mode_t.duplex))
        random_send_failure = +alloc(Of _RANDOM_SEND_FAILURE)()
        random_receive_failure = +alloc(Of _RANDOM_RECEIVE_FAILURE)()
    End Sub

    Public ReadOnly send_pump As slimqless2_event_sync_T_pump(Of T)
    Public ReadOnly receive_pump As slimqless2_event_sync_T_pump(Of T)
    Private ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of T)

    Protected Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of T),
                      ByVal receive_pump As slimqless2_event_sync_T_pump(Of T))
        assert(Not send_pump Is Nothing)
        assert(Not receive_pump Is Nothing)
        Me.send_pump = send_pump
        Me.receive_pump = receive_pump
        Me.receiver = event_sync_T_pump_T_receiver_adapter.[New](Me.receive_pump)
    End Sub

    Public Sub New()
        Me.New(new_pump(), new_pump())
    End Sub

    Private Shared Function random_failure() As Boolean
        Return rnd_bool_trues(3)
    End Function

    Protected Shared Function new_pump() As slimqless2_event_sync_T_pump(Of T)
        Return New slimqless2_event_sync_T_pump(Of T)()
    End Function

    Public Function the_other_end() As mock_dev_T(Of T, _RANDOM_SEND_FAILURE, _RANDOM_RECEIVE_FAILURE)
        Return New mock_dev_T(Of T, _RANDOM_SEND_FAILURE, _RANDOM_RECEIVE_FAILURE)(receive_pump, send_pump)
    End Function

    Public Sub clear()
        send_pump.clear()
        receive_pump.clear()
    End Sub

    Public Function sense(ByVal pending As ref(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return receiver.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Return New event_comb(Function() As Boolean
                                  If random_send_failure AndAlso random_failure() Then
                                      Return False
                                  Else
                                      send_pump.emplace(i)
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Public Function receive(ByVal o As ref(Of T)) As event_comb Implements dev_T(Of T).receive
        Return receiver.receive(o)
    End Function

    Protected Shared Function equal(ByVal p As slimqless2_event_sync_T_pump(Of T),
                                    ByVal [is] As Func(Of T, Boolean),
                                    ByVal finish As Func(Of Boolean)) As Boolean
        assert(Not p Is Nothing)
        assert(Not [is] Is Nothing)
        assert(Not finish Is Nothing)
        Dim r As Boolean = False
        Dim o As T = Nothing
        r = True
        While Not finish() AndAlso p.receive(o)
            assert(Not o Is Nothing)
            If Not [is](o) Then
                r = False
            End If
        End While
        Return r AndAlso finish()
    End Function

    Protected Function equal(ByVal p As slimqless2_event_sync_T_pump(Of T), ByVal v() As T) As Boolean
        assert(Not p Is Nothing)
        Dim i As Int32 = 0
        Return equal(p,
                     Function(x As T) As Boolean
                         If equal(v(i), x) Then
                             i += 1
                             Return True
                         Else
                             Return False
                         End If
                     End Function,
                     Function() As Boolean
                         Return i = array_size(v)
                     End Function)
    End Function

    Protected Overridable Function equal(ByVal left As T, ByVal right As T) As Boolean
        Return compare(left, right) = 0
    End Function

    Public Function send_pump_equal(ByVal v() As T) As Boolean
        Return equal(send_pump, v)
    End Function

    Public Function receive_pump_equal(ByVal v() As T) As Boolean
        Return equal(receive_pump, v)
    End Function

    Public Function send_pump_empty() As Boolean
        Return send_pump.empty()
    End Function

    Public Function receive_pump_empty() As Boolean
        Return receive_pump.empty()
    End Function
End Class
