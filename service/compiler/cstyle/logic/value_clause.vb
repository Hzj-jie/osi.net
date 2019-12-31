
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class value_clause
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Shared Sub register(ByVal b As builders)
        assert(Not b Is Nothing)
        b.register(Of value_clause)()
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        assert(n.child_count() = 3)
        If Not b.of(n.child(0)).build(o) Then
            o.err("@value-clause name ", n.child(0))
            Return False
        End If
        builder_of(Of value).current_target(n.child(0))
        If Not b.of(n.child(2)).build(o) Then
            o.err("@value-clause value ", n.child(2))
            Return False
        End If
        Return True
    End Function
End Class
