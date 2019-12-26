
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata
Imports osi.root.connector

Public NotInheritable Class name
    Inherits builder_wrapper
    Implements builder

    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        o.append(n.word().str())
        Return True
    End Function
End Class
