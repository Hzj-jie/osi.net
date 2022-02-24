
Imports osi.root.formation
Imports osi.root.event
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.transmitter

Partial Public Class channel
    Private Class channel_block
        Implements block

        Private ReadOnly channel As channel
        Private ReadOnly local_id As UInt32
        Private ReadOnly remote_id As UInt32
        Private ReadOnly signal As weak_signal_event
        Private ReadOnly buff As qless(Of ref(Of Byte()))
        Private lams As Int64

        Public Sub New(ByVal channel As channel, ByVal local_id As UInt32, ByVal remote_id As UInt32)
            assert(Not channel Is Nothing)
            Me.channel = channel
            Me.local_id = local_id
            Me.remote_id = remote_id
            Me.signal = New weak_signal_event()
            Me.buff = New qless(Of ref(Of Byte()))()
        End Sub

        Public Function send(ByVal buff() As Byte,
                             ByVal offset As UInt32,
                             ByVal count As UInt32) As event_comb Implements block_injector.send

        End Function

        Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block_pump.receive

        End Function

        Public Function sense(ByVal pending As ref(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb Implements sensor.sense

        End Function

        Public Function last_active_ms() As Int64
            Return lams
        End Function
    End Class
End Class
