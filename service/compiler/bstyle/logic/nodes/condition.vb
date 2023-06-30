
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class condition
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            If Not code_gen_of(n.child(2)).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                    scope.current().value_target().primitive_type()
                Dim condition As String = Nothing
                ' TODO: May want to restrict the type of condition.
                If Not read_target.retrieve(condition) Then
                    raise_error(error_type.user, "Condition of if cannot be a struct.")
                    Return False
                End If
                Dim satisfied_paragraph As Func(Of logic_writer, Boolean) =
                        Function(ByVal oo As logic_writer) As Boolean
                            Return code_gen_of(n.child(4)).build(oo)
                        End Function
                If n.child_count() = 5 Then
                    Return builders.of_if(condition, satisfied_paragraph).to(o)
                End If
                Dim unsatisfied_paragraph As Func(Of logic_writer, Boolean) =
                        Function(ByVal oo As logic_writer) As Boolean
                            Return code_gen_of(n.child(5)).build(oo)
                        End Function
                Return builders.of_if(condition, satisfied_paragraph, unsatisfied_paragraph).to(o)
            End Using
        End Function
    End Class
End Class
