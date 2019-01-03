
Imports System.DateTime
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.threadpool
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.delegates

Friend Class callback_case
    Inherits count_case

    Private ReadOnly busy_wait_ms As Int32
    Private ReadOnly sleep_wait_ms As Int32
    Private ReadOnly check_pass_ms As Int32
    Private ReadOnly strict_time_limited As Boolean

    Public Sub New(ByVal busy_wait_ms As Int32,
                   ByVal sleep_wait_ms As Int32,
                   ByVal check_pass_ms As Int32,
                   ByVal strict_time_limited As Boolean)
        MyBase.New()
        Me.busy_wait_ms = busy_wait_ms
        Me.sleep_wait_ms = sleep_wait_ms
        Me.check_pass_ms = check_pass_ms
        Me.strict_time_limited = strict_time_limited
    End Sub

    Private Sub wait()
        Dim start_ms As Int64 = 0
        start_ms = Now().milliseconds()
        strict_wait_when(Function() Now().milliseconds() - start_ms < busy_wait_ms)
        sleep(sleep_wait_ms)
    End Sub

    Private Function create_callback_action() As callback_action
        Dim i As Int32 = 0
        i = rnd_int(0, 2)
        Dim start_ms As Int64 = 0
        Dim rtn As callback_action = Nothing
        rtn = New callback_action(Function() As Boolean
                                      If i = 0 Then
                                          wait()
                                      End If
                                      start_ms = nowadays.milliseconds()
                                      Return True
                                  End Function,
                                  Function() As Boolean
                                      Return callback_action.true_to_pass(
                                                 nowadays.milliseconds() - start_ms >= check_pass_ms)
                                  End Function,
                                  Function() As Boolean
                                      assertion.more_or_equal(nowadays.milliseconds() - start_ms, check_pass_ms)
                                      If strict_time_limited Then
                                          assertion.less(nowadays.milliseconds() - start_ms,
                                                      check_pass_ms + sixteen_timeslice_length_ms)
                                      End If
                                      assertion.more_or_equal(ticks_to_milliseconds(rtn.check_ticks()), start_ms)
                                      If strict_time_limited Then
                                          assertion.less(ticks_to_milliseconds(rtn.check_ticks()),
                                                      start_ms + two_timeslice_length_ms)
                                      End If
                                      assertion.less_or_equal(ticks_to_milliseconds(rtn.begin_ticks()), start_ms)
                                      assertion.more_or_equal(ticks_to_milliseconds(rtn.end_ticks()), start_ms)

                                      assertion.more_or_equal(rtn.check_ticks(), rtn.begin_ticks())
                                      assertion.more_or_equal(rtn.end_ticks(), rtn.check_ticks())

                                      assertion.is_true(rtn.begin_result().true_())
                                      assertion.is_true(rtn.check_result().true_())

                                      If i = 1 Then
                                          wait()
                                      End If
                                      trigger()
                                      Return True
                                  End Function)
        Return rtn
    End Function

    Private Function timeout_ms() As Int64
        If strict_time_limited Then
            Dim normalize As _do(Of Int64, Int64) =
                Function(ByRef i As Int64) As Int64
                    If i < 0 Then
                        Return 0
                    ElseIf i = 0 Then
                        Return timeslice_length_ms
                    Else
                        Return Math.Ceiling(CDbl(i) / timeslice_length_ms) * timeslice_length_ms
                    End If
                End Function
            Return normalize(busy_wait_ms) +
                   normalize(sleep_wait_ms) +
                   normalize(check_pass_ms) +
                   sixteen_timeslice_length_ms
        Else
            Return max_int64
        End If
    End Function

    Public Overrides Function reserved_processors() As Int16
        If strict_time_limited Then
            Return Environment.ProcessorCount()
        Else
            Return 1
        End If
    End Function

    Protected Overrides Function run_case() As Boolean
        Dim this As Int32 = 0
        this = trigger_times()
        assertion.is_true(async_sync(create_callback_action(), timeout_ms()))
        assertion.more(trigger_times(), this)
        Return True
    End Function
End Class
