
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class unchecked_int_test
    Inherits [case]

    Private int8_inc(,) As Int32
    Private int8_dec(,) As Int32
    Private uint8_inc(,) As Int32
    Private uint8_dec(,) As Int32

    Private Shared Function as_uint8(ByVal i As Int32) As Int32
        Return (i And max_uint8)
    End Function

    Private Shared Function as_int8(ByVal i As Int32) As Int32
        Return uint8_int8(as_uint8(i))
    End Function

    Private Shared Sub init(ByVal inc(,) As Int32,
                            ByVal dec(,) As Int32,
                            ByVal s As Int32,
                            ByVal e As Int32,
                            ByVal normalize As Func(Of Int32, Int32))
        For i As Int32 = s To e
            For j As Int32 = s To e
                inc(i - s, j - s) = normalize(i + j)
                dec(i - s, j - s) = normalize(i - j)
            Next
        Next
    End Sub

    Private Function i8_inc(ByVal i As Int32, ByVal j As Int32) As Int32
        Return int8_inc(i - min_int8, j - min_int8)
    End Function

    Private Function i8_dec(ByVal i As Int32, ByVal j As Int32) As Int32
        Return int8_dec(i - min_int8, j - min_int8)
    End Function

    Private Function ui8_inc(ByVal i As Int32, ByVal j As Int32) As Int32
        Return uint8_inc(i - min_uint8, j - min_uint8)
    End Function

    Private Function ui8_dec(ByVal i As Int32, ByVal j As Int32) As Int32
        Return uint8_dec(i - min_uint8, j - min_uint8)
    End Function

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            ReDim int8_inc(CInt(max_int8) - min_int8, CInt(max_int8) - min_int8)
            ReDim int8_dec(CInt(max_int8) - min_int8, CInt(max_int8) - min_int8)
            ReDim uint8_inc(CInt(max_uint8) - min_uint8, CInt(max_uint8) - min_uint8)
            ReDim uint8_dec(CInt(max_uint8) - min_uint8, CInt(max_uint8) - min_uint8)
            init(int8_inc, int8_dec, min_int8, max_int8, AddressOf as_int8)
            init(uint8_inc, uint8_dec, min_uint8, max_uint8, AddressOf as_uint8)
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = min_int8 To max_int8
            For j As Int32 = min_int8 To max_int8
                Dim this As unchecked_int8 = 0
                Dim that As unchecked_int8 = 0
                this = i
                that = j
                assertion.equal(+(this + that), i8_inc(i, j), i, " + ", j)
                assertion.equal(+(this + CSByte(j)), i8_inc(i, j), i, " + ", j)
                assertion.equal(+(CSByte(i) + that), i8_inc(i, j), i, " + ", j)
                assertion.equal(+(this - that), i8_dec(i, j), i, " - ", j)
                assertion.equal(+(this - CSByte(j)), i8_dec(i, j), i, " - ", j)
                assertion.equal(+(CSByte(i) - that), i8_dec(i, j), i, " - ", j)
            Next
        Next

        For i As Int32 = min_uint8 To max_uint8
            For j As Int32 = min_uint8 To max_uint8
                Dim this As unchecked_uint8 = 0
                Dim that As unchecked_uint8 = 0
                this = i
                that = j
                assertion.equal(+(this + that), ui8_inc(i, j), i, " + ", j)
                assertion.equal(+(this + CByte(j)), ui8_inc(i, j), i, " + ", j)
                assertion.equal(+(CByte(i) + that), ui8_inc(i, j), i, " + ", j)
                assertion.equal(+(this - that), ui8_dec(i, j), i, " - ", j)
                assertion.equal(+(this - CByte(j)), ui8_dec(i, j), i, " - ", j)
                assertion.equal(+(CByte(i) - that), ui8_dec(i, j), i, " - ", j)
            Next
        Next

        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Erase int8_inc
        Erase int8_dec
        Erase uint8_inc
        Erase uint8_dec
        Return MyBase.finish()
    End Function
End Class
