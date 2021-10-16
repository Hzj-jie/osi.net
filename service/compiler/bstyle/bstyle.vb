
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.logic

Public NotInheritable Class bstyle
    Inherits logic_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, logic_gens_t, scope)

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
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.of(Of Action(Of statements))(AddressOf code_types.register)
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.of(Of Action(Of statements))(AddressOf main.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of logic_gens)))

        Protected Overrides Function at() As vector(Of Action(Of logic_gens))
            Return vector.of(Of Action(Of logic_gens))(
                AddressOf root_type.register,
                AddressOf bool.register,
                AddressOf condition.register,
                AddressOf else_condition.register,
                AddressOf for_loop.register,
                AddressOf ufloat.register,
                AddressOf [function].register,
                AddressOf function_call.register,
                AddressOf ignore_result_function_call.register,
                AddressOf [integer].register,
                AddressOf biguint.register,
                AddressOf logic.register,
                AddressOf logic_with_semi_colon.register,
                AddressOf multi_sentence_paragraph.register,
                AddressOf variable_name.register,
                AddressOf paragraph.register,
                AddressOf param.register,
                AddressOf param_with_comma.register,
                AddressOf paramlist.register,
                AddressOf return_clause.register,
                AddressOf sentence.register,
                AddressOf sentence_with_semi_colon.register,
                AddressOf sentence_without_semi_colon.register,
                AddressOf [string].register,
                AddressOf value.register,
                AddressOf value_clause.register,
                AddressOf value_declaration.register,
                AddressOf value_definition.register,
                AddressOf value_declaration_with_semi_colon.register,
                AddressOf value_definition_with_semi_colon.register,
                AddressOf value_list.register,
                AddressOf value_with_bracket.register,
                AddressOf value_with_comma.register,
                AddressOf value_without_bracket.register,
                AddressOf variable_name.register,
                AddressOf [while].register,
                AddressOf include.register,
                AddressOf include_with_string.register,
                AddressOf include_with_file.register,
                AddressOf ifndef_wrapped.register,
                AddressOf define.register,
                AddressOf typedef.register,
                AddressOf typedef_with_semi_colon.register,
                AddressOf struct.register)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
