
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_declaration
        Inherits logic_gen_wrapper_with_parameters
        Implements logic_gen

        Private Sub New(ByVal b As logic_gens, ByVal l As parameters_t)
            MyBase.New(b, l)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens, ByVal l As parameters_t)
            assert(Not b Is Nothing)
            assert(Not l Is Nothing)
            b.register(New value_declaration(b, l))
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            If Not code_gen_of(Of struct).export(n, o) Then
                builders.of_define(ta, n.child(1).word().str(), n.child(0).word().str()).to(o)
            End If
            Return True
        End Function
    End Class
End Class
