
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.threadpool

Friend Class event_driver
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function event_comb_valid(ByVal ec As event_comb) As Boolean
        Return Not ec Is Nothing AndAlso ec.not_pending()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function begin(ByVal ec As event_comb) As Boolean
        If Not event_comb_valid(ec) Then
            Return False
        End If
        If envs.event_driver_begin_in_current_thread AndAlso
           instance_stack(Of event_comb).size() < 128 Then
            ec.do()
        Else
            thread_pool().queue_job(AddressOf ec.do)
        End If
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function begin(ByVal ec As event_comb, ByVal timeout_ms As Int64) As Boolean
        'if the last event_comb is acturally working in the same thread of this event_comb, it will dead lock
        'say thread A & B, event_comb C & D
        'D is waiting for C
        'in thread A, which is the thread of this function call, <coming from D._do>
        'so A has the lock of D,
        'and it needs the lock of C to set the timeout.
        'in thread B, which is the thread C works on <with begin(ec)>
        'B has the lock of C, but also need to resume D <C works in the same thread>,
        'which means it waits for the lock of D

        'so A needs lock of C, and has lock of D,
        'but B needs lock of D, and has lock of C,
        'deadlock happens
        If Not event_comb_valid(ec) Then
            Return False
        End If
        'this should be consistent with set_timeout function
        If timeout_ms >= 0 Then
            ec.cancellation_control.timeout(timeout_ms)
        End If
        Return assert(begin(ec))
    End Function
End Class
