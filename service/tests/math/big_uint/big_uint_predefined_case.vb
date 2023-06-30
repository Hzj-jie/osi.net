
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Friend NotInheritable Class big_uint_predefined_case
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

    Private Shared Function case3() As Boolean
        Dim l As big_uint = Nothing
        l = New big_uint(10)
        Dim r As big_uint = Nothing
        r = New big_uint(13)
        Dim overflow As Boolean = False
        l.sub(r, overflow)
        assertion.is_true(overflow)
        assertion.equal(l.as_uint32(overflow), CUInt(4294967293))
        assertion.is_false(overflow)
        Return True
    End Function

    Private Shared Function case4() As Boolean
        Dim l As big_uint = Nothing
        l = New big_uint(10)
        l *= (max_uint32 + 1UL)
        Dim r As big_uint = Nothing
        r = New big_uint(13)
        r *= (max_uint32 + 1UL)
        Dim overflow As Boolean = False
        l.sub(r, overflow)
        assertion.is_true(overflow)
        assertion.equal(l.as_uint64(overflow), 4294967293UL * (max_uint32 + 1UL))
        assertion.is_false(overflow)
        Return True
    End Function

    Private Shared Function case5() As Boolean
        Dim x As big_uint = Nothing
        x = big_uint.random()
        assertion.equal(x ^ uint32_1, x)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2() AndAlso
               case3() AndAlso
               case4() AndAlso
               case5()
    End Function
End Class
