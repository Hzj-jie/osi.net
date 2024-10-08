
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils
Imports counter = osi.root.utils.counter

Partial Friend NotInheritable Class host
    Private Shared expected_end_ms As Int64
    Private Shared using_threads As Int32 = 0
    Private Shared running_cases As Int32 = 0

    Private Shared Sub assert_running_time()
        assert(commandline.has_specified_selections() OrElse
               assertion.less_or_equal(nowadays.milliseconds(), expected_end_ms))
    End Sub

    Public Shared Function execute_case(ByVal c As [case]) As Boolean
        assert(Not c Is Nothing)
        Return (assertion.is_true(do_(AddressOf c.prepare, False), "failed to prepare case ", c.full_name) AndAlso
                assertion.is_true(do_(AddressOf c.run, False), "failed to run case ", c.full_name)) And
               assertion.is_true(do_(AddressOf c.finish, False), "failed to finish case ", c.full_name)
    End Function

    Private Shared Function execute_case(ByVal c As case_info) As Boolean
        assert(Not c Is Nothing)
        assert(env_vars.repeat_per_case >= 1)
        For i As UInt32 = 0 To env_vars.repeat_per_case - uint32_1
            If Not execute_case(c.case) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Shared Sub run(ByVal c As case_info, ByVal finished As EventWaitHandle)
        assert(Not c Is Nothing)
        assert(Not finished Is Nothing)

        If Not selfhealth.in_stage() Then
            Dim msg() As Object = Nothing
            msg = {"start running ", c.full_name(), " at ", short_time()}
            If env_vars.utt_report_case_name Then
                utt_raise_error(msg)
            Else
                raise_error(msg)
            End If
        End If

        Dim startms As Int64 = nowadays.milliseconds()
        Dim proc_ms As Int64 = envs.total_processor_time_ms()
        execute_case(c)
        Interlocked.Add(using_threads, -c.case.reserved_processors())

        If Not selfhealth.in_stage() Then
            If env_vars.utt_report_background_worker_status Then
                raise_error("background worker status after case ",
                            c.full_name(),
                            ": event_comb count is ",
                            counter.instance_count_counter(Of event_comb).count(),
                            ", queue_runner queue length is ",
                            queue_runner.size())
            End If
            If env_vars.utt_report_memory_status Then
                ' utt_concurrency == 0 means no two cases will run together, so it's safe to force GC to collect.
                If utt_concurrency() = 0 Then
                    garbage_collector.repeat_collect()
                End If
                raise_error("memory status after case ",
                            c.full_name(),
                            ": private bytes ",
                            envs.private_bytes_usage(),
                            ", workset bytes ",
                            envs.workingset_bytes_usage(),
                            ", virtual bytes ",
                            envs.virtual_bytes_usage(),
                            ", peak workset bytes ",
                            envs.peak_workingset_bytes_usage(),
                            ", peak virtual bytes ",
                            envs.peak_virtual_memory_bytes_usage(),
                            ", peak paged bytes ",
                            envs.peak_paged_memory_bytes_usage(),
                            ", gc total memory ",
                            envs.gc_total_memory())
            End If
            Dim ms As Int64 = nowadays.milliseconds() - startms
            Dim pms As Int64 = envs.total_processor_time_ms() - proc_ms
            Dim msg() As Object = {"finish running ",
                                   c.full_name(),
                                   ", total time in milliseconds ",
                                   ms,
                                   ", processor usage milliseconds ",
                                   pms,
                                   ", processor usage percentage ",
                                   pms * 100 / ms}
            If env_vars.utt_report_case_name Then
                utt_raise_error(msg)
            Else
                raise_error(msg)
            End If
        End If

        If envs.mono Then
            garbage_collector.waitfor_collect()
        End If

        Interlocked.Decrement(running_cases)
        assert(finished.Set())
    End Sub

    Private Shared Function utt_concurrency() As Int32
        Return If(envs.utt.concurrency <> npos,
                  envs.utt.concurrency,
                  Environment.ProcessorCount())
    End Function

    Private Shared Function go_through(ByVal finished As EventWaitHandle, ByRef new_case_started As Boolean) As Boolean
        assert(Not new_case_started)
        assert_running_time()
        Dim rtn As Boolean = False
        For i As UInt32 = uint32_0 To cases.size() - uint32_1
            If cases(i).finished Then
                Continue For
            End If
            Dim c As case_info = cases(i)
            rtn = True
            If (c.case.reserved_processors() >= 0 AndAlso
                c.case.reserved_processors() + using_threads <= utt_concurrency()) OrElse
               using_threads = 0 Then
                new_case_started = True
                Interlocked.Increment(running_cases)
                Interlocked.Add(using_threads, c.case.reserved_processors())
                managed_thread_pool.push(Sub()
                                             run(c, finished)
                                         End Sub)
                c.finished = True
            End If
        Next

        Return rtn
    End Function

    Private Shared Function go_through_all(ByVal finished As EventWaitHandle) As Boolean
        Dim new_case_started As Boolean = False
        While go_through(finished, new_case_started)
            If new_case_started Then
                new_case_started = False
            Else
                Return True
            End If
        End While
        Return False
    End Function

    Private Shared Sub wait_finish(ByVal finished As EventWaitHandle)
        assert(Not finished Is Nothing)
        While Not finished.WaitOne(5000)
            assert_running_time()
        End While
    End Sub

    Private Shared Function expected_running_time_ms() As Int64
        Dim base_ms As Double = 0
        base_ms = minutes_to_milliseconds(36 * 60)
        base_ms *= envs.perf_run_ms
        base_ms *= 4
        base_ms *= env_vars.repeat_per_case
        base_ms /= 20
        base_ms /= max(utt_concurrency(), 1)
        If envs.virtual_machine Then
            base_ms *= 4
        End If
        Return CLng(base_ms)
    End Function

    Public Shared Sub run()
        expected_end_ms = nowadays.milliseconds() + expected_running_time_ms()
        Using finished As New AutoResetEvent(False)
            While go_through_all(finished)
                wait_finish(finished)
            End While
            While running_cases > 0
                wait_finish(finished)
            End While
        End Using
    End Sub
End Class
