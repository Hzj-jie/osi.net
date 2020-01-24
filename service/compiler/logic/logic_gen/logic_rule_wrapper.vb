
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports exportable = osi.service.compiler.logic.exportable

Public Interface logic_gen
    Inherits code_gen(Of writer)
End Interface

Public MustInherit Class logic_gen_wrapper
    Inherits code_gen_wrapper(Of writer)

    Protected Sub New(ByVal l As logic_gens)
        MyBase.New(l)
    End Sub

    Protected Function logic_gen_of(Of T As logic_gen)() As T
        Return code_gen_of(Of T)()
    End Function
End Class

Public NotInheritable Class logic_gens
    Inherits code_gens(Of writer)
End Class

Namespace logic
    Public Interface statement
        Inherits statement(Of writer)
    End Interface

    Public NotInheritable Class statements
        Inherits statements(Of writer)
    End Class
End Namespace

Public NotInheritable Class logic_rule_wrapper
    Public ReadOnly type_alias As type_alias
    Public ReadOnly overload As bstyle.overload

    Public Sub New()
        type_alias = New type_alias()
        overload = New bstyle.overload()
    End Sub
End Class

Public Class logic_rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                                   _syntaxer_rule As __do(Of Byte()),
                                   _prefixes As __do(Of vector(Of Action(Of statements, logic_rule_wrapper))),
                                   _suffixes As __do(Of vector(Of Action(Of statements, logic_rule_wrapper))),
                                   _logic_gens As __do(Of vector(Of Action(Of logic_gens, logic_rule_wrapper))))
    Inherits code_gen_rule_wrapper(Of writer,
                                      logic_rule_wrapper,
                                      logic_gens,
                                      statements,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _logic_gens)

    Public Overloads Shared Function parse(ByVal input As String, ByRef o As String) As Boolean
        Dim w As writer = Nothing
        w = New writer()
        If Not parse(input, w) Then
            Return False
        End If
        o = w.dump()
        Return True
    End Function

    Public Shared Function with_functions(ByVal functions As interrupts) As parse_wrapper
        Return New parse_wrapper(functions)
    End Function

    Public NotInheritable Shadows Class parse_wrapper
        Inherits code_gen_rule_wrapper(Of writer,
                                      logic_rule_wrapper,
                                      logic_gens,
                                      statements,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _logic_gens).parse_wrapper
        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function import(ByVal e As interpreter.primitive.exportable, ByVal o As writer) As Boolean
            Dim es As vector(Of exportable) = Nothing
            es = New vector(Of exportable)()
            If Not o.dump(functions, es) Then
                Return False
            End If
            Return e.import(+es)
        End Function
    End Class

    Protected Sub New()
    End Sub
End Class
