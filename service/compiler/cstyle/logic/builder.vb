
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata

Public Interface builder
    Function build(ByVal o As StringBuilder) As Boolean
End Interface

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

    Public Function [of](ByVal n As typed_node) As builder
        assert(Not lp Is Nothing)
        assert(Not n Is Nothing)
        Dim type_name As String = Nothing
        assert(lp.type_name(n.type, type_name))
        Dim it As map(Of String, builder).iterator = Nothing
        it = m.find(type_name)
        assert(it <> m.end())
        Return (+it).second
    End Function
End Class
