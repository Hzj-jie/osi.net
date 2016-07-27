
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.connector

Public Class callback_test
    Inherits multithreading_case_wrapper

    Private Shared ReadOnly thread_count As Int32

    Shared Sub New()
        thread_count = 2 * If(isdebugbuild(), 1, 4)
    End Sub

    Public Sub New()
        MyBase.New(New callback_single_test(False), thread_count)
    End Sub
End Class

Public Class callback_single_test
    Inherits repeat_case_wrapper

    Private Const busy_wait_ms As Int32 = 1
    Private Const sleep_wait_ms As Int32 = -1
    Private Const check_pass_ms As Int32 = 8
    Private Shared ReadOnly size As Int64
    Private ReadOnly strict_time_limited As Boolean

    Shared Sub New()
        size = 1024 * If(isdebugbuild(), 1, 16)
    End Sub

    Friend Sub New(ByVal strict_time_limited As Boolean)
        MyBase.New(New callback_case(busy_wait_ms, sleep_wait_ms, check_pass_ms, strict_time_limited), size)
        Me.strict_time_limited = strict_time_limited
    End Sub

    Public Overrides Function preserved_processors() As Int16
        Return If(strict_time_limited, Environment.ProcessorCount(), 1)
    End Function

    Public Sub New()
        Me.New(True)
    End Sub
End Class