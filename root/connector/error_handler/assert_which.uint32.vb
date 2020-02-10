﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class assert_which
    Public Structure uint32_assertion
        Private ReadOnly i As UInt32

        Public Sub New(ByVal i As UInt32)
            Me.i = i
        End Sub

        Public Function can_cast_to_int32() As Int32
            assert(i <= max_int32)
            Return CInt(i)
        End Function

        Public Function can_cast_to_byte() As Byte
            assert(i <= max_uint8)
            Return CByte(i)
        End Function
    End Structure
End Class
