
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive
Imports osi.service.math

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class ufloat
        Implements code_gen(Of logic_writer)

        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim i As big_udec = Nothing
            If Not big_udec.parse(n.word().str(), i) Then
                raise_error(error_type.user, "Cannot parse data to bigufloat ", n.trace_back_str())
                Return False
            End If
            Return builders.of_copy_const(value.with_single_data_slot_temp_target(code_types.ufloat, n, o),
                                          New data_block(i.as_bytes())).to(o)
        End Function
    End Class
End Class
