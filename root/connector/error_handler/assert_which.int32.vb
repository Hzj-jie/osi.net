
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure int32_assertion
        Private ReadOnly i As Int32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal i As Int32)
            Me.i = i
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_positive() As Int32
            assert(i > 0)
            Return i
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_uint32() As UInt32
            Try
                Return CUInt(i)
            Catch ex As OverflowException
                assert(i >= 0, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_byte() As Byte
            Try
                Return CByte(i)
            Catch ex As OverflowException
                assert(i >= 0, ex)
                assert(i <= max_uint8, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function
    End Structure
End Class
