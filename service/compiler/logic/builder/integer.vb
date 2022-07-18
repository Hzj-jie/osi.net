
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Public MustInherit Class _integer(Of TEMP_TARGET As func_t(Of String, logic_writer, String))
        Inherits raw_value(Of TEMP_TARGET)

        Public Const type_name As String = "Integer"

        Public Sub New()
            MyBase.New(type_name)
        End Sub

        Protected NotOverridable Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            Dim i As Int32 = 0
            If Not Int32.TryParse(n.word().str(), i) Then
                Return False
            End If
            o = New data_block(i)
            Return True
        End Function
    End Class
End Class
