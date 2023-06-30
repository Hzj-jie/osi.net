
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

'shortcuts
Public Module _abs
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As SByte) As SByte
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As Int16) As Int16
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As Int32) As Int32
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As Int64) As Int64
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As Single) As Single
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As Double) As Double
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(i As Decimal) As Decimal
        Return System.Math.Abs(i)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As Byte) As Byte
        Return i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As UInt16) As UInt16
        Return i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As UInt32) As UInt32
        Return i
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function abs(ByVal i As UInt64) As UInt64
        Return i
    End Function
End Module
