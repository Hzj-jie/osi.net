
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.resource
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic_writer)

Partial Public NotInheritable Class bstyle
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
            Return vector.emplace_of(Of Action(Of statements))(AddressOf main.register,
                                                               AddressOf scope.call_hierarchy_t.calculator.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of logic_writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of logic_writer)))
            Return New code_gens_registrar(Of logic_writer)().
                           with(Of bool)().
                           with(Of condition)().
                           with(Of for_loop)().
                           with(Of ufloat)().
                           with(Of _function(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of function_call(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of ignore_result_function_call(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of _integer)().
                           with(Of biguint)().
                           with(Of logic)().
                           with(Of multi_sentence_paragraph)().
                           with(Of param)().
                           with(Of return_clause(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of _string)().
                           with(Of value)().
                           with(Of value_clause(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of value_declaration(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of heap_declaration(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of value_definition(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of heap_name(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of raw_variable_name(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of value_list(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of _while)().
                           with(Of include_with_string)().
                           with(Of include_with_file)().
                           with(scope.define_t.code_gens.ifndef_wrapped(AddressOf code_gen_of)).
                           with(scope.define_t.code_gens.define()).
                           with(Of typedef)().
                           with(Of typedef_type_name)().
                           with(Of typedef_type_str)().
                           with(Of struct)().
                           with(Of reinterpret_cast)().
                           with(Of undefine)().
                           with(Of dealloc)().
                           with(Of static_cast(Of code_builder_proxy, code_gens_proxy, scope))().
                           with(Of _delegate)().
                           with(Of kw_file)().
                           with(Of kw_func)().
                           with(Of kw_line)().
                           with(Of kw_statement)().
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
                           with(code_gen.of_ignore_last_child(Of logic_writer)("root-type-with-semi-colon")).
                           with(code_gen.of_children(Of logic_writer)("else-condition", 1)).
                           with(code_gen.of_first_child(Of logic_writer)("param-with-comma")).
                           with(code_gen.of_children(Of logic_writer)("value-with-bracket", 1)).
                           with(code_gen.of_first_child(Of logic_writer)("value-with-comma")).
                           with(code_gen.of_all_children(Of logic_writer)("paramlist")).
                           with(code_gen.of_ignore_last_child(Of logic_writer)("base-sentence-with-semi-colon")).
                           with(code_gen.of_input_without_ignored(Of logic_writer)("paramtype")).
                           with_of_all_childrens("paramtypelist").
                           with(code_gen.of_first_child(Of logic_writer)("paramtype-with-comma"))
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
