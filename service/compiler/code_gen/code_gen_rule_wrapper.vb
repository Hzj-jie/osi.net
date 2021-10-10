
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

    Protected Shared Function ignore_parameters(ByVal i As Action) As Action(Of PARAMETERS)
        assert(Not i Is Nothing)
        Return Sub(ByVal x As PARAMETERS)
                   i()
               End Sub
    End Function

    Public NotInheritable Class code_builder
        Private ReadOnly l As CODE_GENS_IMPL
        Private nested As UInt32

        Public Sub New(ByVal w As PARAMETERS)
            l = alloc(Of CODE_GENS_IMPL)()
            init_code_gens(w)
        End Sub

        Public Function nested_build_level() As UInt32
            Return nested
        End Function

        Public Function build(ByVal input As String, ByVal o As WRITER) As Boolean
            nested += uint32_1
            Using defer.to(Sub()
                               nested -= uint32_1
                           End Sub)
                assert(Not input.null_or_whitespace())
                assert(Not o Is Nothing)
                Dim root As typed_node = Nothing
                If Not nlp().parse(input, root:=root) Then
                    Return False
                End If
                assert(Not root Is Nothing)
                assert(root.type = typed_node.ROOT_TYPE)
                assert(strsame(root.type_name, typed_node.ROOT_TYPE_NAME))
                If root.leaf() Then
                    Return False
                End If
                assert(root.child_count() > 0)
                Dim i As UInt32 = 0
                While i < root.child_count()
                    If Not l.of(root.child(i)).build(o) Then
                        Return False
                    End If
                    i += uint32_1
                End While
                Return True
            End Using
        End Function

        Private Sub init_code_gens(ByVal w As PARAMETERS)
            Dim v As vector(Of Action(Of CODE_GENS_IMPL, PARAMETERS)) = +alloc(Of _code_gens)()
            Dim i As UInt32 = 0
            While i < v.size()
                v(i)(l, w)
                i += uint32_1
            End While
        End Sub

        Public Shared Function current() As code_builder
            Return thread_static_implementation_of(Of builder).resolve().cb
        End Function
    End Class

    Private NotInheritable Class builder
        Private ReadOnly w As PARAMETERS
        Public ReadOnly cb As code_builder
        Private ReadOnly p As STATEMENTS_IMPL
        Private ReadOnly s As STATEMENTS_IMPL

        Public Sub New()
            w = alloc(Of PARAMETERS)()
            cb = New code_builder(w)
            p = alloc(Of STATEMENTS_IMPL)()
            s = alloc(Of STATEMENTS_IMPL)()
            init_prefixes()
            init_suffixes()
        End Sub

        Public Function build(ByVal input As String, ByVal o As WRITER) As Boolean
            assert(Not o Is Nothing)
            p.export(o)
            If Not cb.build(input, o) Then
                Return False
            End If
            s.export(o)
            Return True
        End Function

        Private Sub init_statements(Of T As __do(Of vector(Of Action(Of STATEMENTS_IMPL, PARAMETERS)))) _
                                   (ByVal p As STATEMENTS_IMPL)
            Dim v As vector(Of Action(Of STATEMENTS_IMPL, PARAMETERS)) = +alloc(Of T)()
            Dim i As UInt32 = 0
            While i < v.size()
                v(i)(p, w)
                i += uint32_1
            End While
        End Sub

        Private Sub init_prefixes()
            init_statements(Of _prefixes)(p)
        End Sub

        Private Sub init_suffixes()
            init_statements(Of _suffixes)(s)
        End Sub
    End Class

    Public Shared Function parse(ByVal input As String, ByVal o As WRITER) As Boolean
        Using thread_static_implementation_of(Of builder).scoped_register(New builder())
            Return thread_static_implementation_of(Of builder).resolve().build(input, o)
        End Using
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
            Dim o As WRITER = alloc(Of WRITER)()
            Return code_gen_rule_wrapper(Of WRITER,
                                        PARAMETERS,
                                        CODE_GENS_IMPL,
                                        STATEMENTS_IMPL,
                                        _nlexer_rule,
                                        _syntaxer_rule,
                                        _prefixes,
                                        _suffixes,
                                        _code_gens).parse(input, o) AndAlso
                   import(e, o)
        End Function

        Protected MustOverride Function import(ByVal e As exportable, ByVal o As WRITER) As Boolean
    End Class

    Protected Sub New()
    End Sub
End Class
