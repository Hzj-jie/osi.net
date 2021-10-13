
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.interpreter.primitive
Imports osi.service.compiler.rewriters
Imports exportable = osi.service.compiler.logic.exportable

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public Interface rewriter
        Inherits code_gen(Of typed_node_writer)
    End Interface

    Public MustInherit Class rewriter_wrapper
        Inherits code_gen_wrapper(Of typed_node_writer)

        Protected Sub New(ByVal i As rewriters)
            MyBase.New(i)
        End Sub
    End Class
End Class

Public Class rewriter_rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                                      _syntaxer_rule As __do(Of Byte()),
                                      _prefixes As __do(Of vector(Of Action(Of statements, parameters))),
                                      _suffixes As __do(Of vector(Of Action(Of statements, parameters))),
                                      _rewriter_gens As __do(Of vector(Of Action(Of rewriters, parameters))))
    Inherits rewriter_rule_wrapper(Of parameters,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _rewriter_gens)

    Protected Sub New()
    End Sub
End Class

Public Class rewriter_rule_wrapper(Of PARAMETERS,
                                      _nlexer_rule As __do(Of Byte()),
                                      _syntaxer_rule As __do(Of Byte()),
                                      _prefixes As __do(Of vector(Of Action(Of statements, PARAMETERS))),
                                      _suffixes As __do(Of vector(Of Action(Of statements, PARAMETERS))),
                                      _rewriter_gens As __do(Of vector(Of Action(Of rewriters, PARAMETERS))))
    Inherits code_gen_rule_wrapper(Of typed_node_writer,
                                      PARAMETERS,
                                      rewriters,
                                      statements,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _rewriter_gens)

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
                                          PARAMETERS,
                                          rewriters,
                                          statements,
                                          _nlexer_rule,
                                          _syntaxer_rule,
                                          _prefixes,
                                          _suffixes,
                                          _rewriter_gens).parse_wrapper
        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function import(ByVal e As interpreter.primitive.exportable,
                                            ByVal o As typed_node_writer) As Boolean
            Dim l() As exportable = Nothing
            If Not logic_parse(o.dump(), l) Then
                Return False
            End If
            Return logic.import(e, l)
        End Function

        Protected MustOverride Function logic_parse(ByVal s As String, ByRef e() As exportable) As Boolean
    End Class

    Protected Shared Function bypass_registerer(ByVal node_name As String) As Action(Of rewriters, PARAMETERS)
        assert(Not node_name.null_or_whitespace())
        Return ignore_parameters(Sub(ByVal i As rewriters)
                                     bypass.register(i, node_name)
                                 End Sub)
    End Function

    Protected Shared Function leaf_registerer(ByVal node_name As String) As Action(Of rewriters, PARAMETERS)
        assert(Not node_name.null_or_whitespace())
        Return ignore_parameters(Sub(ByVal i As rewriters)
                                     leaf.register(i, node_name)
                                 End Sub)
    End Function

    Protected Sub New()
    End Sub
End Class
