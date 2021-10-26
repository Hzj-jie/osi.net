
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class [while]
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of [while])()
        End Sub

        Private Function while_value(ByVal n As typed_node, ByVal o As writer) As Boolean
            Return l.of(n.child(2)).build(o)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 5)
            If Not while_value(n, o) Then
                Return False
            End If
            Using value_target As read_scoped(Of vector(Of String)).ref(Of String) =
                    l.typed_code_gen(Of value)().read_target_internal_typed()
                Dim condition As String = Nothing
                If Not value_target.retrieve(condition) Then
                    raise_error(error_type.user, "Condition of while cannot be a struct.")
                    Return False
                End If
                Return builders.of_while_then(condition,
                                              Function() As Boolean
                                                  Return l.of(n.child(4)).build(o) AndAlso while_value(n, o)
                                              End Function).to(o)
            End Using
        End Function
    End Class
End Class
