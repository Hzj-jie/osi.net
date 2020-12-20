
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure double_assertion
        Private ReadOnly i As Double

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal i As Double)
            Me.i = i
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_uint32() As UInt32
            assert(i.is_integral())
            Try
                Return CUInt(i)
            Catch ex As OverflowException
                assert(i <= max_uint32, ex)
                assert(i >= 0, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_cast_to_uint64() As UInt64
            assert(i.is_integral())
            Try
                Return CULng(i)
            Catch ex As OverflowException
                assert(i <= max_uint64, ex)
                assert(i >= 0, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_floor_to_uint64() As UInt64
            Return assert_which.of(Math.Floor(i)).can_cast_to_uint64()
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function can_truncate_to_uint32() As UInt32
            Try
                Return CUInt(i)
            Catch ex As OverflowException
                assert(i <= max_uint32, ex)
                assert(i >= 0, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function
    End Structure
End Class
