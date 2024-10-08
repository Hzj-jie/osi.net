
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Shared Widening Operator CType(ByVal this As UInt32) As big_uint
        Return New big_uint(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As UInt64) As big_uint
        Return New big_uint(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Boolean) As big_uint
        Dim r As big_uint = Nothing
        r = New big_uint()
        r.replace_by(this)
        Return r
    End Operator

    Public Shared Widening Operator CType(ByVal this As big_uint) As Boolean
        Return Not this Is Nothing AndAlso this.true()
    End Operator

    ' This has been implemented by big_uint.not() function. Revise the performance impact: big_uint.not() is a bit-wise
    ' reverse operation.
    'Public Shared Operator Not(ByVal this As big_uint) As Boolean
    '    Return this Is Nothing OrElse this.false()
    'End Operator

    Public Function fit_int64() As Boolean
        Return v.size() <= 2 AndAlso v.get(1) <= max_int32
    End Function

    Public Function as_int64(ByRef overflow As Boolean) As Int64
        If v.size() = 0 Then
            overflow = False
            Return 0
        End If
        If v.size() = 1 Then
            overflow = False
            Return v.get(0)
        End If
        overflow = Not fit_int64()
        Return v.get(0) + (CLng(v.get(1)) << bit_count_in_uint32)
    End Function

    Public Function fit_uint64() As Boolean
        Return v.size() <= 2
    End Function

    Public Function as_uint64(ByRef overflow As Boolean) As UInt64
        If v.size() = 0 Then
            overflow = False
            Return 0
        End If
        If v.size() = 1 Then
            overflow = False
            Return v.get(0)
        End If
        overflow = Not fit_uint64()
        Return v.get(0) + (CULng(v.get(1)) << bit_count_in_uint32)
    End Function

    Public Function fit_uint32() As Boolean
        Return v.size() <= 1
    End Function

    Public Function as_uint32(ByRef overflow As Boolean) As UInt32
        If v.size() = 0 Then
            overflow = False
            Return 0
        End If
        overflow = Not fit_uint32()
        Return v.get(0)
    End Function

    Public Function fit_int32() As Boolean
        Return v.size() = 0 OrElse
               (v.size() = 1 AndAlso v.get(0) <= max_int32)
    End Function

    Public Function as_int32(ByRef overflow As Boolean) As Int32
        If v.size() = 0 Then
            overflow = False
            Return 0
        End If
        If v.size() > 1 Then
            overflow = True
        Else
            overflow = (v.get(0) > max_int32)
        End If
        Return CInt((v.get(0) And CUInt(max_int32)))
    End Function

    Public Function as_bytes() As Byte()
        Dim r() As Byte = Nothing
        If v.empty() Then
            ReDim r(-1)
        Else
            ReDim r(CInt(byte_size() - uint32_1))
            arrays.memcpy(r, v.data(), array_size(r))
            For i As Int32 = 0 To CInt(v.size()) - 1
                assert(uint32_little_endian_bytes(v.get(CUInt(i)), r, CUInt(i) * byte_count_in_uint32))
            Next
        End If

        Return r
    End Function
End Class
