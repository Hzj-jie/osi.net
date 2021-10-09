
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

<global_init(global_init_level.foundamental)>
Public Class control_c_trace
    Private Shared enabled As singleentry

    Public Shared Function enable_control_c_trace() As Boolean
        If enabled.mark_in_use() Then
            AddHandler control_c.press,
                       AddressOf control_c_trace_handle
            Return True
        End If
        Return False
    End Function

    Public Shared Sub disable_control_c_trace()
        enabled.mark_not_in_use()
        RemoveHandler control_c.press,
                      AddressOf control_c_trace_handle
    End Sub

    Public Shared Sub control_c_trace_handle(ByRef canceled As Boolean)
        suspend_all_current_process_threads()
        pause()
        resume_all_current_process_threads()
        canceled = True
    End Sub

    Private Shared Sub init()
        If envs.control_c_trace Then
            assert(enable_control_c_trace())
        End If
    End Sub
End Class
