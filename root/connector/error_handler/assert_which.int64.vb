
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
            assert(i >= 0)
            assert(i <= max_uint32)
            Return CUInt(i)
        End Function

        Public Function can_cast_to_byte() As Byte
            assert(i >= 0)
            assert(i <= max_uint8)
            Return CByte(i)
        End Function

        Public Function can_cast_to_uint64() As UInt64
            assert(i >= 0)
            Return CULng(i)
        End Function
    End Structure
End Class
