
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.logic

Public NotInheritable Class bstyle
    Inherits logic_rule_wrapper(Of parameters_t, nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, logic_gens_t)

    Public NotInheritable Class parameters_t
        Inherits compiler.logic.parameters_t

        Public ReadOnly defines As New unordered_set(Of String)()
    End Class

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
        Inherits __do(Of vector(Of Action(Of statements, parameters_t)))

        Protected Overrides Function at() As vector(Of Action(Of statements, parameters_t))
            Return vector.of(registerer(AddressOf code_types.register))
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements, parameters_t)))

        Protected Overrides Function at() As vector(Of Action(Of statements, parameters_t))
            Return vector.of(
                       ignore_parameters(AddressOf main.register))
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of logic_gens, parameters_t)))

        Protected Overrides Function at() As vector(Of Action(Of logic_gens, parameters_t))
            Return vector.of(
                ignore_parameters(AddressOf root_type.register),
                ignore_parameters(AddressOf bool.register),
                ignore_parameters(AddressOf condition.register),
                ignore_parameters(AddressOf else_condition.register),
                ignore_parameters(AddressOf for_loop.register),
                ignore_parameters(AddressOf ufloat.register),
                registerer(AddressOf [function].register),
                ignore_parameters(AddressOf function_call.register),
                registerer(AddressOf ignore_result_function_call.register),
                ignore_parameters(AddressOf [integer].register),
                ignore_parameters(AddressOf biguint.register),
                ignore_parameters(AddressOf logic.register),
                ignore_parameters(AddressOf logic_with_semi_colon.register),
                ignore_parameters(AddressOf multi_sentence_paragraph.register),
                ignore_parameters(AddressOf variable_name.register),
                ignore_parameters(AddressOf paragraph.register),
                ignore_parameters(AddressOf param.register),
                ignore_parameters(AddressOf param_with_comma.register),
                ignore_parameters(AddressOf paramlist.register),
                ignore_parameters(AddressOf return_clause.register),
                ignore_parameters(AddressOf sentence.register),
                ignore_parameters(AddressOf sentence_with_semi_colon.register),
                ignore_parameters(AddressOf sentence_without_semi_colon.register),
                ignore_parameters(AddressOf [string].register),
                registerer(AddressOf value.register),
                ignore_parameters(AddressOf value_clause.register),
                registerer(AddressOf value_declaration.register),
                registerer(AddressOf value_definition.register),
                ignore_parameters(AddressOf value_definition_with_semi_colon.register),
                ignore_parameters(AddressOf value_list.register),
                ignore_parameters(AddressOf value_with_bracket.register),
                ignore_parameters(AddressOf value_with_comma.register),
                ignore_parameters(AddressOf value_without_bracket.register),
                ignore_parameters(AddressOf variable_name.register),
                ignore_parameters(AddressOf [while].register),
                ignore_parameters(AddressOf include.register),
                ignore_parameters(AddressOf include_with_string.register),
                ignore_parameters(AddressOf include_with_file.register),
                registerer(AddressOf ifndef_wrapped.register),
                registerer(AddressOf define.register),
                registerer(AddressOf typedef.register),
                ignore_parameters(AddressOf typedef_with_semi_colon.register))
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
