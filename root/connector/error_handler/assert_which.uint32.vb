
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure uint32_assertion
        Private ReadOnly i As UInt32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal i As UInt32)
            Me.i = i
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_int32() As Int32
            Try
                Return CInt(i)
            Catch ex As OverflowException
                assert(i <= max_int32, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_byte() As Byte
            Try
                Return CByte(i)
            Catch ex As OverflowException
                assert(i <= max_uint8, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function less_than(ByVal v As UInt32) As UInt32
            assert(i < v)
            Return i
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function greater_than(ByVal v As UInt32) As UInt32
            assert(i > v)
            Return i
        End Function
    End Structure
End Class
