
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class [while]
        Inherits builder_wrapper
        Implements builder

        <inject_constructor>
        Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As builders)
            assert(Not b Is Nothing)
            b.register(Of [while])()
        End Sub

        Private Function while_value(ByVal n As typed_node, ByVal o As writer) As Boolean
            If Not b.[of](n.child(2)).build(o) Then
                o.err("@while value ", n.child(2))
                Return False
            End If
            Return True
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 5)
            Using value_target As value.target = builder_of(Of value).with_value_target(n.child(2))
                o.append("define", value_target.value_name, "bool")
                If Not while_value(n, o) Then
                    Return False
                End If
                o.append("while_then", value_target.value_name, "{")
                If Not b.[of](n.child(4)).build(o) Then
                    o.err("@while paragraph ", n.child(4))
                    Return False
                End If
                If Not while_value(n, o) Then
                    Return False
                End If
                o.append("}")
            End Using
            Return True
        End Function
    End Class
End Class
