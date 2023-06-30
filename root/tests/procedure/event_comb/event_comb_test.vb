
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.connector

Public Class event_comb_test
    Inherits multithreading_case_wrapper

    Private Shared ReadOnly thread_count As Int32

    Shared Sub New()
        thread_count = 4 * If(isdebugbuild(), 1, 2)
    End Sub

    Public Sub New()
        MyBase.New(New event_comb_single_test(False), thread_count)
    End Sub
End Class

Public Class event_comb_single_test
    Inherits repeat_case_wrapper

    Private Const busy_wait_ms As Int32 = 1
    Private Const sleep_wait_ms As Int32 = 0
    Private Shared ReadOnly size As Int64

    Shared Sub New()
        size = 1024 * If(isdebugbuild(), 1, 16)
    End Sub

    Friend Sub New(ByVal strict_time_limited As Boolean)
        MyBase.New(New event_comb_case(busy_wait_ms, sleep_wait_ms, strict_time_limited), size)
    End Sub

    Public Sub New()
        Me.New(True)
    End Sub
End Class
