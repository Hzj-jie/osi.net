
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class param
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        If Not b.of(n.child(1)).build(o) Then
            o.err("@param name ", n.child(1))
        End If
        If Not b.of(n.child(0)).build(o) Then
            o.err("@param type ", n.child(0))
        End If
        Return True
    End Function
End Class
