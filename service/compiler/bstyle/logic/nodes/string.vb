
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public MustInherit Class _string(Of TEMP_TARGET As func_t(Of String, logic_writer, String))
        Inherits raw_value(Of code_type, TEMP_TARGET)

        Public Const type_name As String = "String"

        Public Structure code_type
            Implements func_t(Of String)

            Public Function run() As String Implements func_t(Of String).run
                Return type_name
            End Function
        End Structure

        Protected NotOverridable Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            o = New data_block(n.word().str().Trim(character.quote).c_unescape())
            Return True
        End Function
    End Class

    Private NotInheritable Class _string
        Inherits _string(Of scope.value_target_t.with_primitive_type_temp_target_t)

        Public Overloads Shared Function build(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return _string.build(New data_block(s), o)
        End Function
    End Class
End Class
