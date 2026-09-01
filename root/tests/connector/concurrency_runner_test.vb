
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public Class concurrency_runner_test
    Inherits [case]

    Private Shared Function concurrency_case() As Boolean
        If Environment.ProcessorCount() > 2 Then
            Const size As Int32 = 10240
            Dim r As New atomic_int()
            Dim max As New atomic_int()
            Dim a() As Action = Nothing
            ReDim a(size - 1)
            For i As Int32 = 0 To size - 1
                a(i) = Sub()
                           Dim v As Int32 = r.increment()
                           Dim cur As Int32 = max.get()
                           While v > cur
                               Dim prev As Int32 = max.compare_exchange(v, cur)
                               If prev = cur Then
                                   Exit While
                               End If
                               cur = prev
                           End While
                           fake_processor_work(10)
                           assert(r.decrement() >= 0)
                       End Sub
            Next
            concurrency_runner.execute(a)
            ' One thread may be stuck at the for loop in concurrency_runner.
            assertion.more_or_equal(+max, Environment.ProcessorCount() - 1)
        End If
        Return True
    End Function

    Private Shared Function all_executed_case() As Boolean
        Const size As Int32 = 1024
        Dim b As New vector(Of Boolean)(size)
        b.resize(size)
        Dim a() As Action = Nothing
        ReDim a(size - 1)
        For i As Int32 = 0 To size - 1
            Dim j As Int32 = i
            a(i) = Sub()
                       assertion.is_false(b(j))
                       b(j) = True
                   End Sub
        Next
        concurrency_runner.execute(a)
        For i As Int32 = 0 To size - 1
            assertion.is_true(b(i))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return all_executed_case() AndAlso
               concurrency_case()
    End Function
End Class
