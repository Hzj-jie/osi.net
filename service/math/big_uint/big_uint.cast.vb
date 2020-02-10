
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
            Return CInt((v(0) And CUInt(max_int32)))
        End If
    End Function

    Public Function as_bytes() As Byte()
        Dim r() As Byte = Nothing
        If v.empty() Then
            ReDim r(-1)
        Else
            ReDim r(CInt(v.size() * byte_count_in_uint32 - uint32_1))
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
                assert(uint32_bytes(v(CUInt(i)), r, CUInt(i) * byte_count_in_uint32))
            Next
        End If

        Return r
    End Function
End Class
