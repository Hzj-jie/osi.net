
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.utils
Imports this_process = osi.root.envs.this_process

Public Module _app
    Public Sub main(ByVal args() As String)
        debugpause()
        ' - or / style switcher conflicts with negative string patterns.
        assert(envs.set_env("argument-no-short-switcher", "true"))
        AppDomain.CurrentDomain().load_all({envs.application_directory, Environment.CurrentDirectory()})
        global_init.execute()
        Dim start_ms As Int64 = Now().milliseconds()
        If envs.utt_no_assert Then
            error_writer_ignore_types(Of file_error_writer).ignore(constants.utt.errortype_char)
        End If
        If envs.utt_no_debug_mode Then
            set_not_debug_mode()
        End If

        ' Ensure the cases are loaded before selfhealth.run.
        assert(Not host.cases.null_or_empty())
        Dim run_case_count As Int32 = 0
        If Not selfhealth.run() Then
            failed("selfhealth failure")
        Else
            raise_error("selfhealth pass")
            run_case_count = host.run(commandline.args())
        End If

        'make sure the Finalize called before output overall failure_count
        host.cases.clear()
        Using garbage_collector.force_aggressive_collecting
            garbage_collector.repeat_collect()
        End Using
        debugpause()

        If Not assertion.equal(counter.instance_count_counter(Of event_comb).count(), 0) Then
            If envs.event_comb_alloc_trace Then
                raise_error(event_comb.dump_alloc_trace())
            End If
        End If
        assertion.equal(counter.instance_count_counter(Of promise).count(), 0)
        'counter.backend_writer
        assertion.less_or_equal(queue_runner.size(), constants.uint32_1)
        assertion.is_true(suppress.init_state())
        ' A .Net framework uses ~ 15 threads, and since ManagedThreadPool was involved in concurrent_runner, it may have
        ' some 4 threads. Unmanaged threads are not controllable, so add an extra 5.
        expectation.less_or_equal(this_process.ref.Threads().Count(),
                                  15 +
                                  Environment.ProcessorCount() +
                                  thread_pool.thread_count() +
                                  queue_runner.thread_count +
                                  5)
        assertion.less_or_equal(envs.gc_total_memory(), 128 * 1024 * 1024)
        If assertion.failure_count() > 0 OrElse expectation.failure_count() > 0 Then
            If assertion.failure_count() > 0 Then
                failed("failure count = ", assertion.failure_count())
            End If
            If expectation.failure_count() > 0 Then
                failed("unsatisfied expectation count = ", expectation.failure_count())
            End If
        Else
            raise_error("finish running all the selected cases, all succeeded")
        End If
        raise_error("finish running all the selected ",
                    run_case_count,
                    " cases, total used time in milliseconds ",
                    Now().milliseconds() - start_ms,
                    ", total processor time in milliseconds ",
                    envs.total_processor_time_ms(),
                    ", average processor usage percentage ",
                    envs.processor_usage())

        this_process.exit(constants.exit_code.succeeded)
    End Sub
End Module
