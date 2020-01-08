
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template

Public NotInheritable Class bstyle
    Inherits logic.logic_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, logic_gens_t)

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of Byte())

        Protected Overrides Function at() As Byte()
            Return bstyle_rules.nlexer_rule
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of Byte())

        Protected Overrides Function at() As Byte()
            Return bstyle_rules.syntaxer_rule
        End Function
    End Class

    Public NotInheritable Class prefixes_t
        Inherits __do(Of vector(Of Action(Of prefixes)))

        Protected Overrides Function at() As vector(Of Action(Of prefixes))
            Return vector.of(Of Action(Of prefixes))(
                AddressOf types.register,
                AddressOf constants.register,
                AddressOf temps.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of logic_gens)))

        Protected Overrides Function at() As vector(Of Action(Of logic_gens))
            Return vector.of(Of Action(Of logic_gens))(
                AddressOf condition.register,
                AddressOf else_condition.register,
                AddressOf for_loop.register,
                AddressOf [function].register,
                AddressOf function_call.register,
                AddressOf multi_sentence_paragraph.register,
                AddressOf name.register,
                AddressOf param.register,
                AddressOf param_with_comma.register,
                AddressOf paramlist.register,
                AddressOf semi_colon.register,
                AddressOf sentence.register,
                AddressOf sentence_with_semi_colon.register,
                AddressOf sentence_without_semi_colon.register,
                AddressOf value.register,
                AddressOf value_clause.register,
                AddressOf value_declaration.register,
                AddressOf value_definition.register,
                AddressOf value_list.register,
                AddressOf value_with_bracket.register,
                AddressOf value_with_comma.register,
                AddressOf value_without_bracket.register,
                AddressOf [while].register)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
