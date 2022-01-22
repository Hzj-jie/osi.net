
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic.writer)

Public Class logic_rule_wrapper(Of _nlexer_rule As __do(Of String),
                                   _syntaxer_rule As __do(Of String),
                                   _prefixes As __do(Of vector(Of Action(Of statements))),
                                   _suffixes As __do(Of vector(Of Action(Of statements))),
                                   _logic_gens As __do(Of vector(Of Action(Of code_gens(Of writer)))),
                                    SCOPE_T As scope(Of SCOPE_T))
    Inherits code_gen_rule_wrapper(Of writer,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _logic_gens,
                                      SCOPE_T)

    Public Overloads Shared Function parse(ByVal input As String, ByRef o As String) As Boolean
        Dim w As New writer()
        If Not parse(input, w) Then
            Return False
        End If
        o = w.dump()
        Return True
    End Function

    Public Shared Function with_functions(ByVal functions As interrupts) As parse_wrapper
        Return New parse_wrapper(functions)
    End Function

    Public Shared Function with_default_functions() As parse_wrapper
        Return with_functions(interrupts.default)
    End Function

    Public NotInheritable Shadows Class parse_wrapper
        Inherits code_gen_rule_wrapper(Of writer,
                                          _nlexer_rule,
                                          _syntaxer_rule,
                                          _prefixes,
                                          _suffixes,
                                          _logic_gens,
                                          SCOPE_T).parse_wrapper
        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function import(ByVal e As exportable, ByVal o As writer) As Boolean
            Return New importer(functions).import(o.dump(), e)
        End Function
    End Class

    Protected Sub New()
    End Sub
End Class
