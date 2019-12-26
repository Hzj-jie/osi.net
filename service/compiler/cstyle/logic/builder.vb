
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Public Interface builder
    Function build(ByVal n As typed_node, ByVal o As writer) As Boolean
End Interface

Public MustInherit Class builder_wrapper
    Protected ReadOnly b As builders
    Protected ReadOnly lp As lang_parser

    Protected Sub New(ByVal b As builders, ByVal lp As lang_parser)
        assert(Not b Is Nothing)
        assert(Not lp Is Nothing)
        Me.b = b
        Me.lp = lp
    End Sub

    Protected Function type_name(ByVal n As typed_node) As String
        Dim r As String = Nothing
        assert(lp.type_name(n.type, r))
        Return r
    End Function
End Class

Public NotInheritable Class builders
    Private ReadOnly m As map(Of String, builder)
    Private ReadOnly lp As lang_parser

    Public Sub New(ByVal lp As lang_parser)
        assert(Not lp Is Nothing)
        Me.lp = lp
        m = New map(Of String, builder)()
    End Sub

    Public Sub register(ByVal s As String, ByVal b As builder)
        assert(Not s.null_or_whitespace())
        assert(Not b Is Nothing)
        m.emplace(s, b)
    End Sub

    Public Sub register(ByVal s As String, ByVal f As Func(Of builders, lang_parser, builder))
        assert(Not f Is Nothing)
        register(s, f(Me, lp))
    End Sub

    Public Function builder_of(ByVal n As typed_node) As builder
        assert(Not n Is Nothing)
        Dim type_name As String = Nothing
        assert(lp.type_name(n.type, type_name))
        Dim it As map(Of String, builder).iterator = Nothing
        it = m.find(type_name)
        assert(it <> m.end())
        Return (+it).second
    End Function

    Public NotInheritable Class builder_proxy
        Private ReadOnly b As builder
        Private ReadOnly n As typed_node

        Public Sub New(ByVal b As builder, ByVal n As typed_node)
            assert(Not b Is Nothing)
            assert(Not n Is Nothing)
            Me.b = b
            Me.n = n
        End Sub

        Public Function build(ByVal o As writer) As Boolean
            Return b.build(n, o)
        End Function
    End Class

    Public Function [of](ByVal n As typed_node) As builder_proxy
        Return New builder_proxy(builder_of(n), n)
    End Function
End Class
