
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class condition
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Shared Sub register(ByVal b As builders)
        assert(Not b Is Nothing)
        b.register(Of condition)()
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        assert(n.child_count() >= 5)
        Dim value_name As String = Nothing
        value_name = builder_of(Of value).value_target(n.child(2))
        o.append("define").append(value_name).append("bool")
        If Not b.[of](n.child(2)).build(o) Then
            o.err("@condition value ", n.child(2))
            Return False
        End If
        o.append("if").append(value_name).append("{")
        If Not b.[of](n.child(4)).build(o) Then
            o.err("@condition paragraph ", n.child(4))
            Return False
        End If
        o.append("}")
        If n.child_count() = 6 Then
            o.append("else").append("{")
            If Not b.[of](n.child(5)).build(o) Then
                o.err("@condition else-condition ", n.child(5))
                Return False
            End If
            o.append("}")
        End If
        Return True
    End Function
End Class
