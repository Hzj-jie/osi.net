
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.rewriters
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.rewriters.typed_node_writer)

Public Class rewriter_rule_wrapper(Of _nlexer_rule As __do(Of String),
                                      _syntaxer_rule As __do(Of String),
                                      _prefixes As __do(Of vector(Of Action(Of statements))),
                                      _suffixes As __do(Of vector(Of Action(Of statements))),
                                      _rewriter_gens As __do(Of vector(Of Action(Of code_gens(Of typed_node_writer)))),
                                       SCOPE_T As {scope(Of SCOPE_T), New})
    Inherits code_gen_rule_wrapper(Of typed_node_writer,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _rewriter_gens,
                                      SCOPE_T)

    Public Overloads Shared Function parse(ByVal input As String, ByRef o As String) As Boolean
        Dim w As New typed_node_writer()
        If Not parse(input, w) Then
            Return False
        End If
        o = w.dump()
        Return True
    End Function

    Public MustInherit Shadows Class parse_wrapper
        Inherits code_gen_rule_wrapper(Of typed_node_writer,
                                          _nlexer_rule,
                                          _syntaxer_rule,
                                          _prefixes,
                                          _suffixes,
                                          _rewriter_gens,
                                          SCOPE_T).parse_wrapper
        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected NotOverridable Overrides Function import(ByVal e As exportable,
                                                           ByVal o As typed_node_writer) As Boolean
            assert(Not o Is Nothing)
            Return text_import(o.dump(), e)
        End Function

        Protected MustOverride Function text_import(ByVal i As String, ByVal e As exportable) As Boolean
    End Class

    Protected Sub New()
    End Sub
End Class
