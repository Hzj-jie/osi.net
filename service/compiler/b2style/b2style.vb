
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.rewriters
Imports osi.service.interpreter.primitive
Imports [default] = osi.service.compiler.code_gens(Of osi.service.compiler.rewriters.typed_node_writer).[default]
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.rewriters.typed_node_writer)

Partial Public NotInheritable Class b2style
    Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                      syntaxer_rule_t,
                                      __do.default_of(Of vector(Of Action(Of statements))),
                                      __do.default_of(Of vector(Of Action(Of statements))),
                                      rewriter_gens_t,
                                      scope)
    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of Byte())

        Protected Overrides Function at() As Byte()
            Return b2style_rules.nlexer_rule
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of Byte())

        Protected Overrides Function at() As Byte()
            Return b2style_rules.syntaxer_rule
        End Function
    End Class

    Public NotInheritable Class rewriter_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of typed_node_writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of typed_node_writer)))
            Return vector.of(Of Action(Of code_gens(Of typed_node_writer)))(
                       [default].of_all_children("root-type"),
 _
                       leaf.registerer("kw-if"),
                       leaf.registerer("kw-else"),
                       leaf.registerer("kw-for"),
                       leaf.registerer("kw-while"),
                       leaf.registerer("kw-do"),
                       leaf.registerer("kw-loop"),
                       leaf.registerer("kw-return"),
                       leaf.registerer("kw-break"),
                       leaf.registerer("kw-logic"),
                       leaf.registerer("start-square-bracket"),
                       leaf.registerer("end-square-bracket"),
                       leaf.registerer("bool"),
                       leaf.registerer("integer"),
                       leaf.registerer("biguint"),
                       leaf.registerer("ufloat"),
                       leaf.registerer("string"),
                       leaf.registerer("semi-colon"),
                       leaf.registerer("comma"),
                       leaf.registerer("start-paragraph"),
                       leaf.registerer("end-paragraph"),
                       leaf.registerer("start-bracket"),
                       leaf.registerer("end-bracket"),
                       leaf.registerer("assignment"),
                       AddressOf name.register,
 _
                       [default].of_all_children("function"),
                       [default].of_all_children("paramlist"),
                       [default].of_all_children("multi-sentence-paragraph"),
                       [default].of_all_children("param-with-comma"),
                       AddressOf param.register,
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
                       [default].of_all_children("ignore-result-function-call"),
                       [default].of_all_children("logic"),
                       [default].of_all_children("logic-with-semi-colon"),
                       [default].of_all_children("condition"),
                       [default].of_all_children("while"),
                       [default].of_all_children("for-loop"),
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
                       [default].of_all_children("function-call"),
                       [default].of_all_children("value-list"),
                       [default].of_all_children("value-with-comma"),
 _
                       [default].of_all_children("include"),
                       AddressOf include_with_string.register,
                       AddressOf include_with_file.register,
 _
                       leaf.registerer("kw-ifndef"),
                       leaf.registerer("kw-define"),
                       leaf.registerer("kw-endif"),
                       [default].of_all_children("ifndef-wrapped"),
                       [default].of_all_children("define"),
 _
                       leaf.registerer("kw-typedef"),
                       [default].of_all_children("typedef-type"),
                       [default].of_all_children("typedef"),
                       [default].of_all_children("typedef-with-semi-colon"),
 _
                       AddressOf struct.register,
                       leaf.registerer("kw-struct")
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

        Protected Overrides Function logic_parse(ByVal s As String, ByRef e() As logic.exportable) As Boolean
            Dim w As New logic.writer()
            Dim v As vector(Of logic.exportable) = Nothing
            If bstyle.parse(s, w) AndAlso w.dump(functions, v.renew()) Then
                e = +v
                Return True
            End If
            Return False
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
