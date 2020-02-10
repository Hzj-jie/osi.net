
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure double_assertion
        Private ReadOnly i As Double

        Public Sub New(ByVal i As Double)
            Me.i = i
        End Sub

        Public Function can_cast_to_uint32() As UInt32
            assert(i <= max_uint32)
            assert(i >= 0)
            assert(i.is_integral())
            Return CUInt(i)
        End Function
    End Structure
End Class
