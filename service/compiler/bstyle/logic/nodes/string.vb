
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
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New _string()

        Private Sub New()
        End Sub

        Public Shared Function build(ByVal n As typed_node, ByVal s As String, ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not s Is Nothing)
            assert(Not o Is Nothing)
            Return builders.of_copy_const(value.with_single_data_slot_temp_target(code_types.string, n, o),
                                          New data_block(s)).
                            to(o)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.leaf())
            Return build(n, n.word().str().Trim(character.quote).c_unescape(), o)
        End Function
    End Class
End Class
