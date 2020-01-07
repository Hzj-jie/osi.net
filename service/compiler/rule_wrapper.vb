
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.resource

Public Class rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                             _syntaxer_rule As __do(Of Byte()))
    Inherits rule_wrapper(Of _nlexer_rule, _syntaxer_rule, empty_prefixes, empty_logic_gens)

    Public NotInheritable Class empty_prefixes
        Inherits __do(Of vector(Of Action(Of prefixes)))

        Protected Overrides Function at() As vector(Of Action(Of prefixes))
            Return vector.of(Of Action(Of prefixes))()
        End Function
    End Class

    Public NotInheritable Class empty_logic_gens
        Inherits __do(Of vector(Of Action(Of logic_gens)))

        Protected Overrides Function at() As vector(Of Action(Of logic_gens))
            Return vector.of(Of Action(Of logic_gens))()
        End Function
    End Class
End Class

Public Class rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                             _syntaxer_rule As __do(Of Byte()),
                             _prefixes As __do(Of vector(Of Action(Of prefixes))),
                             _logic_gens As __do(Of vector(Of Action(Of logic_gens))))
    Public Shared ReadOnly nlexer_rule As String
    Public Shared ReadOnly syntaxer_rule As String
    Private Shared ReadOnly l As logic_gens
    Private Shared ReadOnly p As prefixes

    Public Shared Function nlp() As nlp
        Return nlp_holder.nlp
    End Function

    Shared Sub New()
        nlexer_rule = (+alloc(Of _nlexer_rule)()).as_text()
        syntaxer_rule = (+alloc(Of _syntaxer_rule)()).as_text()
        l = New logic_gens()
        p = New prefixes()
        init_prefixes()
        init_logic_gens()
    End Sub

    Private Shared Sub init_prefixes()
        Dim v As vector(Of Action(Of prefixes)) = Nothing
        v = +alloc(Of _prefixes)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(p)
            i += uint32_1
        End While
    End Sub

    Private Shared Sub init_logic_gens()
        Dim v As vector(Of Action(Of logic_gens)) = Nothing
        v = +alloc(Of _logic_gens)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(l)
            i += uint32_1
        End While
    End Sub

    Private NotInheritable Class nlp_holder
        Public Shared ReadOnly nlp As nlp

        Shared Sub New()
            assert(nlp.of(nlexer_rule, syntaxer_rule, nlp))
        End Sub
    End Class

    Protected Sub New()
    End Sub
End Class
