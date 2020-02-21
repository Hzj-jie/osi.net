
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure int64_assertion
        Private ReadOnly i As Int64

        Public Sub New(ByVal i As Int64)
            Me.i = i
        End Sub

        Public Function can_cast_to_uint32() As UInt32
            Try
                Return CUInt(i)
            Catch ex As OverflowException
                assert(i >= 0, ex)
                assert(i <= max_uint32, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function

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

        Public Function can_cast_to_uint64() As UInt64
            Try
                Return CULng(i)
            Catch ex As OverflowException
                assert(i >= 0, ex)
                assert(False, ex)
                Return 0
            End Try
        End Function
    End Structure
End Class
