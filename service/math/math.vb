
Option Explicit On
Option Infer Off

Imports System.Runtime.CompilerServices
Imports osi.root
Imports osi.root.connector
Imports osi.root.constants

Public Module _math
    <Extension()> Public Function even(ByVal i As SByte) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As SByte) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As Byte) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As Byte) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As Int16) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As Int16) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As UInt16) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As UInt16) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As Int32) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As Int32) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As UInt32) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As UInt32) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As Int64) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As Int64) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function even(ByVal i As UInt64) As Boolean
        Return Not i.odd()
    End Function

    <Extension()> Public Function odd(ByVal i As UInt64) As Boolean
        Return i.getrbit(0)
    End Function

    <Extension()> Public Function sqrt(ByVal i As SByte) As SByte
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As Byte) As Byte
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As Int16) As Int16
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As UInt16) As UInt16
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As Int32) As Int32
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As UInt32) As UInt32
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As Int64) As Int64
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function sqrt(ByVal i As UInt64) As UInt64
        Return CDbl(i).integeral_sqrt()
    End Function

    <Extension()> Public Function integeral_sqrt(ByVal i As Double) As Int64
        Return System.Math.Truncate(System.Math.Sqrt(i))
    End Function

    <Extension()> Public Function sawtooth(ByVal i As Double) As Double
        Return i - System.Math.Floor(i)
    End Function

    <Extension()> Public Function sawtooth(ByVal i As Decimal) As Decimal
        Return i - System.Math.Floor(i)
    End Function

    <Extension()> Public Function is_integral(ByVal i As Double) As Boolean
        Return connector.is_integral(i)
    End Function

    <Extension()> Public Function is_int(ByVal i As Double) As Boolean
        Return connector.is_int(i)
    End Function

    <Extension()> Public Function is_not_integral(ByVal i As Double) As Boolean
        Return connector.is_not_integral(i)
    End Function

    <Extension()> Public Function is_not_int(ByVal i As Double) As Boolean
        Return connector.is_not_int(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As SByte) As Int16
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As Byte) As UInt16
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As Int16) As Int32
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As UInt16) As UInt32
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As Int32) As Int64
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As UInt32) As UInt64
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function power_2(ByVal i As Double) As Double
        Return connector.power_2(i)
    End Function

    <Extension()> Public Function binary_trailing_zero_count(ByVal b As Byte) As Byte
        assert(b > 0)
        Dim r As Byte = 0
        While b.even()
            r += uint8_1
            b >>= 1
        End While
        Return r
    End Function

    <Extension()> Public Function binary_trailing_zero_count(ByVal b As UInt16) As Byte
        assert(b > 0)
        Dim r As Byte = 0
        While b.even()
            r += uint8_1
            b >>= 1
        End While
        Return r
    End Function

    <Extension()> Public Function binary_trailing_zero_count(ByVal b As UInt32) As Byte
        assert(b > 0)
        Dim r As Byte = 0
        While b.even()
            r += uint8_1
            b >>= 1
        End While
        Return r
    End Function

    <Extension()> Public Function binary_trailing_zero_count(ByVal b As UInt64) As Byte
        assert(b > 0)
        Dim r As Byte = 0
        While b.even()
            r += uint8_1
            b >>= 1
        End While
        Return r
    End Function
End Module
