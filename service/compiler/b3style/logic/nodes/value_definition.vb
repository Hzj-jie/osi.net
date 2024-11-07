
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class value_definition
        Implements code_gen(Of logic_writer)

        Public Shared Function name_of(ByVal r As String) As String
            assert(Not r.null_or_whitespace())
            If _disable_namespace Then
                Return r
            End If
            Return scope.current_namespace_t.of(r)
        End Function

        Public Shared Function name_of(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            Return name_of(n.input_without_ignored())
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            Return value_declaration.of(n, o) AndAlso
                   value_clause.stack_name_build(n.child(1), n.child(3), o)
        End Function
    End Class
End Class
