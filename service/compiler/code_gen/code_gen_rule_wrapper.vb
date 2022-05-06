
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.interpreter.primitive

Public Class code_gen_rule_wrapper(Of WRITER As New,
                                      _nlexer_rule As __do(Of String),
                                      _syntaxer_rule As __do(Of String),
                                      _prefixes As __do(Of vector(Of Action(Of statements(Of WRITER)))),
                                      _suffixes As __do(Of vector(Of Action(Of statements(Of WRITER)))),
                                      _code_gens As __do(Of vector(Of Action(Of code_gens(Of WRITER)))),
                                       SCOPE_T As {scope(Of SCOPE_T), New})
    Inherits rule_wrapper(Of _nlexer_rule, _syntaxer_rule)

    ' @VisibleForTesting
    Public Shared Function new_code_gens() As code_gens(Of WRITER)
        Dim l As New code_gens(Of WRITER)()
        Dim v As vector(Of Action(Of code_gens(Of WRITER))) = +alloc(Of _code_gens)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(l)
            i += uint32_1
        End While
        Return l
    End Function

    Public NotInheritable Class code_builder
        Private Shared ReadOnly code_gens As code_gens(Of WRITER) = new_code_gens()

        Public Shared Function build(ByVal input As String, ByVal o As WRITER) As Boolean
            assert(Not o Is Nothing)
            If input.null_or_whitespace() Then
                Return True
            End If
            Dim root As typed_node = Nothing
            If Not nlp().parse(input, root:=root) Then
                Return False
            End If
            assert(Not root Is Nothing)
            assert(root.type = typed_node.root_type)
            assert(root.type_name.Equals(typed_node.root_type_name))
            If root.leaf() Then
                Return False
            End If
            assert(root.child_count() > 0)
            Dim i As UInt32 = 0
            While i < root.child_count()
                If Not code_gens.of(root.child(i)).build(o) Then
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function

        Private Sub New()
        End Sub
    End Class

    ' TODO: builder and code_builder should be a singleton.
    Private NotInheritable Class builder
        Private Shared ReadOnly p As New statements(Of WRITER)()
        Private Shared ReadOnly s As New statements(Of WRITER)()

        Shared Sub New()
            init_prefixes()
            init_suffixes()
        End Sub

        Public Shared Function build(ByVal input As String, ByVal o As WRITER) As Boolean
            assert(Not o Is Nothing)
            ' Do not run the end_scope operations if currently it's in the root scope, the interpreter/primitive will
            ' be freed anyway.
            Using New SCOPE_T().without_end_scope()
                p.export(o)
                If Not code_builder.build(input, o) Then
                    Return False
                End If
                s.export(o)
            End Using
            Return True
        End Function

        Private Shared Sub init_statements(Of T As __do(Of vector(Of Action(Of statements(Of WRITER))))) _
                                          (ByVal p As statements(Of WRITER))
            Dim v As vector(Of Action(Of statements(Of WRITER))) = +alloc(Of T)()
            Dim i As UInt32 = 0
            While i < v.size()
                v(i)(p)
                i += uint32_1
            End While
        End Sub

        Private Shared Sub init_prefixes()
            init_statements(Of _prefixes)(p)
        End Sub

        Private Shared Sub init_suffixes()
            init_statements(Of _suffixes)(s)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Function parse(ByVal input As String, ByVal o As WRITER) As Boolean
        Using syntaxer.matching.disable_cycle_dependency_check_in_thread()
            Return builder.build(input, o)
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

        ' TODO: Move to scope?
        Public Shared Function current_file() As String
            Dim r As String = Nothing
            If instance_stack(Of String, parse_wrapper).back(r) Then
                Return r
            End If
            Return "unknown_file"
        End Function

        ' @VisibleForTesting
        Public Shared Function with_current_file(ByVal filename As String) As IDisposable
            assert(Not filename.empty_or_whitespace())
            instance_stack(Of String, parse_wrapper).current() = filename
            Return defer.to(Sub()
                                instance_stack(Of String, parse_wrapper).current() = Nothing
                            End Sub)
        End Function

        Public Shared Function with_current_file(ByVal filename As String,
                                                 ByVal parse As Func(Of String, Boolean)) As Boolean
            assert(Not filename.empty_or_whitespace())
            assert(Not parse Is Nothing)
            Dim s As String
            Try
                s = File.ReadAllText(filename)
            Catch ex As IOException
                raise_error(error_type.user, "Cannot read content from ", filename, ", ex ", ex)
                Return False
            End Try
            Using with_current_file(filename)
                Return parse(s)
            End Using
        End Function

        Public Function parse_file(ByVal filename As String, ByRef e As executor) As Boolean
            Dim o As executor = Nothing
            If Not with_current_file(filename, Function(ByVal s As String) As Boolean
                                                   Return parse(s, o)
                                               End Function) Then
                Return False
            End If
            e = o
            Return True
        End Function

        Public Function parse(ByVal input As String, ByVal e As exportable) As Boolean
            assert(Not e Is Nothing)
            Dim o As New WRITER()
            Return code_gen_rule_wrapper(Of WRITER,
                                            _nlexer_rule,
                                            _syntaxer_rule,
                                            _prefixes,
                                            _suffixes,
                                            _code_gens,
                                            SCOPE_T).parse(input, o) AndAlso
                   import(e, o)
        End Function

        Protected MustOverride Function import(ByVal e As exportable, ByVal o As WRITER) As Boolean
    End Class

    Protected Sub New()
    End Sub
End Class
