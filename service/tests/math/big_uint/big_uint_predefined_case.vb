
Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_predefined_case
    Inherits [case]

    Private Shared Function case1() As Boolean
        Dim r As big_uint = Nothing
        r = New big_uint()
        Dim exp As UInt64 = 0
        For i As Int32 = 1 To 5000001
            exp += i
            Dim x As big_uint = Nothing
            x = New big_uint(CUInt(i))
            r.add(x)
            Dim overflow As Boolean = False
            If Not assert_equal(x.as_int32(overflow), i) Then
                Return False
            End If
            If Not assert_equal(r.as_uint64(overflow), exp, i) Then
                Return False
            End If
            assert_false(overflow)
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1()
    End Function
End Class
