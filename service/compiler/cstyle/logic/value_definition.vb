
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class value_definition
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Shared Sub register(ByVal b As builders)
        assert(Not b Is Nothing)
        b.register(Of value_definition)()
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        o.append("define")
        assert(n.child_count() = 4)
        If Not b.of(n.child(1)).build(o) Then
            o.err("@value-definition name ", n.child(1))
            Return False
        End If
        If Not b.of(n.child(0)).build(o) Then
            o.err("@value-definition type ", n.child(0))
            Return False
        End If
        builder_of(Of value).current_target(n.child(1))
        If Not b.of(n.child(3)).build(o) Then
            o.err("@value-definition value ", n.child(3))
            Return False
        End If
        Return True
    End Function
End Class
