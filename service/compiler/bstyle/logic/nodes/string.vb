
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _string
        Inherits raw_value

        Public Shared ReadOnly instance As New _string()

        Private Sub New()
            MyBase.New(code_types.string)
        End Sub

        Public Overloads Shared Function build(ByVal n As typed_node,
                                               ByVal s As String,
                                               ByVal o As logic_writer) As Boolean
            Return instance.build(n, New data_block(s), o)
        End Function

        Protected Overrides Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean
            o = New data_block(n.word().str().Trim(character.quote).c_unescape())
            Return True
        End Function
    End Class
End Class
