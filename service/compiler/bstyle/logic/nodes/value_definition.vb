
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class value_definition
        Implements code_gen(Of logic_writer)

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            Return value_declaration.build(n.child(0), n.child(1), o) AndAlso
                   code_gens().typed(Of value_clause)().build(n.child(1), n.child(3), o)
        End Function
    End Class
End Class
