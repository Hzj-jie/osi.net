
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class big_uint
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
        If this Is Nothing Then
            Return False
        Else
            Return this.true()
        End If
    End Operator

    Public Function fit_uint64() As Boolean
        Return v.size() <= 2
    End Function

    Public Function as_uint64(ByRef overflow As Boolean) As UInt64
        If v.size() = 0 Then
            overflow = False
            Return 0
        ElseIf v.size() = 1 Then
            overflow = False
            Return v(0)
        Else
            overflow = (v.size() > 2)
            Return v(0) + v(1) * (CULng(max_uint32) + uint32_1)
        End If
    End Function

    Public Function fit_uint32() As Boolean
        Return v.size() <= 1
    End Function

    Public Function as_uint32(ByRef overflow As Boolean) As UInt32
        If v.size() = 0 Then
            overflow = False
            Return 0
        Else
            overflow = (v.size() > 1)
            Return v(0)
        End If
    End Function

    Public Function fit_int32() As Boolean
        Return v.size() = 0 OrElse
               (v.size() = 1 AndAlso v(0) <= max_int32)
    End Function

    Public Function as_int32(ByRef overflow As Boolean) As Int32
        If v.size() = 0 Then
            overflow = False
            Return 0
        Else
            If v.size() > 1 Then
                overflow = True
            Else
                overflow = (v(0) > max_int32)
            End If
            Return (v(0) And max_int32)
        End If
    End Function

    Public Function as_bytes() As Byte()
        Dim r() As Byte = Nothing
        If v.empty() Then
            ReDim r(-1)
        Else
            ReDim r(v.size() * byte_count_in_uint32 - uint32_1)
            Dim start As Int64 = 0
            Dim [end] As Int64 = 0
            Dim [step] As Int32 = 0
            If BitConverter.IsLittleEndian Then
                start = 0
                [end] = v.size() - uint32_1
                [step] = 1
            Else
                start = v.size() - uint32_1
                [end] = 0
                [step] = -1
            End If
            For i As Int64 = start To [end] Step [step]
                assert(uint32_bytes(v(i), r, i * byte_count_in_uint32))
            Next
        End If

        Return r
    End Function
End Class
