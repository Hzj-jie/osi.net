
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.transmitter

Public Class mock_dev_int_test
    Inherits [case]

    Private Shared Function one_way_case() As Boolean
        Dim d As mock_dev_int = Nothing
        d = New mock_dev_int()
        Dim v1() As Int32 = Nothing
        Dim v2() As Int32 = Nothing
        v1 = rnd_ints(rnd_int(1024, 4096))
        v2 = rnd_ints(rnd_int(1024, 4096))
        For i As Int32 = 0 To array_size(v1) - 1
            d.send_pump.emplace(v1(i))
        Next
        For i As Int32 = 0 To array_size(v2) - 1
            d.receive_pump.emplace(v2(i))
        Next
        assertion.is_true(d.send_pump_equal(v1))
        assertion.is_true(d.receive_pump_equal(v2))
        Return True
    End Function

    Private Shared Function bi_way_case() As Boolean
        Dim d1 As mock_dev_int = Nothing
        Dim d2 As mock_dev_int = Nothing
        d1 = New mock_dev_int()
        d2 = d1.the_other_end()
        If assertion.is_not_null(d2) Then
            Dim v1() As Int32 = Nothing
            Dim v2() As Int32 = Nothing
            v1 = rnd_ints(rnd_int(1024, 4096))
            v2 = rnd_ints(rnd_int(1024, 4096))
            For i As Int32 = 0 To array_size(v1) - 1
                d1.send_pump.emplace(v1(i))
            Next
            For i As Int32 = 0 To array_size(v2) - 1
                d2.send_pump.emplace(v2(i))
            Next
            assertion.is_true(d1.receive_pump_equal(v2))
            assertion.is_true(d2.receive_pump_equal(v1))
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return one_way_case() AndAlso
               bi_way_case()
    End Function
End Class
