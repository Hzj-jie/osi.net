
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.logic
Imports osi.service.resource
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic.writer)

Public NotInheritable Class bstyle
    Inherits logic_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, logic_gens_t, scope)

    Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "service/compiler/bstyle")

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "nlexer_rule.txt")

        Shared Sub New()
            bstyle_rules.nlexer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "syntaxer_rule.txt")

        Shared Sub New()
            bstyle_rules.syntaxer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class prefixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf code_types.register)
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf main.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of writer)))
            Return vector.emplace_of(Of Action(Of code_gens(Of writer)))(
                code_gen.of_only_child(Of writer)("base-root-type"),
                code_gen.of_only_child(Of writer)("root-type"),
                AddressOf bool.register,
                AddressOf condition.register,
                code_gen.of_children(Of writer)("else-condition", 1),
                AddressOf for_loop.register,
                code_gen.of_only_child(Of writer)("for-increase"),
                code_gen.of_only_child(Of writer)("base-for-increase"),
                AddressOf ufloat.register,
                AddressOf [function].register,
                AddressOf function_call.register,
                AddressOf ignore_result_function_call.register,
                AddressOf [integer].register,
                AddressOf biguint.register,
                AddressOf logic.register,
                code_gen.of_first_child(Of writer)("logic-with-semi-colon"),
                AddressOf multi_sentence_paragraph.register,
                code_gen.of_only_child(Of writer)("paragraph"),
                AddressOf param.register,
                code_gen.of_first_child(Of writer)("param-with-comma"),
                code_gen.of_all_children(Of writer)("paramlist"),
                AddressOf return_clause.register,
                code_gen.of_only_child(Of writer)("sentence"),
                code_gen.of_first_child(Of writer)("base-sentence-with-semi-colon"),
                code_gen.of_only_child(Of writer)("sentence-with-semi-colon"),
                code_gen.of_only_child(Of writer)("sentence-without-semi-colon"),
                AddressOf [string].register,
                AddressOf value.register,
                AddressOf value_clause.register,
                AddressOf value_declaration.register,
                AddressOf heap_declaration.register,
                AddressOf value_definition.register,
                code_gen.of_only_child(Of writer)("variable-name"),
                AddressOf heap_name.register,
                AddressOf raw_variable_name.register,
                code_gen.of_first_child(Of writer)("value-declaration-with-semi-colon"),
                code_gen.of_first_child(Of writer)("value-definition-with-semi-colon"),
                code_gen.of_first_child(Of writer)("heap-declaration-with-semi-colon"),
                AddressOf value_list.register,
                code_gen.of_children(Of writer)("value-with-bracket", 1),
                code_gen.of_first_child(Of writer)("value-with-comma"),
                code_gen.of_only_child(Of writer)("value-without-bracket"),
                AddressOf [while].register,
                code_gen.of_only_child(Of writer)("include"),
                AddressOf include_with_string.register,
                AddressOf include_with_file.register,
                AddressOf ifndef_wrapped.register,
                AddressOf define.register,
                AddressOf typedef.register,
                code_gen.of_only_child(Of writer)("typedef-type"),
                AddressOf typedef_type_name.register,
                AddressOf typedef_type_str.register,
                code_gen.of_first_child(Of writer)("typedef-with-semi-colon"),
                AddressOf struct.register,
                code_gen.of_first_child(Of writer)("reinterpret-cast-with-semi-colon"),
                AddressOf reinterpret_cast.register)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
