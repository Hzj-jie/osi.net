
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class for_loop
        Inherits builder_wrapper
        Implements builder

        <inject_constructor>
        Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As builders)
            assert(Not b Is Nothing)
            b.register(Of for_loop)()
        End Sub

        Private NotInheritable Class ref
            Public ReadOnly declaration As typed_node
            Public ReadOnly condition As typed_node
            Public ReadOnly clause As typed_node
            Public ReadOnly paragraph As typed_node

            Public Sub New(ByVal lp As lang_parser, ByVal n As typed_node)
                assert(Not n Is Nothing)
                assert(n.child_count() > 6)
                Dim m As typed_node.child_named_map = Nothing
                m = n.named_children(lp)
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
            If n.condition Is Nothing OrElse
           b.of(n.condition).build(o) Then
                Return True
            End If
            o.err("@for-loop value ", n.condition)
            Return False
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
            Dim ref As ref = Nothing
            ref = New ref(lp, n)
            assert(Not o Is Nothing)
            If Not ref.declaration Is Nothing Then
                If Not b.of(ref.declaration).build(o) Then
                    o.err("@for-loop value-declaration", ref.declaration)
                    Return False
                End If
            End If
            Using value_target As value.target = builder_of(Of value).with_value_target(ref.condition)
                o.append("define", value_target.value_name, "bool")
                If Not condition_value(ref, o) Then
                    Return False
                End If
                o.append("while_then", value_target.value_name, "{")
                If Not b.[of](ref.paragraph).build(o) Then
                    o.err("@for-loop paragraph ", ref.paragraph)
                    Return False
                End If
                If Not ref.clause Is Nothing Then
                    If Not b.of(ref.clause).build(o) Then
                        o.err("@for-loop value-clause", ref.clause)
                        Return False
                    End If
                End If
                If Not condition_value(ref, o) Then
                    Return False
                End If
                o.append("}")
            End Using
            Return True
        End Function
    End Class
End Class
