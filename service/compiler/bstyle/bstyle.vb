
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
            Return New code_gens_registrar(Of writer)().
                           with(Of bool)().
                           with(Of condition)().
                           with(Of for_loop)().
                           with(Of ufloat)().
                           with(Of _function)().
                           with(Of function_call)().
                           with(Of ignore_result_function_call)().
                           with(Of _integer)().
                           with(Of biguint)().
                           with(Of logic)().
                           with(Of multi_sentence_paragraph)().
                           with(Of param)().
                           with(Of return_clause)().
                           with(Of _string)().
                           with(Of value)().
                           with(Of value_clause)().
                           with(value_declaration.instance).
                           with(Of heap_declaration)().
                           with(Of value_definition)().
                           with(Of heap_name)().
                           with(Of raw_variable_name)().
                           with(Of value_list)().
                           with(Of _while)().
                           with(include_with_string.instance).
                           with(include_with_file.instance).
                           with(Of ifndef_wrapped)().
                           with(define.instance).
                           with(Of typedef)().
                           with(typedef_type_name.instance).
                           with(typedef_type_str.instance).
                           with(Of struct)().
                           with(reinterpret_cast.instance).
                           with(Of _delegate)().
                           with_of_only_childs(
                               "base-root-type",
                               "root-type",
                               "for-increase",
                               "base-for-increase",
                               "paragraph",
                               "sentence",
                               "sentence-with-semi-colon",
                               "variable-name",
                               "value-without-bracket",
                               "base-value-without-bracket",
                               "include",
                               "typedef-type").
                           with(code_gen.of_ignore_last_child(Of writer)("root-type-with-semi-colon")).
                           with(code_gen.of_children(Of writer)("else-condition", 1)).
                           with(code_gen.of_first_child(Of writer)("param-with-comma")).
                           with(code_gen.of_children(Of writer)("value-with-bracket", 1)).
                           with(code_gen.of_first_child(Of writer)("value-with-comma")).
                           with(code_gen.of_all_children(Of writer)("paramlist")).
                           with(code_gen.of_ignore_last_child(Of writer)("base-sentence-with-semi-colon")).
                           with(code_gen.of_children_word_str(Of writer)("paramtype")).
                           with_of_all_childrens("paramtypelist").
                           with(code_gen.of_first_child(Of writer)("paramtype-with-comma"))
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
