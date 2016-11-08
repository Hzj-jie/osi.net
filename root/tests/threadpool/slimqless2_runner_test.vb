
Imports osi.root.formation
Imports osi.root.threadpool
Imports osi.root.lock
Imports osi.root.utt

Public Class slimqless2_runner_test
    Inherits [case]

    Private Shared Function single_thread_case() As Boolean
        Const size As Int32 = 1024 * 1024
        Dim b As bit_array_thread_safe = Nothing
        b = New bit_array_thread_safe(size)
        Dim c As atomic_int32 = Nothing
        c = New atomic_int32()
        Dim r As slimqless2_runner = Nothing
        r = New slimqless2_runner()
        For i As Int32 = 0 To size - 1
            Dim j As Int32 = 0
            j = i
            r.emplace(Sub()
                          assert_false(b(j))
                          b(j) = True
                          c.increment()
                      End Sub)
        Next
        r.emplace(Sub()
                      r.stop()
                  End Sub)
        r.join()

        assert_equal(+c, size)
        Return True
    End Function

    Private Shared Function execute_case() As Boolean
        Const size As Int32 = 32 * 1024 * 1024
        Dim b As bit_array_thread_safe = Nothing
        b = New bit_array_thread_safe(size)
        Dim c As atomic_int32 = Nothing
        c = New atomic_int32()
        Dim r As slimqless2_runner = Nothing
        r = New slimqless2_runner()
        For i As Int32 = 0 To size - 1
            Dim j As Int32 = 0
            j = i
            r.emplace(Sub()
                          assert_false(b(j))
                          b(j) = True
                          c.increment()
                      End Sub)
        Next
        While r.execute()
        End While

        assert_equal(+c, size)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return single_thread_case() AndAlso
               execute_case()
    End Function
End Class
