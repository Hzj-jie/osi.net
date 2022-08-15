
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class temp_logic_name_t
        Private id As UInt32 = 0

        Public Function variable() As String
            id += uint32_1
            Return "temp_value_@" + id.ToString()
        End Function
    End Class
End Class
