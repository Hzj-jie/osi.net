
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_definition
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_definition)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            If Not code_gen_of(Of value_declaration).declare_internal_typed_variable(n, o) Then
                Return False
            End If
            If Not l.of(n.child(3)).build(o) Then
                Return False
            End If
            Using r As read_scoped(Of vector(Of String)).ref(Of String) =
                    code_gen_of(Of value)().read_target_only()
                builders.of_move(n.child(1).word().str(), +r).to(o)
            End Using
            Return True
        End Function
    End Class
End Class
