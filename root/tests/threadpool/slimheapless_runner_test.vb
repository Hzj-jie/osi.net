
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimheapless_runner_test.vbp ----------
'so change slimheapless_runner_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimqless2_runner_test.vbp ----------
'so change slimqless2_runner_test.vbp instead of this file


Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.threadpool
Imports osi.root.lock
Imports osi.root.utt

Public Class slimheapless_runner_test
    Inherits [case]

    Private Shared Function single_thread_case() As Boolean
        Const size As Int32 = 1024 * 1024
        Dim b As bit_array_thread_safe = Nothing
        b = New bit_array_thread_safe(size)
        Dim c As atomic_int32 = Nothing
        c = New atomic_int32()
        Dim r As slimheapless_runner = Nothing
        r = New slimheapless_runner()
        assert_false(r.running_in_current_thread())
        For i As Int32 = 0 To size - 1
            Dim j As Int32 = 0
            j = i
            r.emplace(Sub()
                          assert_true(r.running_in_current_thread())
                          assert_false(b(j))
                          b(j) = True
                          If c.increment() = size Then
                              r.stop()
                          End If
                      End Sub)
        Next
        assert_true(r.join(seconds_to_milliseconds(size / 32 / 1024 / 1024 * 240)))
        assert_equal(+c, size)
        assert_false(r.stopping())
        assert_true(r.stopped())
        r.stop()
        r.join()
        assert_false(r.stopping())
        assert_true(r.stopped())
        Return True
    End Function

    Private Shared Function execute_case() As Boolean
        Const size As Int32 = 128 * 1024
        Dim b As bit_array_thread_safe = Nothing
        b = New bit_array_thread_safe(size)
        Dim c As atomic_int32 = Nothing
        c = New atomic_int32()
        Dim executed As atomic_int32 = Nothing
        executed = New atomic_int32()
        Dim r As slimheapless_runner = Nothing
        r = New slimheapless_runner()
        assert_false(r.running_in_current_thread())
        For i As Int32 = 0 To size - 1
            Dim j As Int32 = 0
            j = i
            r.emplace(Sub()
                          If Not r.running_in_current_thread() Then
                              executed.increment()
                          End If
                          assert_false(b(j))
                          b(j) = True
                          c.increment()
                          fake_processor_ticks_work(1)
                      End Sub)
        Next
        While r.execute()
        End While
        assert_equal(+c, size)
        assert_false(r.stopping())
        assert_more(+executed, 0)
        r.stop()
        r.join()
        assert_false(r.stopping())
        assert_true(r.stopped())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return single_thread_case() AndAlso
               execute_case()
    End Function
End Class
'finish slimqless2_runner_test.vbp --------
'finish slimheapless_runner_test.vbp --------
