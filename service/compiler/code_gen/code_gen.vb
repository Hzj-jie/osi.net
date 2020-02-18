﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.constructor

Public Interface code_gen(Of WRITER)
    Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean
End Interface

Public MustInherit Class code_gen_wrapper(Of WRITER)
    Protected ReadOnly l As code_gens(Of WRITER)

    Protected Sub New(ByVal l As code_gens(Of WRITER))
        assert(Not l Is Nothing)
        Me.l = l
    End Sub

    Protected Function code_gen_of(Of T As code_gen(Of WRITER))() As T
        Return l.code_gen_of(Of T)()
    End Function
End Class

Public Class code_gens(Of WRITER)
    Private ReadOnly m As map(Of String, code_gen(Of WRITER))

    Public Sub New()
        m = New map(Of String, code_gen(Of WRITER))()
    End Sub

    Public Shared Function code_gen_name(Of T As code_gen(Of WRITER))() As String
        Return GetType(T).Name().Replace("_"c, "-"c)
    End Function

    Public Sub register(ByVal s As String, ByVal b As code_gen(Of WRITER))
        assert(Not s.null_or_whitespace())
        assert(Not b Is Nothing)
        m.emplace(s, b)
    End Sub

    Public Sub register(ByVal s As String, ByVal f As Func(Of code_gens(Of WRITER), code_gen(Of WRITER)))
        assert(Not f Is Nothing)
        register(s, f(Me))
    End Sub

    Public Sub register(Of T As code_gen(Of WRITER))(ByVal b As T)
        register(code_gen_name(Of T)(), b)
    End Sub

    Public Sub register(Of T As code_gen(Of WRITER))()
        register(code_gen_name(Of T)(),
                 Function(ByVal b As code_gens(Of WRITER)) As code_gen(Of WRITER)
                     Return inject_constructor(Of T).invoke(b)
                 End Function)
    End Sub

    Public Function code_gen_of(ByVal name As String) As code_gen(Of WRITER)
        Dim it As map(Of String, code_gen(Of WRITER)).iterator = Nothing
        it = m.find(name)
        assert(it <> m.end(), "Cannot find code_gen of ", name)
        Return (+it).second
    End Function

    Public Function code_gen_of(ByVal n As typed_node) As code_gen(Of WRITER)
        assert(Not n Is Nothing)
        Return code_gen_of(n.type_name)
    End Function

    Public Function code_gen_of(Of T As code_gen(Of WRITER))() As T
        Return direct_cast(Of T)(code_gen_of(code_gen_name(Of T)()))
    End Function

    Public NotInheritable Class code_gen_proxy
        Private ReadOnly b As code_gen(Of WRITER)
        Private ReadOnly n As typed_node

        Public Sub New(ByVal b As code_gen(Of WRITER), ByVal n As typed_node)
            assert(Not b Is Nothing)
            assert(Not n Is Nothing)
            Me.b = b
            Me.n = n
        End Sub

        Public Function build(ByVal o As WRITER) As Boolean
            Return b.build(n, o)
        End Function
    End Class

    Public Function [of](ByVal n As typed_node) As code_gen_proxy
        Return New code_gen_proxy(code_gen_of(n), n)
    End Function
End Class
