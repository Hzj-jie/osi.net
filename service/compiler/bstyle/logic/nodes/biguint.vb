
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports osi.service.math

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class biguint
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New biguint()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim s As String = n.word().str()
            s = strmid(s, 0, strlen(s) - uint32_1)
            Dim i As big_uint = Nothing
            If Not big_uint.parse(s, i) Then
                raise_error(error_type.user, "Cannot parse data to biguint ", n.trace_back_str())
                Return False
            End If
            Return builders.of_copy_const(value.with_single_data_slot_temp_target(code_types.biguint, n, o),
                                          New data_block(i.as_bytes())).to(o)
        End Function
    End Class
End Class
