
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.service.automata

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_definition(Of BUILDER As func_t(Of String, logic_writer, Boolean),
                                                    CODE_GENS As func_t(Of code_gens(Of logic_writer)),
                                                    T As scope(Of logic_writer, BUILDER, CODE_GENS, T))
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            Return value_declaration(Of BUILDER, CODE_GENS, T).build(n.child(0), n.child(1), o) AndAlso
                   value_clause(Of BUILDER, CODE_GENS, T).build(n.child(1), n.child(3), o)
        End Function
    End Class
End Class
