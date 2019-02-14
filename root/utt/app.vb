
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants.utt
Imports osi.root.envs
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.utils

Public Module _app
    Public Sub main(ByVal args() As String)
        debugpause()
        global_init.execute()
        Dim start_ms As Int64 = 0
        start_ms = Now().milliseconds()
        assertion.is_true(using_default_ithreadpool())
        If envs.utt_no_assert Then
            error_writer_ignore_types(Of file_error_writer).ignore(errortype_char)
        End If
        commandline.initialize(args)

        Dim run_case_count As Int32 = 0
        If Not selfhealth.run() Then
            failed("selfhealth failure")
        Else
            raise_error("selfhealth pass")
            run_case_count = host.run(commandline.args())
        End If

        'make sure the Finalize called before output overall failure_count
        host.cases.clear()
        repeat_gc_collect()
        debugpause()

        If Not assertion.equal(counter.instance_count_counter(Of event_comb).count(), 0) Then
            If event_comb_alloc_trace Then
                raise_error(event_comb.dump_alloc_trace())
            End If
        End If
        assertion.equal(counter.instance_count_counter(Of promise).count(), 0)
        'counter.backend_writer
        assertion.less_or_equal(queue_runner.size(), constants.uint32_1)
        assertion.is_true(suppress.init_state())
        assertion.is_true(using_default_ithreadpool())
        ' A .Net framework uses ~ 15 threads, and since ManagedThreadPool was involved in concurrent_runner, it may have
        ' some 4 threads. Unmanaged threads are not controllable, so add an extra 5.
        assertion.less_or_equal(current_process.Threads().Count(),
                             15 +
                             Environment.ProcessorCount() +
                             thread_pool().thread_count() +
                             queue_runner.thread_count +
                             5)
        assertion.less_or_equal(gc_total_memory(), 64 * 1024 * 1024)
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
                    total_processor_time_ms(),
                    ", average processor usage percentage ",
                    processor_usage())

        [exit]()
    End Sub
End Module
