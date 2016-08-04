
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.device

Partial Public Class dispenser_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Const accepter_count As Int32 = 10
        Const data_size As Int32 = 1000
        Dim q As qless2(Of pair(Of Int32, Int32)) = Nothing
        q = _new(q)
        Dim d As dispenser(Of Int32, Int32) = Nothing
        d = New dispenser(Of Int32, Int32)(New receiver(q))
        Dim accepters() As accepter = Nothing
        ReDim accepters(accepter_count - 1)
        For i As Int32 = 0 To accepter_count - 1
            accepters(i) = New accepter(i)
            assert_true(d.attach(accepters(i)))
        Next

        For i As Int32 = 0 To data_size - 1
            For j As Int32 = 0 To accepter_count
                q.emplace(emplace_make_pair(i, j))
            Next
        Next

        For i As Int32 = 0 To accepter_count - 1
            Dim j As Int32 = 0
            j = i
            assert_true(timeslice_sleep_wait_until(Function() As Boolean
                                                       Return accepters(j).q.size() = data_size
                                                   End Function,
                                                   seconds_to_milliseconds(10)))
        Next

        For i As Int32 = 0 To accepter_count - 1
            For j As Int32 = 0 To data_size - 1
                Dim k As Int32 = 0
                assert_true(accepters(i).q.pop(k))
                assert_equal(k, j)
            Next
        Next

        For i As Int32 = 0 To accepter_count - 1
            assert_true(d.detach(accepters(i)))
        Next

        assert_true(d.expired())
        Return True
    End Function
End Class
