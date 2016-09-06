
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device

' TODO: Use slimqless2_event_sync_T_pump / event_sync_T_pump_T_receiver_adapter
<type_attribute()>
Public Class mock_dev_T(Of T)
    Implements dev_T(Of T)

    Shared Sub New()
        type_attribute.of(Of mock_dev_T(Of T))().set(transmitter.[New](). _
            with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Private Class mock_dev_T_indicator
        Implements indicator

        Private ReadOnly dev_T As mock_dev_T(Of T)

        Public Sub New(ByVal dev_T As mock_dev_T(Of T))
            assert(Not dev_T Is Nothing)
            Me.dev_T = dev_T
        End Sub

        Public Function indicate(ByVal pending As pointer(Of Boolean)) As event_comb Implements indicator.indicate
            Return sync_async(Function() As Boolean
                                  If assert_not_nothing(pending) Then
                                      pending.set(dev_T.pending())
                                      Return True
                                  Else
                                      Return False
                                  End If
                              End Function)
        End Function
    End Class

    Public ReadOnly send_q As qless2(Of T)
    Public ReadOnly receive_q As qless2(Of T)
    Private ReadOnly sensor As sensor

    Protected Sub New(ByVal send_q As qless2(Of T),
                      ByVal receive_q As qless2(Of T))
        assert(Not send_q Is Nothing)
        assert(Not receive_q Is Nothing)
        Me.send_q = send_q
        Me.receive_q = receive_q
        Me.sensor = New indicator_sensor_adapter(New mock_dev_T_indicator(Me))
    End Sub

    Public Sub New()
        Me.New(New qless2(Of T)(), New qless2(Of T)())
    End Sub

    Protected Function the_other_end(Of RT As mock_dev_T(Of T)) _
                                    (ByVal c As Func(Of qless2(Of T), qless2(Of T), RT)) As RT
        assert(Not c Is Nothing)
        Return c(Me.receive_q, Me.send_q)
    End Function

    Public Function the_other_end() As mock_dev_T(Of T)
        Return the_other_end(Function(x, y) New mock_dev_T(Of T)(x, y))
    End Function

    Public Sub clear()
        send_q.clear()
        receive_q.clear()
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return sensor.sense(pending, timeout_ms)
    End Function

    Public Function pending() As Boolean
        Return Not receive_q.empty()
    End Function

    Public Function sync_send(ByVal i As T) As Boolean
        send_q.push(i)
        Return True
    End Function

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Return sync_async(Function() sync_send(i))
    End Function

    Public Function sync_receive(ByRef o As T) As Boolean
        Return receive_q.pop(o)
    End Function

    Public Function sync_receive(ByVal o As pointer(Of T)) As Boolean
        Dim v As T = Nothing
        If Not o Is Nothing AndAlso sync_receive(v) Then
            o.set(v)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function receive(ByVal o As pointer(Of T)) As event_comb Implements dev_T(Of T).receive
        Return sync_async(Function() sync_receive(o))
    End Function

    Protected Overridable Function consistent(ByVal x As T, ByVal y As T) As Boolean
        Return compare(x, y) = 0
    End Function

    Private Function consistent(ByVal q As qless2(Of T), ByVal v() As T) As Boolean
        assert(Not q Is Nothing)
        If array_size(v) <> q.size() Then
            Return False
        Else
            Dim r As Boolean = False
            r = True
            For i As Int32 = 0 To array_size(v) - 1
                Dim c As T = Nothing
                assert(q.pop(c))
                If Not consistent(c, v(i)) Then
                    r = False
                End If
                q.emplace(c)
            Next
            Return r
        End If
    End Function

    Public Function send_q_consistent(ByVal v() As T) As Boolean
        Return consistent(send_q, v)
    End Function

    Public Function receive_q_consistent(ByVal v() As T) As Boolean
        Return consistent(receive_q, v)
    End Function

    Public Function consistent(ByVal v() As T) As Boolean
        Return send_q_consistent(v)
    End Function
End Class
