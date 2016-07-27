
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.template

Public Module __int64
    <Extension()> Public Function as_int32(ByVal this As _int64) As Int32
        Dim x As Int64 = 0
        x = +this
        assert(x >= min_int32 AndAlso x <= max_int32)
        Return CInt(x)
    End Function

    <Extension()> Public Function as_uint32(ByVal this As _int64) As UInt32
        Dim x As Int64 = 0
        x = +this
        assert(x >= min_uint32 AndAlso x <= max_uint32)
        Return CUInt(x)
    End Function

    <Extension()> Public Function as_uint64(ByVal this As _int64) As UInt64
        Dim x As Int64 = 0
        x = +this
        assert(x >= min_uint64 AndAlso x <= max_uint64)
        Return CULng(x)
    End Function
End Module
