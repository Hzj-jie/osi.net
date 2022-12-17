
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
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

    Private NotInheritable Class _integer
        Inherits _integer(Of scope.value_target_t.with_primitive_type_temp_target_t)

        Public Overloads Shared Function build(ByVal i As Int32, ByVal o As logic_writer) As Boolean
            Return code_gens().typed(Of _integer)().build(New data_block(i), o)
        End Function
    End Class
End Class
