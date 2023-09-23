
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class value_list
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() > 0)
            Return build(Sub(ByVal a As Action(Of typed_node))
                             assert(Not a Is Nothing)
                             Dim i As UInt32 = 0
                             While i < n.child_count()
                                 a(n.child(i))
                                 i += uint32_1
                             End While
                         End Sub,
                         o)
        End Function

        Public Shared Function build(ByVal foreach As Action(Of Action(Of typed_node)),
                                     ByVal o As logic_writer) As Boolean
            assert(Not foreach Is Nothing)
            assert(Not o Is Nothing)
            Dim v As New vector(Of String)()
            Try
                foreach(Sub(ByVal n As typed_node)
                            If Not code_gen_of(n).build(o) Then
                                break_lambda.at_here()
                            End If
                            Using r As read_scoped(Of scope.value_target_t.target).ref =
                                           scope.current().value_target().value()
                                v.emplace_back((+r).names)
                            End Using
                        End Sub)
            Catch ex As break_lambda
                Return False
            End Try
            scope.current().value_target().with_value_list(v)
            Return True
        End Function

        Public Shared Sub with_empty()
            scope.current().value_target().with_value_list(New vector(Of String)())
        End Sub

        Public Shared Function current_targets() As read_scoped(Of vector(Of String)).ref
            Return scope.current().value_target().value_list()
        End Function
    End Class
End Class
