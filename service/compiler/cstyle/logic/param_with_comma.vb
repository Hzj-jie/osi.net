
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class param_with_comma
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        Return b.of(n.child(0)).build(o)
    End Function
End Class
