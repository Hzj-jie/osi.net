
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.transmitter

Public Class event_sync_T_pump_T_receiver_adapter_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(New basic_case(),
                   multithreading(repeat(New multithreading_case(),
                                         multithreading_case.round),
                                  multithreading_case.thread_count))
    End Sub

    Private Class basic_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim sp As slimqless2_event_sync_T_pump(Of Int32)
            Dim r As event_sync_T_pump_T_receiver_adapter(Of Int32)
            Dim b As ref(Of Boolean) = Nothing
            Dim p As ref(Of Int32) = Nothing
            Dim v As Int32 = 0

            sp = New slimqless2_event_sync_T_pump(Of Int32)()
            r = event_sync_T_pump_T_receiver_adapter.[New](sp)

            assertion.is_true(async_sync(r.sense(b.renew(), 100)))
            assertion.is_false(+b)

            v = rnd_int()
            sp.emplace(v)
            assertion.is_true(async_sync(r.sense(b.renew(), 1)))
            assertion.is_true(+b)
            assertion.is_true(async_sync(r.receive(p.renew()), 1))
            assertion.equal(+p, v)

            v = rnd_int()
            assert_begin(r.sense(b.renew(), 1000))
            sp.emplace(v)
            assertion.is_true(timeslice_sleep_wait_until(Function() +b, 100))
            assertion.is_true(async_sync(r.receive(p.renew()), 1))
            assertion.equal(+p, v)

            v = rnd_int()
            assert_begin(r.receive(p.renew()), 1000)
            sp.emplace(v)
            assertion.is_true(timeslice_sleep_wait_until(Function() +p = v, 100))

            Return True
        End Function
    End Class

    Private Class multithreading_case
        Inherits [case]

        Public Shared ReadOnly thread_count As Int32 = max(Environment.ProcessorCount() - 2, 2)
        Public Shared ReadOnly round As Int32 = 2 * 1024 * 1024 \ thread_count
        Private Const sense_timeout_ms As Int64 = 16
        Private Shared ReadOnly receive_procedure_count As Int32 = max(Environment.ProcessorCount(), 4)
        Private ReadOnly sp As slimqless2_event_sync_T_pump(Of Int32)
        Private ReadOnly r As event_sync_T_pump_T_receiver_adapter(Of Int32)
        Private ReadOnly exp As expiration_controller.settable
        Private ReadOnly has As bit_array_thread_safe
        Private ReadOnly index As atomic_int
        Private ReadOnly received As atomic_int

        Public Sub New()
            sp = New slimqless2_event_sync_T_pump(Of Int32)()
            r = event_sync_T_pump_T_receiver_adapter.[New](sp)
            exp = expiration_controller.settable.[New]()
            has = New bit_array_thread_safe()
            index = New atomic_int()
            received = New atomic_int()
        End Sub

        Public Overrides Function reserved_processors() As Int16
            Return CShort(Environment.ProcessorCount())
        End Function

        Private Sub start()
            Dim ec As event_comb = Nothing
            Dim b As ref(Of Boolean) = Nothing
            Dim p As ref(Of Int32) = Nothing
            assert_begin(lifetime_event_comb(exp,
                                             Function() As Boolean
                                                 ec = r.sense(b.renew(), sense_timeout_ms)
                                                 Return waitfor(ec) AndAlso
                                                        goto_next()
                                             End Function,
                                             Function() As Boolean
                                                 assertion.is_true(ec.end_result())
                                                 If +b Then
                                                     ec = r.receive(p.renew())
                                                     Return waitfor(ec) AndAlso
                                                            goto_next()
                                                 End If
                                                 Return goto_begin()
                                             End Function,
                                             Function() As Boolean
                                                 assertion.is_true(ec.end_result())
                                                 If +p >= 0 Then
                                                     received.increment()
                                                     assertion.is_false(has(CUInt(+p)))
                                                     has(CUInt(+p)) = True
                                                 End If
                                                 Return goto_begin()
                                             End Function))
        End Sub

        Public Overrides Function prepare() As Boolean
            If Not MyBase.prepare() Then
                Return False
            End If
            sp.clear()
            has.resize(CUInt(thread_count * round))
            index.set(0)
            received.set(0)
            For i As Int32 = 0 To receive_procedure_count - 1
                start()
            Next
            Return True
        End Function

        Public Overrides Function run() As Boolean
            sp.emplace(index.increment() - 1)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.is_true(timeslice_sleep_wait_until(Function() +received = +index, minutes_to_milliseconds(5)))
            assert(exp.stop())
            ' Ensure all the blocking receive procedures can finish.
            For i As Int32 = 0 To receive_procedure_count - 1
                sp.emplace(-1)
            Next
            sleep(sense_timeout_ms * 2)
            For i As UInt32 = 0 To has.size() - uint32_1
                assertion.is_true(has(i), "has(", i, ") is false.")
            Next
            sp.clear()
            has.resize(0)
            Return MyBase.finish()
        End Function
    End Class
End Class
