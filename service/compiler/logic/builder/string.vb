
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Public MustInherit Class _string(Of TEMP_TARGET As func_t(Of String, logic_writer, String))
        Inherits raw_value(Of TEMP_TARGET)

        Public Const type_name As String = "String"

        Public Sub New()
            MyBase.New(type_name)
        End Sub

        Protected NotOverridable Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            o = New data_block(n.word().str().Trim(character.quote).c_unescape())
            Return True
        End Function
    End Class
End Class
