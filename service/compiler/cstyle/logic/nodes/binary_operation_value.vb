
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Partial Public NotInheritable Class binary_operation_value
        Inherits builder_wrapper
        Implements builder

        <inject_constructor>
        Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As builders)
            assert(Not b Is Nothing)
            b.register(Of binary_operation_value)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Dim left As value.target = Nothing
            left = builder_of(Of value).with_value_target(n.child(0))
            Using left
                If Not b.of(n.child(0)).build(o) Then
                    o.err("@binary-operation-value value [0] ", n.child(0))
                    Return False
                End If
            End Using
            Dim right As value.target = Nothing
            right = builder_of(Of value).with_value_target(n.child(2))
            Using right
                If Not b.of(n.child(2)).build(o) Then
                    o.err("@binary-operation-value value [1] ", n.child(2))
                    Return False
                End If
            End Using
            Using with_current_targets(left, right)
                If Not b.of(n.child(1)).build(o) Then
                    o.err("@binary-operation-value operator ", n.child(1))
                    Return False
                End If
            End Using
            Return True
        End Function
    End Class
End Class
