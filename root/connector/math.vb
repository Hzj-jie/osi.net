
Option Explicit On
Option Infer Off
Option Strict On

'legacy, other math related functions should be put in to math service
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _math
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_integral(ByVal i As Double) As Boolean
        Return i = Math.Truncate(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_integral(ByVal i As Decimal) As Boolean
        Return i = Math.Truncate(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_int(ByVal i As Double) As Boolean
        Return i.is_integral() AndAlso
               i >= min_int32 AndAlso
               i <= max_int32
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_not_integral(ByVal i As Double) As Boolean
        Return Not is_integral(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_not_integral(ByVal i As Decimal) As Boolean
        Return Not is_integral(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function is_not_int(ByVal i As Double) As Boolean
        Return Not is_int(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As SByte) As Int16
        Return CShort(i) * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Byte) As UInt16
        Return CUShort(i) * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Int16) As Int32
        Return CInt(i) * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As UInt16) As UInt32
        Return CUInt(i) * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Int32) As Int64
        Return CLng(i) * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As UInt32) As UInt64
        Return CULng(i) * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function power_2(ByVal i As Double) As Double
        Return i * i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As SByte) As Boolean
        Return (i And int8_1) = int8_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Byte) As Boolean
        Return (i And uint8_1) = uint8_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Int16) As Boolean
        Return (i And int16_1) = int16_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As UInt16) As Boolean
        Return (i And uint16_1) = uint16_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Int32) As Boolean
        Return (i And int32_1) = int32_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As UInt32) As Boolean
        Return (i And uint32_1) = uint32_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As Int64) As Boolean
        Return (i And int64_1) = int64_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function odd(ByVal i As UInt64) As Boolean
        Return (i And uint64_1) = uint64_1
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As SByte) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Byte) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Int16) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As UInt16) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Int32) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As UInt32) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As Int64) As Boolean
        Return Not odd(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function even(ByVal i As UInt64) As Boolean
        Return Not odd(i)
    End Function
End Module
