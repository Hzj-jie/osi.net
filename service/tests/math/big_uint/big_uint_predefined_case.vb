
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.service.math

Friend Class big_uint_predefined_case
    Inherits [case]

    Private Shared Function case1() As Boolean
        Dim r As big_uint = Nothing
        r = New big_uint()
        Dim exp As UInt64 = 0
        For i As Int32 = 1 To 5000001
            exp += CUInt(i)
            Dim x As big_uint = Nothing
            x = New big_uint(CUInt(i))
            r.add(x)
            Dim overflow As Boolean = False
            If Not assertion.equal(x.as_int32(overflow), i) Then
                Return False
            End If
            If Not assertion.equal(r.as_uint64(overflow), exp, i) Then
                Return False
            End If
            assertion.is_false(overflow)
        Next
        Return True
    End Function

    Private Shared Function case2() As Boolean
        Dim l As big_uint = Nothing
        l = New big_uint(10)
        l.factorial()
        assertion.equal(l, New big_uint(3628800))
        l = New big_uint(11)
        l.factorial()
        assertion.equal(l, New big_uint(39916800))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2()
    End Function
End Class
