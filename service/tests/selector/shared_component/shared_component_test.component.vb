
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.transmitter
Imports lock_t = osi.root.lock.slimlock.monitorlock

Partial Public Class shared_component_test
    Private Class component
        Private Shared ReadOnly locked As bit_array
        Private Shared locked_lock As lock_t
        Public ReadOnly receiver As event_sync_T_pump_T_receiver_adapter(Of pair(Of Int32,
                                                                                    const_pair(Of UInt16, UInt16)))
        Private ReadOnly id As UInt16
        Private ReadOnly pump As slimqless2_event_sync_T_pump(Of pair(Of Int32, const_pair(Of UInt16, UInt16)))

        Shared Sub New()
            locked = New bit_array(max_uint16 - min_uint16)
        End Sub

        Public Shared Function is_valid_port(ByVal id As UInt16) As Boolean
            Return id <> min_uint16 AndAlso id <> max_uint16
        End Function

        Public Sub New(ByVal id As UInt16)
            assert_true(is_valid_port(id))
            locked_lock.wait()
            assert_false(locked(id))
            locked(id) = True
            locked_lock.release()
            Me.id = id
            _new(Me.pump)
            Me.receiver = event_sync_T_pump_T_receiver_adapter.[New](pump)
        End Sub

        Public Sub push(ByVal data As Int32, ByVal address As UInt16, ByVal port As UInt16)
            pump.emplace(emplace_make_pair(data, emplace_make_const_pair(address, port)))
        End Sub

        Public Sub dispose()
            ' shared_component (ref_instance) should guarantee to call this function exactly once per instance.
            locked_lock.wait()
            assert_true(locked(id))
            locked(id) = False
            locked_lock.release()
        End Sub
    End Class
End Class
