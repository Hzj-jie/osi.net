
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.transmitter
Imports lock_t = osi.root.lock.slimlock.monitorlock

Partial Public Class sharedtransmitter_test
    Private Class component
        Private Shared ReadOnly instances()() As component
        Private Shared instances_lock As lock_t
        Public ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of pair(Of Int32, const_pair(Of Byte, Byte)))
        Public ReadOnly address As Byte
        Public ReadOnly local_port As Byte
        Private ReadOnly pump As slimqless2_event_sync_T_pump(Of pair(Of Int32, const_pair(Of Byte, Byte)))

        Shared Sub New()
            ReDim instances(max_uint8 - min_uint8)
            For i As Int32 = min_uint8 To max_uint8
                ReDim instances(i - min_uint8)(max_uint8 - min_uint8)
            Next
        End Sub

        Public Shared Function is_valid_port(ByVal id As Byte) As Boolean
            Return id <> min_uint8 AndAlso id <> max_uint8
        End Function

        Public Sub New(ByVal address As Byte, ByVal local_port As Byte)
            assert_true(is_valid_port(local_port))
            Me.address = address
            Me.local_port = local_port
            instances_lock.wait()
            assert_nothing(instances(Me.address)(Me.local_port))
            instances(Me.address)(Me.local_port) = Me
            instances_lock.release()
            _new(Me.pump)
            Me.receiver = event_sync_T_pump_T_receiver_adapter.[New](pump)
        End Sub

        Public Sub send(ByVal data As Int32, ByVal address As Byte, ByVal port As Byte)
            If assert_not_nothing(instances(address)(port)) Then
                instances(address)(port).receive(data, Me.address, Me.local_port)
            End If
        End Sub

        Public Sub receive(ByVal data As Int32, ByVal address As Byte, ByVal port As Byte)
            pump.emplace(emplace_make_pair(data, emplace_make_const_pair(address, port)))
        End Sub

        Public Sub dispose()
            ' sharedtransmitter (ref_instance) should guarantee to call this function exactly once per instance.
            instances_lock.wait()
            assert_reference_equal(instances(Me.address)(Me.local_port), Me)
            instances(Me.address)(Me.local_port) = Nothing
            instances_lock.release()
        End Sub
    End Class
End Class
