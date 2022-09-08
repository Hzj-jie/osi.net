
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
                           with(Of bstyle.bool)().
                           with(Of bstyle.condition)().
                           with(Of bstyle.for_loop)().
                           with(Of bstyle.ufloat)().
                           with(Of bstyle._function)().
                           with(Of bstyle.function_call)().
                           with(Of bstyle.ignore_result_function_call)().
                           with(Of bstyle._integer)().
                           with(Of bstyle.biguint)().
                           with(Of bstyle.logic)().
                           with(Of bstyle.multi_sentence_paragraph)().
                           with(Of bstyle.param)().
                           with(Of bstyle.return_clause)().
                           with(Of bstyle._string)().
                           with(Of bstyle.value)().
                           with(Of bstyle.value_clause)().
                           with(Of bstyle.value_declaration)().
                           with(Of bstyle.heap_declaration)().
                           with(Of bstyle.value_definition)().
                           with(Of bstyle.heap_name)().
                           with(Of bstyle.raw_variable_name)().
                           with(Of bstyle.value_list)().
                           with(Of bstyle._while)().
                           with(Of include_with_string)().
                           with(Of include_with_file)().
                           with(scope.define_t.code_gens.ifndef_wrapped(AddressOf code_gen_of,
                                                                        Function() As scope.define_t
                                                                            Return scope.current().defines()
                                                                        End Function)).
                           with(scope.define_t.code_gens.define(Function() As scope.define_t
                                                                    Return scope.current().defines()
                                                                End Function)).
                           with(Of bstyle.typedef)().
                           with(Of bstyle.typedef_type_name)().
                           with(Of bstyle.typedef_type_str)().
                           with(Of bstyle.struct)().
                           with(Of bstyle.reinterpret_cast)().
                           with(Of bstyle.undefine)().
                           with(Of bstyle.dealloc)().
                           with(Of bstyle.static_cast)().
                           with(Of bstyle._delegate)().
                           with(Of bstyle.kw_file)().
                           with(Of bstyle.kw_func)().
                           with(Of bstyle.kw_line)().
                           with(Of bstyle.kw_statement)().
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
End Class