
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _integer
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New _integer()

        Private Sub New()
        End Sub

        Public Shared Function build(ByVal n As typed_node, ByVal i As Int32, ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return builders.of_copy_const(value.with_single_data_slot_temp_target(code_types.int, n, o),
                                          New data_block(i)).
                            to(o)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.leaf())
            Dim i As Int32 = 0
            If Not Int32.TryParse(n.word().str(), i) Then
                raise_error(error_type.user, "Cannot parse data to int ", n.trace_back_str())
                Return False
            End If
            Return build(n, i, o)
        End Function
    End Class
End Class
