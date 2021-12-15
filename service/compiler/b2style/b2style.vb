
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
Imports [default] = osi.service.compiler.code_gens(Of osi.service.compiler.rewriters.typed_node_writer).[default]
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
                       [default].of_all_children("root-type"),
 _
                       leaf.of("kw-if"),
                       leaf.of("kw-else"),
                       leaf.of("kw-for"),
                       leaf.of("kw-while"),
                       leaf.of("kw-do"),
                       leaf.of("kw-loop"),
                       leaf.of("kw-return"),
                       leaf.of("kw-break"),
                       leaf.of("kw-logic"),
                       leaf.of("start-square-bracket"),
                       leaf.of("end-square-bracket"),
                       leaf.of("bool"),
                       leaf.of("integer"),
                       leaf.of("biguint"),
                       leaf.of("ufloat"),
                       leaf.of("string"),
                       leaf.of("semi-colon"),
                       leaf.of("comma"),
                       leaf.of("start-paragraph"),
                       leaf.of("end-paragraph"),
                       leaf.of("start-bracket"),
                       leaf.of("end-bracket"),
                       leaf.of("assignment"),
                       AddressOf name.register,
 _
                       AddressOf [function].register,
                       [default].of_all_children("paramlist"),
                       [default].of_all_children_with_wrapper(AddressOf scope.wrap, "multi-sentence-paragraph"),
                       [default].of_all_children("param-with-comma"),
                       leaf.of("bit-and"),  ' Allowing & in param.
                       [default].of_all_children("param"),
                       AddressOf namespace_.register,
                       [default].of_only_child("paragraph"),
                       [default].of_all_children("sentence"),
                       [default].of_all_children("sentence-with-semi-colon"),
                       [default].of_all_children("sentence-without-semi-colon"),
                       [default].of_all_children("value-definition"),
                       [default].of_all_children("value-declaration"),
                       [default].of_all_children("heap-declaration"),
                       [default].of_all_children("value-definition-with-semi-colon"),
                       [default].of_all_children("value-declaration-with-semi-colon"),
                       [default].of_all_children("heap-declaration-with-semi-colon"),
                       [default].of_all_children("value-clause"),
                       [default].of_all_children("heap-name"),
                       AddressOf heap_struct_name.register,
                       AddressOf self_value_clause.register,
                       [default].of_all_children("return-clause"),
                       [default].of_only_child("ignore-result-function-call"),
                       [default].of_only_child("ignore-result-heap-struct-function-call"),
                       [default].of_all_children("logic"),
                       [default].of_all_children("logic-with-semi-colon"),
                       [default].of_all_children("condition"),
                       [default].of_all_children("while"),
                       [default].of_all_children_with_wrapper(AddressOf scope.wrap, "for-loop"),
                       [default].of_all_children("value"),
                       [default].of_all_children("else-condition"),
                       [default].of_all_children("value-with-bracket"),
                       [default].of_all_children("raw-value"),
                       [default].of_all_children("value-without-bracket"),
                       [default].of_all_children("value-with-operation"),
                       [default].of_all_children("unary-operation-value"),
                       AddressOf binary_operation_value.register,
                       AddressOf pre_operation_value.register,
                       AddressOf post_operation_value.register,
                       [default].of_all_children("variable-name"),
                       AddressOf function_call.register,
                       AddressOf heap_struct_function_call.register,
                       [default].of_all_children("value-list"),
                       [default].of_all_children("value-with-comma"),
 _
                       [default].of_all_children("include"),
                       AddressOf include_with_string.register,
                       AddressOf include_with_file.register,
 _
                       leaf.of("kw-ifndef"),
                       leaf.of("kw-define"),
                       leaf.of("kw-endif"),
                       [default].of_all_children("ifndef-wrapped"),
                       [default].of_all_children("define"),
 _
                       leaf.of("kw-typedef"),
                       [default].of_all_children("typedef-type"),
                       [default].of_all_children("typedef"),
                       [default].of_all_children("typedef-with-semi-colon"),
 _
                       AddressOf struct.register,
                       leaf.of("kw-struct"),
 _
                       AddressOf [class].register,
 _
                       AddressOf template.register
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
