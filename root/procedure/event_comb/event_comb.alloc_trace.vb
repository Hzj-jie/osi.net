
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports monitorlock = osi.root.lock.slimlock.monitorlock

Partial Public Class event_comb
    Private Shared callstack_alloc_lock As monitorlock
    Private Shared ReadOnly callstack_alloc As map(Of String, Int64)

    Private Shared Sub callstack_alloc_change(ByVal n As String, ByVal c As Int64)
        assert(Not callstack_alloc Is Nothing)
        callstack_alloc_lock.locked(Sub()
                                        If callstack_alloc.find(n) = callstack_alloc.end() Then
                                            callstack_alloc(n) = counter.register_counter(
                                                                        strtoupper(strcat("event_comb_",
                                                                                          n,
                                                                                          "_instance_count")))
                                        End If
                                        assert(counter.increase(callstack_alloc(n), c))
                                    End Sub)
    End Sub

    Private Sub trace_start()
        counter.instance_count_counter(Of event_comb).alloc()
        If event_comb_alloc_trace Then
            callstack_alloc_change(callstack(), 1)
        End If
    End Sub

    Private Sub trace_finish()
        counter.instance_count_counter(Of event_comb).dealloc()
        If event_comb_alloc_trace Then
            callstack_alloc_change(callstack(), -1)
        End If
    End Sub
End Class
