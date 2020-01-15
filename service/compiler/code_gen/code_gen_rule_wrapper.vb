
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Public Class code_gen_rule_wrapper(Of WRITER,
                                      PARAMETERS,
                                      CODE_GENS_IMPL As code_gens(Of WRITER),
                                      STATEMENTS_IMPL As statements(Of WRITER),
                                      _nlexer_rule As __do(Of Byte()),
                                      _syntaxer_rule As __do(Of Byte()),
                                      _prefixes As __do(Of vector(Of Action(Of STATEMENTS_IMPL, PARAMETERS))),
                                      _suffixes As __do(Of vector(Of Action(Of STATEMENTS_IMPL, PARAMETERS))),
                                      _code_gens As __do(Of vector(Of Action(Of CODE_GENS_IMPL, PARAMETERS))))
    Inherits rule_wrapper(Of _nlexer_rule, _syntaxer_rule)

    Private Shared ReadOnly w As PARAMETERS
    Private Shared ReadOnly l As CODE_GENS_IMPL
    Private Shared ReadOnly p As STATEMENTS_IMPL
    Private Shared ReadOnly s As STATEMENTS_IMPL

    Shared Sub New()
        w = alloc(Of PARAMETERS)()
        l = alloc(Of CODE_GENS_IMPL)()
        p = alloc(Of STATEMENTS_IMPL)()
        s = alloc(Of STATEMENTS_IMPL)()
        init_code_gens()
        init_prefixes()
        init_suffixes()
    End Sub

    ' Used by vector.of
    Protected Shared Function registerer(ByVal i As Action(Of STATEMENTS_IMPL, PARAMETERS)) _
                                  As Action(Of STATEMENTS_IMPL, PARAMETERS)
        assert(Not i Is Nothing)
        Return i
    End Function

    ' Used by vector.of
    Protected Shared Function registerer(ByVal i As Action(Of CODE_GENS_IMPL, PARAMETERS)) _
                                  As Action(Of CODE_GENS_IMPL, PARAMETERS)
        assert(Not i Is Nothing)
        Return i
    End Function

    Private Shared Function ignore_parameters(Of T)(ByVal i As Action(Of T)) As Action(Of T, PARAMETERS)
        assert(Not i Is Nothing)
        Return Sub(ByVal x As T, ByVal y As PARAMETERS)
                   i(x)
               End Sub
    End Function

    Protected Shared Function ignore_parameters(ByVal i As Action(Of STATEMENTS_IMPL)) _
                                  As Action(Of STATEMENTS_IMPL, PARAMETERS)
        Return ignore_parameters(Of STATEMENTS_IMPL)(i)
    End Function

    Protected Shared Function ignore_parameters(ByVal i As Action(Of CODE_GENS_IMPL)) _
                                  As Action(Of CODE_GENS_IMPL, PARAMETERS)
        Return ignore_parameters(Of CODE_GENS_IMPL)(i)
    End Function

    Public Shared Function build(ByVal root As typed_node, ByVal o As WRITER) As Boolean
        assert(Not root Is Nothing)
        assert(Not o Is Nothing)
        assert(root.type = typed_node.ROOT_TYPE)
        assert(strsame(root.type_name, typed_node.ROOT_TYPE_NAME))
        If root.leaf() Then
            Return False
        End If
        p.export(o)
        assert(root.child_count() > 0)
        For i As UInt32 = 0 To root.child_count() - uint32_1
            l.of(root.child(i)).build(o)
        Next
        s.export(o)
        Return True
    End Function

    Public Shared Function parse(ByVal input As String, ByVal o As WRITER) As Boolean
        assert(Not input.null_or_whitespace())
        assert(Not o Is Nothing)
        Dim r As typed_node = Nothing
        If Not nlp().parse(input, root:=r) Then
            Return False
        End If
        assert(Not r Is Nothing)
        Return build(r, o)
    End Function

    Public MustInherit Class parse_wrapper
        Protected ReadOnly functions As interrupts

        Public Sub New(ByVal functions As interrupts)
            assert(Not functions Is Nothing)
            Me.functions = functions
        End Sub

        Public Function parse(ByVal input As String, ByRef e As executor) As Boolean
            e = New simulator(functions)
            If parse(input, direct_cast(Of exportable)(e)) Then
                Return True
            End If
            e = Nothing
            Return False
        End Function

        Public Function parse(ByVal input As String, ByVal e As exportable) As Boolean
            assert(Not e Is Nothing)
            Dim o As WRITER = Nothing
            o = alloc(Of WRITER)()
            If Not code_gen_rule_wrapper(Of WRITER,
                                            PARAMETERS,
                                            CODE_GENS_IMPL,
                                            STATEMENTS_IMPL,
                                            _nlexer_rule,
                                            _syntaxer_rule,
                                            _prefixes,
                                            _suffixes,
                                            _code_gens).
                parse(input, o) Then
                Return False
            End If
            Return import(e, o)
        End Function

        Protected MustOverride Function import(ByVal e As exportable, ByVal o As WRITER) As Boolean
    End Class

    Private Shared Sub init_code_gens()
        Dim v As vector(Of Action(Of CODE_GENS_IMPL, PARAMETERS)) = Nothing
        v = +alloc(Of _code_gens)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(l, w)
            i += uint32_1
        End While
    End Sub

    Private Shared Sub init_statements(Of T As __do(Of vector(Of Action(Of STATEMENTS_IMPL, PARAMETERS)))) _
                                      (ByVal p As STATEMENTS_IMPL)
        Dim v As vector(Of Action(Of STATEMENTS_IMPL, PARAMETERS)) = Nothing
        v = +alloc(Of T)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(p, w)
            i += uint32_1
        End While
    End Sub

    Private Shared Sub init_prefixes()
        init_statements(Of _prefixes)(p)
    End Sub

    Private Shared Sub init_suffixes()
        init_statements(Of _suffixes)(s)
    End Sub

    Protected Sub New()
    End Sub
End Class
