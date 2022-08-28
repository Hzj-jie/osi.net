
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class temp_logic_name_t
        Private id As UInt32 = 0

        Public Function variable() As String
            id += uint32_1
            Return "temp_value_@" + id.ToString()
        End Function
    End Class
End Class
