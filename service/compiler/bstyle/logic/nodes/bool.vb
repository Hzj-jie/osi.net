
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public MustInherit Class bool(Of TEMP_TARGET As func_t(Of String, logic_writer, String))
        Inherits raw_value(Of code_type, TEMP_TARGET)

        Public Const type_name As String = "Boolean"

        Public Structure code_type
            Implements func_t(Of String)

            Public Function run() As String Implements func_t(Of String).run
                Return type_name
            End Function
        End Structure

        Protected NotOverridable Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            Dim i As Boolean = False
            If Not str_bool(n.word().str(), i) Then
                Return False
            End If
            o = New data_block(i)
            Return True
        End Function
    End Class

    Private NotInheritable Class bool
        Inherits bool(Of scope.value_target_t.with_primitive_type_temp_target_t)
    End Class
End Class
