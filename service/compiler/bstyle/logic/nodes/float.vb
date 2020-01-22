
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class float
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of float)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim i As Double = 0
            If Not Double.TryParse(n.word().str(), i) Then
                raise_error(error_type.user, "Cannot parse data to float ", n.trace_back_str())
                Return False
            End If
            Using r As read_scoped(Of value.write_target_ref).ref = code_gen_of(Of value)().write_target()
                With +r
                    builders.of_copy_const(.name, New data_block(i)).to(o)
                    .set_type(types.float)
                End With
            End Using
            Return True
        End Function
    End Class
End Class
