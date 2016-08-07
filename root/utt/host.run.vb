
Imports System.Threading
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.procedure
Imports counter = osi.root.utils.counter

Partial Friend NotInheritable Class host
    Private Shared expected_end_ms As Int64
    Private Shared using_threads As Int32 = 0
    Private Shared running_cases As Int32 = 0

    Private Shared Sub assert_running_time()
        assert(commandline.has_specific_selections() OrElse
               assert_less_or_equal(nowadays.milliseconds(), expected_end_ms))
    End Sub

    Public Shared Function execute_case(ByVal c As [case]) As Boolean
        assert(Not c Is Nothing)
        Return (assert_true(do_(AddressOf c.prepare, False), "failed to prepare case ", c.full_name) AndAlso
                assert_true(do_(AddressOf c.run, False), "failed to run case ", c.full_name)) And
               assert_true(do_(AddressOf c.finish, False), "failed to finish case ", c.full_name)
    End Function

    Public Shared Function execute_case(ByVal c As case_info) As Boolean
        assert(Not c Is Nothing)
        Return execute_case(c.case)
    End Function

    Private Shared Sub run(ByVal c As case_info, ByVal started As EventWaitHandle, ByVal finished As EventWaitHandle)
        assert(Not c Is Nothing)
        assert(Not started Is Nothing)
        assert(Not finished Is Nothing)
        Interlocked.Increment(running_cases)

        If Not self_health_stage() Then
            Dim msg() As Object = Nothing
            msg = {"start running ", c.full_name(), " at ", short_time()}
            If envs.utt_report_case_name Then
                utt_raise_error(msg)
            Else
                raise_error(msg)
            End If
        End If

        Dim startms As Int64 = 0
        startms = nowadays.milliseconds()
        Dim proc_ms As Int64 = 0
        proc_ms = envs.total_processor_time_ms()
        Interlocked.Add(using_threads, c.case.preserved_processors())
        assert(started.Set())
        execute_case(c)
        Interlocked.Add(using_threads, -c.case.preserved_processors())

        If Not self_health_stage() Then
            If envs.utt_report_background_worker_status Then
                raise_error("background worker status after case ",
                            c.full_name(),
                            ": event_comb count is ",
                            counter.instance_count_counter(Of event_comb).count(),
                            ", queue_runner queue length is ",
                            queue_runner.size())
            End If
            If envs.utt_report_memory_status Then
                ' utt_concurrency == 0 means no two cases will run together, so it's safe to force GC to collect.
                If utt_concurrency() = 0 Then
                    repeat_gc_collect()
                End If
                raise_error("memory status after caes ",
                            c.full_name(),
                            ": private bytes ",
                            envs.private_bytes_usage(),
                            ", virtual bytes ",
                            envs.virtual_bytes_usage(),
                            ", workset bytes ",
                            envs.workingset_bytes_usage(),
                            ", gc total memory ",
                            envs.gc_total_memory())
            End If
            Dim ms As Int64 = 0
            ms = nowadays.milliseconds() - startms
            Dim pms As Int64 = 0
            pms = envs.total_processor_time_ms() - proc_ms
            Dim msg() As Object = Nothing
            msg = {"finish running ",
                   c.full_name(),
                   ", total time in milliseconds ",
                   ms,
                   ", processor usage milliseconds ",
                   pms,
                   ", processor usage percentage ",
                   pms * 100 / ms}
            If envs.utt_report_case_name Then
                utt_raise_error(msg)
            Else
                raise_error(msg)
            End If
        End If

        If envs.mono Then
            waitfor_gc_collect()
        End If

        Interlocked.Decrement(running_cases)
        assert(finished.Set())
    End Sub

    Private Shared Function utt_concurrency() As Int32
        Return If(envs.utt_concurrency <> npos,
                  envs.utt_concurrency,
                  Environment.ProcessorCount())
    End Function

    Private Shared Function go_through(ByVal finished As EventWaitHandle, ByRef new_case_started As Boolean) As Boolean
        assert(Not new_case_started)
        assert_running_time()
        Dim rtn As Boolean = False
        For i As Int32 = 0 To cases.size() - 1
            If Not cases(i).finished Then
                rtn = True
                If (cases(i).case.preserved_processors() >= 0 AndAlso
                    cases(i).case.preserved_processors() + using_threads <= utt_concurrency()) OrElse
                   using_threads = 0 Then
                    new_case_started = True
                    cases(i).finished = True
                    Dim j As Int32 = 0
                    j = i
                    Dim started As AutoResetEvent = Nothing
                    started = New AutoResetEvent(False)
                    queue_in_managed_threadpool(Sub() run(cases(j), started, finished))
                    assert(started.WaitOne())
                    started.Close()
                End If
            End If
        Next

        Return rtn
    End Function

    Private Shared Function go_through_all(ByVal finished As EventWaitHandle) As Boolean
        Dim r As Int32 = 0
        Dim new_case_started As Boolean = False
        While go_through(finished, new_case_started)
            r += 1
            If new_case_started Then
                new_case_started = False
            Else
                Exit While
            End If
        End While
        Return r > 0
    End Function

    Private Shared Sub wait_finish(ByVal finished As EventWaitHandle)
        assert(Not finished Is Nothing)
        While Not finished.WaitOne(5000)
            assert_running_time()
        End While
    End Sub

    Public Shared Sub run()
        expected_end_ms = nowadays.milliseconds() + minute_to_milliseconds(36 * 60)
        Dim finished As AutoResetEvent = Nothing
        finished = New AutoResetEvent(False)
        While go_through_all(finished)
            wait_finish(finished)
        End While
        While running_cases > 0
            wait_finish(finished)
        End While
        finished.Close()
    End Sub
End Class
