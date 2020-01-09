
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class for_loop
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of for_loop)()
        End Sub

        Private NotInheritable Class ref
            Public ReadOnly declaration As typed_node
            Public ReadOnly condition As typed_node
            Public ReadOnly clause As typed_node
            Public ReadOnly paragraph As typed_node

            Public Sub New(ByVal n As typed_node)
                assert(Not n Is Nothing)
                assert(n.child_count() > 6)
                Dim m As typed_node.child_named_map = Nothing
                m = n.named_children()
                If Not m.node("value-declaration", declaration) Then
                    declaration = Nothing
                End If
                If Not m.node("value", condition) Then
                    condition = Nothing
                End If
                If Not m.node("value-clause", clause) Then
                    clause = Nothing
                End If
                paragraph = m.node("paragraph")
            End Sub
        End Class

        Private Function condition_value(ByVal n As ref, ByVal o As writer) As Boolean
            Return n.condition Is Nothing OrElse l.of(n.condition).build(o)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            Dim ref As ref = Nothing
            ref = New ref(n)
            assert(Not o Is Nothing)
            If Not ref.declaration Is Nothing AndAlso Not l.of(ref.declaration).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of String).ref = value.read_target()
                If Not condition_value(ref, o) Then
                    Return False
                End If
                Return builders.of_while_then(+read_target,
                                              Function() As Boolean
                                                  Return l.of(ref.paragraph).build(o) AndAlso
                                                         (ref.clause Is Nothing OrElse
                                                          l.of(ref.clause).build(o)) AndAlso
                                                         condition_value(ref, o)
                                              End Function).to(o)
            End Using
        End Function
    End Class
End Class
