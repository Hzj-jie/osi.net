
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
            Return vector.emplace_of(Of Action(Of code_gens(Of typed_node_writer)))(
                       code_gen.of_all_children(Of typed_node_writer)("root-type"),
 _
                       code_gen.of_leaf_node("kw-if"),
                       code_gen.of_leaf_node("kw-else"),
                       code_gen.of_leaf_node("kw-for"),
                       code_gen.of_leaf_node("kw-while"),
                       code_gen.of_leaf_node("kw-do"),
                       code_gen.of_leaf_node("kw-loop"),
                       code_gen.of_leaf_node("kw-return"),
                       code_gen.of_leaf_node("kw-break"),
                       code_gen.of_leaf_node("kw-logic"),
                       code_gen.of_leaf_node("start-square-bracket"),
                       code_gen.of_leaf_node("end-square-bracket"),
                       code_gen.of_leaf_node("bool"),
                       code_gen.of_leaf_node("integer"),
                       code_gen.of_leaf_node("biguint"),
                       code_gen.of_leaf_node("ufloat"),
                       code_gen.of_leaf_node("string"),
                       code_gen.of_leaf_node("semi-colon"),
                       code_gen.of_leaf_node("comma"),
                       code_gen.of_leaf_node("start-paragraph"),
                       code_gen.of_leaf_node("end-paragraph"),
                       code_gen.of_leaf_node("start-bracket"),
                       code_gen.of_leaf_node("end-bracket"),
                       code_gen.of_leaf_node("assignment"),
                       AddressOf name.register,
                       name.of("raw-type-name"),
                       code_gen.of_only_child(Of typed_node_writer)("type-name"),
 _
                       AddressOf [function].register,
                       code_gen.of_all_children(Of typed_node_writer)("paramlist"),
                       code_gen.of_all_children_with_wrapper(Of scope_wrapper, typed_node_writer) _
                                                            (AddressOf scope.wrap, "multi-sentence-paragraph"),
                       code_gen.of_all_children(Of typed_node_writer)("param-with-comma"),
                       code_gen.of_only_child(Of typed_node_writer)("reference"),
                       code_gen.of_leaf_node("bit-and"),  ' Allowing & in param.
                       code_gen.of_all_children(Of typed_node_writer)("param"),
                       AddressOf namespace_.register,
                       code_gen.of_only_child(Of typed_node_writer)("paragraph"),
                       code_gen.of_all_children(Of typed_node_writer)("sentence"),
                       code_gen.of_all_children(Of typed_node_writer)("sentence-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("sentence-without-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("value-definition"),
                       code_gen.of_all_children(Of typed_node_writer)("value-declaration"),
                       code_gen.of_all_children(Of typed_node_writer)("heap-declaration"),
                       code_gen.of_all_children(Of typed_node_writer)("value-definition-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("value-declaration-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("heap-declaration-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("value-clause"),
                       code_gen.of_all_children(Of typed_node_writer)("heap-name"),
                       AddressOf heap_struct_name.register,
                       AddressOf self_value_clause.register,
                       code_gen.of_all_children(Of typed_node_writer)("return-clause"),
                       code_gen.of_only_child(Of typed_node_writer)("ignore-result-function-call"),
                       code_gen.of_only_child(Of typed_node_writer)("ignore-result-heap-struct-function-call"),
                       code_gen.of_all_children(Of typed_node_writer)("logic"),
                       code_gen.of_all_children(Of typed_node_writer)("logic-with-semi-colon"),
                       code_gen.of_all_children(Of typed_node_writer)("condition"),
                       code_gen.of_all_children(Of typed_node_writer)("while"),
                       code_gen.of_all_children_with_wrapper(Of scope_wrapper, typed_node_writer) _
                                                            (AddressOf scope.wrap, "for-loop"),
                       code_gen.of_all_children(Of typed_node_writer)("value"),
                       code_gen.of_all_children(Of typed_node_writer)("else-condition"),
                       code_gen.of_all_children(Of typed_node_writer)("value-with-bracket"),
                       code_gen.of_all_children(Of typed_node_writer)("raw-value"),
                       code_gen.of_all_children(Of typed_node_writer)("value-without-bracket"),
                       code_gen.of_all_children(Of typed_node_writer)("value-with-operation"),
                       code_gen.of_all_children(Of typed_node_writer)("unary-operation-value"),
                       AddressOf binary_operation_value.register,
                       AddressOf pre_operation_value.register,
                       AddressOf post_operation_value.register,
                       code_gen.of_all_children(Of typed_node_writer)("variable-name"),
                       AddressOf function_call.register,
                       AddressOf heap_struct_function_call.register,
                       code_gen.of_all_children(Of typed_node_writer)("value-list"),
                       code_gen.of_all_children(Of typed_node_writer)("value-with-comma"),
 _
                       code_gen.of_all_children(Of typed_node_writer)("include"),
                       AddressOf include_with_string.register,
                       AddressOf include_with_file.register,
 _
                       code_gen.of_leaf_node("kw-ifndef"),
                       code_gen.of_leaf_node("kw-define"),
                       code_gen.of_leaf_node("kw-endif"),
                       code_gen.of_all_children(Of typed_node_writer)("ifndef-wrapped"),
                       code_gen.of_all_children(Of typed_node_writer)("define"),
 _
                       code_gen.of_leaf_node("kw-typedef"),
                       code_gen.of_only_child(Of typed_node_writer)("typedef-type"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef-type-name"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef-type-str"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef"),
                       code_gen.of_all_children(Of typed_node_writer)("typedef-with-semi-colon"),
 _
                       AddressOf struct.register,
                       code_gen.of_leaf_node("kw-struct"),
 _
                       AddressOf [class].register,
 _
                       AddressOf template.register,
                       AddressOf template_type_name.register,
                       code_gen.of_all_children(Of typed_node_writer)("type-param-list"),
                       code_gen.of_first_child(Of typed_node_writer)("type-param-with-comma"),
                       code_gen.of_children_word_str(Of typed_node_writer)("type-param")
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
