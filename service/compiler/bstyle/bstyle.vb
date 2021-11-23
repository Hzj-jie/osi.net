
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports [default] = osi.service.compiler.code_gens(Of osi.service.compiler.logic.writer).[default]
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic.writer)

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
        Inherits __do(Of vector(Of Action(Of code_gens(Of writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of writer)))
            Return vector.of(Of Action(Of code_gens(Of writer)))(
                [default].of_only_child("root-type"),
                AddressOf bool.register,
                AddressOf condition.register,
                [default].of("else-condition", 1),
                AddressOf for_loop.register,
                AddressOf ufloat.register,
                AddressOf [function].register,
                AddressOf function_call.register,
                AddressOf ignore_result_function_call.register,
                AddressOf [integer].register,
                AddressOf biguint.register,
                AddressOf logic.register,
                [default].of_first_child("logic-with-semi-colon"),
                AddressOf multi_sentence_paragraph.register,
                [default].of_only_child_with_wrapper(AddressOf scope.wrapper, "paragraph"),
                AddressOf param.register,
                [default].of_first_child("param-with-comma"),
                AddressOf paramlist.register,
                AddressOf return_clause.register,
                [default].of_only_child("sentence"),
                [default].of_first_child("sentence-with-semi-colon"),
                [default].of_only_child("sentence-without-semi-colon"),
                AddressOf [string].register,
                AddressOf value.register,
                AddressOf value_clause.register,
                AddressOf value_declaration.register,
                AddressOf heap_declaration.register,
                AddressOf value_definition.register,
                AddressOf heap_name.register,
                [default].of_first_child("value-declaration-with-semi-colon"),
                [default].of_first_child("value-definition-with-semi-colon"),
                [default].of_first_child("heap-declaration-with-semi-colon"),
                AddressOf value_list.register,
                [default].of("value-with-bracket", 1),
                [default].of_first_child("value-with-comma"),
                [default].of_only_child("value-without-bracket"),
                AddressOf variable_name.register,
                AddressOf [while].register,
                [default].of_only_child("include"),
                AddressOf include_with_string.register,
                AddressOf include_with_file.register,
                AddressOf ifndef_wrapped.register,
                AddressOf define.register,
                AddressOf typedef.register,
                [default].of_first_child("typedef-with-semi-colon"),
                AddressOf struct.register)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
