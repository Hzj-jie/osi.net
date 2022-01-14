
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.compiler.rewriters
Imports osi.service.interpreter.primitive
Imports osi.service.resource
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.rewriters.typed_node_writer)

Partial Public NotInheritable Class b2style
    Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                      syntaxer_rule_t,
                                      __do.default_of(Of vector(Of Action(Of statements))),
                                      __do.default_of(Of vector(Of Action(Of statements))),
                                      rewriter_gens_t,
                                      scope)

    Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "service/compiler/b2style")

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "nlexer_rule.txt")

        Shared Sub New()
            b2style_rules.nlexer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "syntaxer_rule.txt")

        Shared Sub New()
            b2style_rules.syntaxer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class rewriter_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of typed_node_writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of typed_node_writer)))
            Return code_gens(Of typed_node_writer).registrars_of(
                       code_gen.of_only_child(Of typed_node_writer)("base-root-type"),
                       code_gen.of_all_children(Of typed_node_writer)("root-type-with-semi-colon"),
                       code_gen.of_only_child(Of typed_node_writer)("root-type"),
 _
                       code_gen.of_leaf_nodes("kw-if",
                                              "kw-else",
                                              "kw-for",
                                              "kw-while",
                                              "kw-do",
                                              "kw-loop",
                                              "kw-return",
                                              "kw-break",
                                              "kw-logic",
                                              "kw-reinterpret-cast",
                                              "start-square-bracket",
                                              "end-square-bracket",
                                              "bool",
                                              "integer",
                                              "biguint",
                                              "ufloat",
                                              "string",
                                              "semi-colon",
                                              "comma",
                                              "start-paragraph",
                                              "end-paragraph",
                                              "start-bracket",
                                              "end-bracket",
                                              "assignment"),
                       code_gens(Of typed_node_writer).code_gen(Of name)(),
                       name.of("raw-type-name"),
                       code_gen.of_only_child(Of typed_node_writer)("type-name"),
 _
                       code_gens(Of typed_node_writer).code_gen(Of [function])(),
                       code_gen.of_all_children(Of typed_node_writer)("paramlist"),
                       code_gen.of_all_children_with_wrapper(Of scope_wrapper, typed_node_writer) _
                                                            (AddressOf scope.wrap, "multi-sentence-paragraph"),
                       code_gen.of_all_children(Of typed_node_writer)("param-with-comma"),
                       code_gen.of_only_child(Of typed_node_writer)("reference"),
                       code_gen.of_leaf_node("bit-and"),  ' Allowing & in param.
                       code_gen.of_all_children(Of typed_node_writer)("param"),
                       code_gens(Of typed_node_writer).code_gen(Of namespace_)(),
                       code_gen.of_only_child(Of typed_node_writer)("paragraph"),
                       code_gen.of_all_children(Of typed_node_writer)("sentence"),
                       code_gen.of_all_children(Of typed_node_writer)("base-sentence-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("b2style-sentence-with-semi-colon"),
                       code_gen.of_only_child(Of typed_node_writer)("sentence-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("value-definition"),
                       code_gen.of_all_children(Of typed_node_writer)("value-declaration"),
                       code_gen.of_all_children(Of typed_node_writer)("heap-declaration"),
                       code_gen.of_all_children(Of typed_node_writer)("value-clause"),
                       code_gen.of_all_children(Of typed_node_writer)("heap-name"),
                       heap_struct_name.code_gen,
                       code_gens(Of typed_node_writer).code_gen(Of self_value_clause)(),
                       code_gen.of_all_children(Of typed_node_writer)("return-clause"),
                       code_gen.of_only_child(Of typed_node_writer)("ignore-result-function-call"),
                       code_gen.of_only_child(Of typed_node_writer)("ignore-result-heap-struct-function-call"),
                       code_gen.of_all_children(Of typed_node_writer)("logic"),
                       code_gen.of_all_children(Of typed_node_writer)("condition"),
                       code_gen.of_all_children(Of typed_node_writer)("while"),
                       code_gen.of_all_children_with_wrapper(Of scope_wrapper, typed_node_writer) _
                                                            (AddressOf scope.wrap, "for-loop"),
                       code_gen.of_only_child(Of typed_node_writer)("for-increase"),
                       code_gen.of_only_child(Of typed_node_writer)("base-for-increase"),
                       code_gen.of_all_children(Of typed_node_writer)("value"),
                       code_gen.of_all_children(Of typed_node_writer)("else-condition"),
                       code_gen.of_all_children(Of typed_node_writer)("value-with-bracket"),
                       code_gen.of_all_children(Of typed_node_writer)("raw-value"),
                       code_gen.of_all_children(Of typed_node_writer)("value-without-bracket"),
                       code_gen.of_only_child(Of typed_node_writer)("base-value-without-bracket"),
                       code_gen.of_all_children(Of typed_node_writer)("value-with-operation"),
                       code_gen.of_all_children(Of typed_node_writer)("unary-operation-value"),
                       code_gens(Of typed_node_writer).code_gen(Of binary_operation_value)(),
                       code_gens(Of typed_node_writer).code_gen(Of pre_operation_value)(),
                       code_gens(Of typed_node_writer).code_gen(Of post_operation_value)(),
                       code_gen.of_all_children(Of typed_node_writer)("variable-name"),
                       code_gens(Of typed_node_writer).code_gen(Of function_call)(),
                       code_gens(Of typed_node_writer).code_gen(Of heap_struct_function_call)(),
                       code_gen.of_all_children(Of typed_node_writer)("value-list"),
                       code_gen.of_all_children(Of typed_node_writer)("value-with-comma"),
 _
                       code_gen.of_all_children(Of typed_node_writer)("include"),
                       include_with_string.code_gen,
                       include_with_file.code_gen,
 _
                       code_gen.of_leaf_nodes("kw-ifndef",
                                              "kw-define",
                                              "kw-endif"),
                       code_gen.of_all_children(Of typed_node_writer)("ifndef-wrapped"),
                       code_gen.of_all_children(Of typed_node_writer)("define"),
 _
                       code_gen.of_leaf_node("kw-typedef"),
                       code_gen.of_only_child(Of typed_node_writer)("typedef-type"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef-type-name"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef-type-str"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef"),
 _
                       code_gens(Of typed_node_writer).code_gen(Of struct)(),
                       code_gen.of_leaf_node("kw-struct"),
 _
                       [class].code_gen,
 _
                       code_gens(Of typed_node_writer).code_gen(Of template)(),
                       code_gens(Of typed_node_writer).code_gen(Of template_type_name)(),
                       code_gen.of_all_children(Of typed_node_writer)("type-param-list"),
                       code_gen.of_first_child(Of typed_node_writer)("type-param-with-comma"),
                       code_gen.of_children_word_str(Of typed_node_writer)("type-param"),
 _
                       code_gen.of_all_children(Of typed_node_writer)("reinterpret-cast")
                   )
        End Function
    End Class

    Public Shared Function with_functions(ByVal functions As interrupts) As parse_wrapper
        Return New parse_wrapper(functions)
    End Function

    Public Shared Function with_default_functions() As parse_wrapper
        Return with_functions(interrupts.default)
    End Function

    Public NotInheritable Shadows Class parse_wrapper
        Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                          syntaxer_rule_t,
                                          __do.default_of(Of vector(Of Action(Of statements))),
                                          __do.default_of(Of vector(Of Action(Of statements))),
                                          rewriter_gens_t,
                                          scope).parse_wrapper

        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function text_import(ByVal s As String, ByVal o As exportable) As Boolean
            Return bstyle.with_functions(functions).parse(s, o)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
