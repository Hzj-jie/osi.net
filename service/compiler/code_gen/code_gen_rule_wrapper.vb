
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
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

    Public Structure code_gens_proxy
        Implements func_t(Of code_gens(Of WRITER))

        Public Function run() As code_gens(Of WRITER) Implements func_t(Of code_gens(Of WRITER)).run
            Return code_gens()
        End Function
    End Structure

    ' @VisibleForTesting
    Public Shared Function code_gens() As code_gens(Of WRITER)
        Return code_builder.code_gens
    End Function

    Protected Shared Function code_gen_of(ByVal n As typed_node) As code_gens(Of WRITER).code_gen_proxy
        Return code_gens().of(n)
    End Function

    Protected Shared Function my_node(Of T)(ByVal n As typed_node) As typed_node
        assert(Not n Is Nothing)
        Return n.ancestor_of(compiler.code_gens(Of WRITER).code_gen_name(Of T)())
    End Function

    Public Structure code_builder_proxy
        Implements func_t(Of String, WRITER, Boolean)

        Public Function run(ByVal i As String, ByVal j As WRITER) As Boolean _
                               Implements func_t(Of String, WRITER, Boolean).run
            Return code_builder.build(i, j)
        End Function
    End Structure

    Protected NotInheritable Class code_builder
        Public Shared ReadOnly code_gens As code_gens(Of WRITER) =
            Function() As code_gens(Of WRITER)
                Dim l As New code_gens(Of WRITER)()
                Dim v As vector(Of Action(Of code_gens(Of WRITER))) = +alloc(Of _code_gens)()
                Dim i As UInt32 = 0
                While i < v.size()
                    v(i)(l)
                    i += uint32_1
                End While
                Return l
            End Function()

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

    Protected Shared Function build(ByVal input As String, ByVal o As WRITER) As Boolean
        Using syntaxer.matching.disable_cycle_dependency_check_in_thread()
            Return builder.build(input, o)
        End Using
    End Function

    Public MustInherit Class compile_wrapper
        Protected ReadOnly functions As interrupts

        Public Sub New(ByVal functions As interrupts)
            assert(Not functions Is Nothing)
            Me.functions = functions
        End Sub

        Public Function compile(ByVal input As String, ByRef e As executor) As Boolean
            e = New simulator(functions)
            If compile(input, direct_cast(Of exportable)(e)) Then
                Return True
            End If
            e = Nothing
            Return False
        End Function

        ' TODO: Move to scope?
        Public Shared Function current_file() As String
            Dim r As String = Nothing
            If instance_stack(Of String, compile_wrapper).back(r) Then
                Return r
            End If
            Return "unknown_file"
        End Function

        ' @VisibleForTesting
        Public Shared Function with_current_file(ByVal filename As String) As IDisposable
            assert(Not filename.empty_or_whitespace())
            Return instance_stack(Of String, compile_wrapper).with(filename)
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

        Public Function compile_file(ByVal filename As String, ByRef e As executor) As Boolean
            Dim o As executor = Nothing
            If Not with_current_file(filename, Function(ByVal s As String) As Boolean
                                                   Return compile(s, o)
                                               End Function) Then
                Return False
            End If
            e = o
            Return True
        End Function

<<<<<<< HEAD
        Public Function generate(ByVal input As String, ByVal o As WRITER) As Boolean
            Return build(input, o)
        End Function

        Public Function compile(ByVal input As String, ByVal e As exportable) As Boolean
            assert(Not e Is Nothing)
            Dim o As New WRITER()
            Return generate(input, o) AndAlso import(e, o)
=======
        Public Function build(ByVal input As String, ByVal o As WRITER) As Boolean
            Return code_gen_rule_wrapper(Of WRITER,
                                            _nlexer_rule,
                                            _syntaxer_rule,
                                            _prefixes,
                                            _suffixes,
                                            _code_gens,
                                            SCOPE_T).build(input, o)
>>>>>>> master
        End Function

        Public Function compile(ByVal input As String, ByVal e As exportable) As Boolean
            assert(Not e Is Nothing)
            Dim o As New WRITER()
            Return build(input, o) AndAlso import(o, e)
        End Function

        Protected MustOverride Function import(ByVal o As WRITER, ByVal e As exportable) As Boolean
    End Class

    Protected Sub New()
    End Sub
End Class
