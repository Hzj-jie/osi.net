
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root
Imports osi.root.connector
Imports osi.root.constants

Public Module _math
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As SByte) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As SByte) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Byte) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Byte) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Int16) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Int16) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As UInt16) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As UInt16) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Int32) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Int32) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As UInt32) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As UInt32) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Int64) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Int64) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As UInt64) As Boolean
        Return Not i.odd()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As UInt64) As Boolean
        Return i.getrbit(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As SByte) As SByte
        Return CSByte(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As Byte) As Byte
        Return CByte(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As Int16) As Int16
        Return CShort(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As UInt16) As UInt16
        Return CUShort(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As Int32) As Int32
        Return CInt(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As UInt32) As UInt32
        Return CUInt(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As Int64) As Int64
        Return CDbl(i).integeral_sqrt()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sqrt(ByVal i As UInt64) As UInt64
        Return CULng(CDbl(i).integeral_sqrt())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function integeral_sqrt(ByVal i As Double) As Int64
        Return CLng(System.Math.Truncate(System.Math.Sqrt(i)))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sawtooth(ByVal i As Double) As Double
        Return i - System.Math.Floor(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function sawtooth(ByVal i As Decimal) As Decimal
        Return i - System.Math.Floor(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_integral(ByVal i As Double) As Boolean
        Return connector.is_integral(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_int(ByVal i As Double) As Boolean
        Return connector.is_int(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_not_integral(ByVal i As Double) As Boolean
        Return connector.is_not_integral(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_not_int(ByVal i As Double) As Boolean
        Return connector.is_not_int(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As SByte) As Int16
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Byte) As UInt16
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Int16) As Int32
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As UInt16) As UInt32
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Int32) As Int64
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As UInt32) As UInt64
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Double) As Double
        Return connector.power_2(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function trailing_binary_zero_count(ByVal b As Byte) As Byte
        assert(b > 0)
        If (b And &HF) = 0 Then
            If (b And &H1F) <> 0 Then
                Return 4
            End If
            If (b And &H3F) <> 0 Then
                Return 5
            End If
            If (b And &H7F) <> 0 Then
                Return 6
            End If
            Return 7
        End If
        If (b And &H7) = 0 Then
            Return 3
        End If
        If (b And &H3) = 0 Then
            Return 2
        End If
        If (b And &H1) = 0 Then
            Return 1
        End If
        Return 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function trailing_binary_zero_count(ByVal b As UInt16) As Byte
        assert(b > 0)
        If (b And max_uint8) = 0 Then
            Return CByte(8) + CByte(b >> 8).trailing_binary_zero_count()
        End If
        Return CByte(b And max_uint8).trailing_binary_zero_count()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function trailing_binary_zero_count(ByVal b As UInt32) As Byte
        assert(b > 0)
        If (b And max_uint16) = 0 Then
            Return CByte(16) + CUShort(b >> 16).trailing_binary_zero_count()
        End If
        Return CUShort(b And max_uint16).trailing_binary_zero_count()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function trailing_binary_zero_count(ByVal b As UInt64) As Byte
        assert(b > 0)
        If (b And max_uint32) = 0 Then
            Return CByte(32) + CUInt(b >> 32).trailing_binary_zero_count()
        End If
        Return CUInt(b And max_uint32).trailing_binary_zero_count()
    End Function
End Module
