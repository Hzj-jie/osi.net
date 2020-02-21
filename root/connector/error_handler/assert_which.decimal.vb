
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure decimal_assertion
        Private ReadOnly i As Decimal

        Public Sub New(ByVal i As Decimal)
            Me.i = i
        End Sub

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
