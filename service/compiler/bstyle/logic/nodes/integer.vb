
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _integer
        Inherits raw_value

        Public Sub New()
            MyBase.New(code_types.int)
        End Sub

        Public Overloads Shared Function build(ByVal i As Int32, ByVal o As logic_writer) As Boolean
            Return code_gens().typed(Of _integer)().build(New data_block(i), o)
        End Function

        Protected Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            Dim i As Int32 = 0
            If Not Int32.TryParse(n.word().str(), i) Then
                Return False
            End If
            o = New data_block(i)
            Return True
        End Function
    End Class
End Class
