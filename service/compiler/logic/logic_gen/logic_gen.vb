
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Public Interface logic_gen
    Function build(ByVal n As typed_node, ByVal o As writer) As Boolean
End Interface

Public MustInherit Class logic_gen_wrapper
    Protected ReadOnly l As logic_gens

    Protected Sub New(ByVal l As logic_gens)
        assert(Not l Is Nothing)
        Me.l = l
    End Sub

    Protected Function logic_gen_of(Of T As logic_gen)() As T
        Return direct_cast(Of T)(l.logic_gen_of(logic_gens.logic_gen_name(Of T)()))
    End Function
End Class

Public NotInheritable Class logic_gens
    Private ReadOnly m As map(Of String, logic_gen)

    Public Sub New()
        m = New map(Of String, logic_gen)()
    End Sub

    Public Shared Function logic_gen_name(Of T As logic_gen)() As String
        Return GetType(T).Name().Replace("_"c, "-"c)
    End Function

    Public Sub register(ByVal s As String, ByVal b As logic_gen)
        assert(Not s.null_or_whitespace())
        assert(Not b Is Nothing)
        m.emplace(s, b)
    End Sub

    Public Sub register(ByVal s As String, ByVal f As Func(Of logic_gens, logic_gen))
        assert(Not f Is Nothing)
        register(s, f(Me))
    End Sub

    Public Sub register(Of T As logic_gen)()
        register(logic_gen_name(Of T)(),
                 Function(ByVal b As logic_gens) As logic_gen
                     Return inject_constructor(Of T).invoke(b)
                 End Function)
    End Sub

    Public Function logic_gen_of(ByVal name As String) As logic_gen
        Dim it As map(Of String, logic_gen).iterator = Nothing
        it = m.find(name)
        assert(it <> m.end(), "Cannot find logic_gen of ", name)
        Return (+it).second
    End Function

    Public Function logic_gen_of(ByVal n As typed_node) As logic_gen
        assert(Not n Is Nothing)
        Return logic_gen_of(n.type_name)
    End Function

    Public NotInheritable Class logic_gen_proxy
        Private ReadOnly b As logic_gen
        Private ReadOnly n As typed_node

        Public Sub New(ByVal b As logic_gen, ByVal n As typed_node)
            assert(Not b Is Nothing)
            assert(Not n Is Nothing)
            Me.b = b
            Me.n = n
        End Sub

        Public Function build(ByVal o As writer) As Boolean
            Return b.build(n, o)
        End Function
    End Class

    Public Function [of](ByVal n As typed_node) As logic_gen_proxy
        Return New logic_gen_proxy(logic_gen_of(n), n)
    End Function
End Class
