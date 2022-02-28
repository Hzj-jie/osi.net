
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.event
Imports osi.root.threadpool
Imports osi.root.utils

Partial Public Class event_comb
    Public NotInheritable Class cancellation_controller
        Private ReadOnly e As event_comb
        Private ReadOnly cc As utils.cancellation_controller

        Public Sub New(ByVal e As event_comb)
            assert(Not e Is Nothing)
            Me.e = e
            Me.cc = New utils.cancellation_controller()
        End Sub

        Public Function manual() As flip_events.manual_flip_event
            Return cc.manual(e, AddressOf cancel_event_comb)
        End Function

        Public Sub cancel_manual()
            cc.cancel_manual()
        End Sub

        Public Function ref_counted(ByVal init_value As UInt32) As flip_events.ref_counted_flip_event
            Return cc.ref_counted(init_value, e, AddressOf cancel_event_comb)
        End Function

        Public Function ref_counted() As flip_events.ref_counted_flip_event
            Return cc.ref_counted(e, AddressOf cancel_event_comb)
        End Function

        Public Sub cancel_ref_counted()
            cc.cancel_ref_counted()
        End Sub

        Public Function timeout(ByVal ms As UInt32) As flip_event
            Return cc.timeout(ms, e, AddressOf trigger_timeout)
        End Function

        Public Function timeout(ByVal ms As Int64) As flip_event
            If ms < 0 Then
                cancel_timeout()
                Return Nothing
            End If

            If event_comb_trace AndAlso ms >= max_int32 Then
                raise_error(error_type.performance,
                            "timeout of event_comb @ ",
                            e.callstack(),
                            " has been set to a number over max_int32, ",
                            "which means it almost cannot be timeout forever")
            End If
            If ms >= max_int32 Then
                ms = max_int32
            End If
            Return timeout(CUInt(ms))
        End Function

        Public Sub cancel_timeout()
            cc.cancel_timeout()
        End Sub

        Public Sub cancel()
            cc.cancel()
        End Sub

        Private Shared Sub cancel_event_comb(ByVal e As event_comb)
            assert(Not e Is Nothing)
            e.cancel()
        End Sub

        Private Shared Sub trigger_timeout(ByVal e As event_comb)
            assert(Not e Is Nothing)
            ' flip_events.timeout may immediately execute the callback, which breaks event_comb in debug-build, since
            ' it does not expect the lock to be re-entered.
            thread_pool.push(AddressOf e.timeout)
        End Sub
    End Class
End Class
