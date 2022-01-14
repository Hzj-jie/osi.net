
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class condition
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                     value.read_target_single_data_slot()
                Dim condition As String = Nothing
                ' TODO: May want to restrict the type of condition.
                If Not read_target.retrieve(condition) Then
                    raise_error(error_type.user, "Condition of if cannot be a struct.")
                    Return False
                End If
                Dim satisfied_paragraph As Func(Of writer, Boolean) = Function(ByVal oo As writer) As Boolean
                                                                          Return l.[of](n.child(4)).build(oo)
                                                                      End Function
                If n.child_count() = 5 Then
                    Return builders.of_if(condition, satisfied_paragraph).to(o)
                End If
                Dim unsatisfied_paragraph As Func(Of writer, Boolean) = Function(ByVal oo As writer) As Boolean
                                                                            Return l.[of](n.child(5)).build(oo)
                                                                        End Function
                Return builders.of_if(condition, satisfied_paragraph, unsatisfied_paragraph).to(o)
            End Using
        End Function
    End Class
End Class
