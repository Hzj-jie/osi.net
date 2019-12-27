
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class sentence_with_semi_colon
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Shared Sub register(ByVal b As builders)
        assert(Not b Is Nothing)
        b.register(Of sentence_with_semi_colon)()
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        If Not b.of(n.child()).build(o) Then
            o.err("@sentence-with-semi-colon ", n.child())
            Return False
        End If
        Return True
    End Function
End Class
