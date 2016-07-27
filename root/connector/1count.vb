
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module __1count
    Private Function _positive_1count(ByVal b As SByte) As Byte
        assert(b >= 0)
        Dim i As Byte = 0
        While b > 0
            b = b And (b - 1)
            i += 1
        End While

        Return i
    End Function

    <Extension()> Public Function _1count(ByVal b As SByte) As Byte
        If b < 0 Then
            Return _positive_1count(max_int8 And int8_uint8(b)) + 1
        Else
            Return _positive_1count(b)
        End If
    End Function

    <Extension()> Public Function _1count(ByVal b As Byte) As Byte
        Return _1count(uint8_int8(b))
    End Function

    <Extension()> Public Function onecount(ByVal b As SByte) As Byte
        Return _1count(b)
    End Function

    <Extension()> Public Function onecount(ByVal b As Byte) As Byte
        Return _1count(b)
    End Function

    Private Function _positive_1count(ByVal b As Int16) As Byte
        assert(b >= 0)
        Dim i As Byte = 0
        While b > 0
            b = b And (b - 1)
            i += 1
        End While

        Return i
    End Function

    <Extension()> Public Function _1count(ByVal b As Int16) As Byte
        If b < 0 Then
            Return _positive_1count(max_int16 And int16_uint16(b)) + 1
        Else
            Return _positive_1count(b)
        End If
    End Function

    <Extension()> Public Function _1count(ByVal b As UInt16) As Byte
        Return _1count(uint16_int16(b))
    End Function

    <Extension()> Public Function onecount(ByVal b As Int16) As Byte
        Return _1count(b)
    End Function

    <Extension()> Public Function onecount(ByVal b As UInt16) As Byte
        Return _1count(b)
    End Function

    Private Function _positive_1count(ByVal b As Int32) As Byte
        assert(b >= 0)
        Dim i As Byte = 0
        While b > 0
            b = b And (b - 1)
            i += 1
        End While

        Return i
    End Function

    <Extension()> Public Function _1count(ByVal b As Int32) As Byte
        If b < 0 Then
            Return _positive_1count(max_int32 And int32_uint32(b)) + 1
        Else
            Return _positive_1count(b)
        End If
    End Function

    <Extension()> Public Function _1count(ByVal b As UInt32) As Byte
        Return _1count(uint32_int32(b))
    End Function

    <Extension()> Public Function onecount(ByVal b As Int32) As Byte
        Return _1count(b)
    End Function

    <Extension()> Public Function onecount(ByVal b As UInt32) As Byte
        Return _1count(b)
    End Function

    Private Function _positive_1count(ByVal b As Int64) As Byte
        assert(b >= 0)
        Dim i As Byte = 0
        While b > 0
            b = b And (b - 1)
            i += 1
        End While

        Return i
    End Function

    <Extension()> Public Function _1count(ByVal b As Int64) As Byte
        If b < 0 Then
            Return _positive_1count(CLng(CULng(max_int64) And int64_uint64(b))) + 1
        Else
            Return _positive_1count(b)
        End If
    End Function

    <Extension()> Public Function _1count(ByVal b As UInt64) As Byte
        Return _1count(uint64_int64(b))
    End Function

    <Extension()> Public Function onecount(ByVal b As Int64) As Byte
        Return _1count(b)
    End Function

    <Extension()> Public Function onecount(ByVal b As UInt64) As Byte
        Return _1count(b)
    End Function

    <Extension()> Public Function power_of_2(ByVal b As SByte) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As Byte) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As UInt16) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As Int16) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As UInt32) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As Int32) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As UInt64) As Boolean
        Return b._1count() = 1
    End Function

    <Extension()> Public Function power_of_2(ByVal b As Int64) As Boolean
        Return b._1count() = 1
    End Function
End Module
