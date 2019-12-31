
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class value
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Shared Sub register(ByVal b As builders)
        assert(Not b Is Nothing)
        b.register(Of [function])()
    End Sub

    Private Shared Function current_target() As String
        Dim c As annotated_ref(Of value, String) = Nothing
        c = instance_stack(Of annotated_ref(Of value, String)).current()
        instance_stack(Of annotated_ref(Of value, String)).pop()
        Return c.v
    End Function

    Public Shared Sub current_target(ByVal v As String)
        assert(Not v.null_or_whitespace())
        instance_stack(Of annotated_ref(Of value, String)).push(annotated_ref(Of value).with(v))
    End Sub

    Public Sub current_target(ByVal n As typed_node)
        assert(Not n Is Nothing)
        assert(strsame(type_name(n), "name"))
        current_target(n.word().str())
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        If Not b.of(n.child()).build(o) Then
            o.err("@value ", n.child())
            Return False
        End If
        Return True
    End Function
End Class
