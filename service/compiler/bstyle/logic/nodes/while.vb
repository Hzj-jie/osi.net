
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class _while
        Implements code_gen(Of logic_writer)

        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Private Function while_value(ByVal n As typed_node,
                                     ByVal o As logic_writer,
                                     ByRef condition As String) As Boolean
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using value_target As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                    value.read_target_single_data_slot()
                ' TODO: May want to restrict the type of condition.
                If Not value_target.retrieve(condition) Then
                    raise_error(error_type.user, "Condition of while cannot be a struct.")
                    Return False
                End If
                Return True
            End Using
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 5)
            Dim condition As String = Nothing
            If Not while_value(n, o, condition) Then
                Return False
            End If
            Return builders.of_while_then(condition,
                                          Function() As Boolean
                                              Dim cur_condition As String = Nothing
                                              Return l.of(n.child(4)).build(o) AndAlso
                                                     while_value(n, o, cur_condition) AndAlso
                                                     builders.of_copy(condition, cur_condition).to(o)
                                          End Function).to(o)
        End Function
    End Class
End Class
