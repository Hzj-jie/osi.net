
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
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

Public MustInherit Class logic_gen_wrapper_with_parameters
    Inherits logic_gen_wrapper

    Protected ReadOnly ta As type_alias

    Protected Sub New(ByVal l As logic_gens, ByVal p As parameters_t)
        MyBase.New(l)
        assert(Not p Is Nothing)
        ta = p.type_alias
        assert(Not ta Is Nothing)
    End Sub
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

    Public Class parameters_t
        Public ReadOnly type_alias As New type_alias()
    End Class
End Namespace

Public Class logic_rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                                   _syntaxer_rule As __do(Of Byte()),
                                   _prefixes As __do(Of vector(Of Action(Of statements, logic.parameters_t))),
                                   _suffixes As __do(Of vector(Of Action(Of statements, logic.parameters_t))),
                                   _logic_gens As __do(Of vector(Of Action(Of logic_gens, logic.parameters_t))))
    Inherits logic_rule_wrapper(Of logic.parameters_t,
                                   _nlexer_rule,
                                   _syntaxer_rule,
                                   _prefixes,
                                   _suffixes,
                                   _logic_gens)

    Protected Sub New()
    End Sub
End Class

Public Class logic_rule_wrapper(Of _PARAMETERS_T As logic.parameters_t,
                                   _nlexer_rule As __do(Of Byte()),
                                   _syntaxer_rule As __do(Of Byte()),
                                   _prefixes As __do(Of vector(Of Action(Of statements, _PARAMETERS_T))),
                                   _suffixes As __do(Of vector(Of Action(Of statements, _PARAMETERS_T))),
                                   _logic_gens As __do(Of vector(Of Action(Of logic_gens, _PARAMETERS_T))))
    Inherits code_gen_rule_wrapper(Of writer,
                                      _PARAMETERS_T,
                                      logic_gens,
                                      statements,
                                      _nlexer_rule,
                                      _syntaxer_rule,
                                      _prefixes,
                                      _suffixes,
                                      _logic_gens)

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

    Public NotInheritable Shadows Class parse_wrapper
        Inherits code_gen_rule_wrapper(Of writer,
                                          _PARAMETERS_T,
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
            Dim es As New vector(Of exportable)()
            If Not o.dump(functions, es) Then
                Return False
            End If
            Return e.import(+es)
        End Function
    End Class

    Protected Sub New()
    End Sub
End Class
