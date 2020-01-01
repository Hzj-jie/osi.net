
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class value_with_bracket
        Inherits builder_wrapper
        Implements builder

        <inject_constructor>
        Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
            MyBase.New(b, lp)
        End Sub

        Public Shared Sub register(ByVal b As builders)
            assert(Not b Is Nothing)
            b.register(Of value_with_bracket)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            If Not b.of(n.child(1)).build(o) Then
                o.err("@value_with_bracket ", n.child())
                Return False
            End If
            Return True
        End Function
    End Class
End Class
