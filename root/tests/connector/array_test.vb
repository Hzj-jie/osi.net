
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class array_test
    Inherits [case]

    Private Shared Function reverse_case() As Boolean
        For i As Int32 = 0 To 1000
            Dim a1() As Byte = Nothing
            ReDim a1(rnd_int(1, 100))
            For j As Int32 = 0 To array_size(a1) - 1
                a1(j) = rnd_int(min_uint8, max_uint8 + 1)
            Next
            Dim a2() As Byte = Nothing
            a2 = a1.reverse()
            If assertion.equal(array_size(a1), array_size(a2)) Then
                Dim j As Int32 = 0
                Dim k As Int32 = 0
                j = 0
                k = array_size(a1) - 1
                While j < array_size(a1)
                    assertion.equal(a1(j), a2(k))
                    j += 1
                    k -= 1
                End While
            End If
        Next
        Return True
    End Function

    Private Shared Function array_concat_case() As Boolean
        Dim b() As Byte = Nothing
        assertion.is_null(array_concat(b))

        ReDim b(-1)
        assertion.reference_equal(array_concat(b), b)

        ReDim b(100)
        assertion.reference_equal(array_concat(b), b)

        ReDim b(0)
        b(0) = rnd_byte()
        Dim b2() As Byte = Nothing
        Dim r() As Byte = Nothing
        r = array_concat(b, b2)
        assertion.equal(array_size(r), uint32_1)
        assertion.equal(r(0), b(0))

        b = rnd_bytes(5)
        b2 = rnd_bytes(5)
        r = array_concat(b, b2)
        assertion.equal(array_size(r), CUInt(10))
        For i As UInt32 = 0 To array_size(r) - uint32_1
            If i < array_size(b) Then
                assertion.equal(r(i), b(i))
            Else
                assertion.equal(r(i), b2(i - array_size(b)))
            End If
        Next

        Dim b3() As Byte = Nothing
        b = rnd_bytes(5)
        b2 = Nothing
        b3 = rnd_bytes(5)
        r = array_concat(b, b2, b3)
        assertion.equal(array_size(r), CUInt(10))
        For i As UInt32 = 0 To array_size(r) - uint32_1
            If i < array_size(b) Then
                assertion.equal(r(i), b(i))
            Else
                assertion.equal(r(i), b3(i - array_size(b)))
            End If
        Next

        b = Nothing
        b2 = Nothing
        b3 = Nothing
        r = array_concat(b, b2, b3)
        assertion.is_not_null(r)
        assertion.equal(array_size(r), uint32_0)

        Return True
    End Function

    Private Shared Function concat_ext_case() As Boolean
        Dim b() As Byte = Nothing
        assertion.is_null(b.concat())

        b = rnd_bytes(5)
        assertion.reference_equal(b.concat(), b)

        Dim b2() As Byte = Nothing
        Dim r() As Byte = Nothing
        r = b.concat(b2)
        assertion.array_equal(r, array_concat(b, b2))

        r = b2.concat(b)
        assertion.array_equal(r, array_concat(b2, b))

        Dim b3() As Byte = Nothing
        b3 = rnd_bytes(5)

        r = b.concat(b2, b3)
        assertion.array_equal(r, array_concat(b, b2, b3))

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return reverse_case() AndAlso
               array_concat_case() AndAlso
               concat_ext_case()
    End Function
End Class
