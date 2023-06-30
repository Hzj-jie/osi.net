
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.resource
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic_writer)

Partial Public NotInheritable Class b3style
    Inherits logic_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, logic_gens_t, scope)

    Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "service/compiler/b3style")

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "nlexer_rule.txt")

        Shared Sub New()
            static_constructor(Of b2style.nlexer_rule_t).execute()
            b3style_rules.nlexer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "syntaxer_rule.txt")

        Shared Sub New()
            static_constructor(Of b2style.syntaxer_rule_t).execute()
            b3style_rules.syntaxer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class prefixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf bstyle.code_types.register)
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf bstyle.main.register,
                                                               AddressOf scope.call_hierarchy_t.calculator.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of logic_writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of logic_writer)))
            Return New code_gens_registrar(Of logic_writer)().
                with_of_only_childs(
                    "root-type",
                    "base-root-type",
                    "include",
                    "typedef-type",
                    "paragraph",
                    "sentence",
                    "sentence-with-semi-colon",
                    "value-without-bracket",
                    "base-value-without-bracket",
                    "variable-name",
                    "for-increase",
                    "base-for-increase"
                ).
                with(code_gen.of_ignore_last_child(Of logic_writer)("root-type-with-semi-colon")).
                with(Of include_with_file)().
                with(Of include_with_string)().
                with(Of biguint)().
                with(Of bool)().
                with(Of _integer)().
                with(Of _string)().
                with(Of ufloat)().
                with(Of bstyle.logic)().
                with(scope.define_t.code_gens.ifndef_wrapped(AddressOf code_gen_of)).
                with(scope.define_t.code_gens.define()).
                with(Of typedef)().
                with(Of bstyle.typedef_type_name)().
                with(Of bstyle.typedef_type_str)().
                with(Of heap_declaration)().
                with(Of heap_name)().
                with(Of struct)().
                with(Of value_declaration)().
                with(Of value_definition)().
                with(Of value_clause)().
                with(Of function_call)().
                with(Of return_clause)().
                with(Of static_cast)().
                with(Of ignore_result_function_call)().
                with(Of value_list)().
                without(Of raw_variable_name)().
                with(Of kw_file)().
                with(Of kw_func)().
                with(Of kw_line)().
                with(Of kw_statement)().
                with(Of _function)().
                with(Of param)().
                with(code_gen.of_all_children(Of logic_writer)("paramlist")).
                with(Of multi_sentence_paragraph)().
                with(code_gen.of_ignore_last_child(Of logic_writer)("base-sentence-with-semi-colon")).
                with(Of value)().
                with_of_all_childrens("raw-value").
                with(Of name)().
                with(code_gen.of_first_child(Of logic_writer)("param-with-comma")).
                with(code_gen.of_first_child(Of logic_writer)("value-with-comma")).
                with(Of condition)().
                with(code_gen.of_children(Of logic_writer)("else-condition", 1)).
                with(Of for_loop)().
                with(Of _delegate)().
                with(Of reinterpret_cast)().
                with(code_gen.of_ignore_last_child(Of logic_writer)("b2style-sentence-with-semi-colon")).
                with(Of dealloc)().
                with(Of undefine)().
                with(code_gen.of_children(Of logic_writer)("value-with-bracket", 1)).
                with(code_gen.of_input_without_ignored(Of logic_writer)("paramtype")).
                with_of_all_childrens("paramtypelist").
                with(code_gen.of_first_child(Of logic_writer)("paramtype-with-comma"))
        End Function
    End Class
End Class