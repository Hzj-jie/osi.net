
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.compiler.rewriters
Imports osi.service.interpreter.primitive
Imports statements = osi.service.compiler.rewriters.statements

Partial Public NotInheritable Class b2style
    Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                      syntaxer_rule_t,
                                      prefixes_t,
                                      suffixes_t,
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

    Public NotInheritable Class prefixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.of(Of Action(Of statements))()
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return New vector(Of Action(Of statements))()
        End Function
    End Class

    Public NotInheritable Class rewriter_gens_t
        Inherits __do(Of vector(Of Action(Of rewriters)))

        Protected Overrides Function at() As vector(Of Action(Of rewriters))
            Return vector.of(Of Action(Of rewriters))(
                       bypass.registerer("root-type"),
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
                       bypass.registerer("function"),
                       bypass.registerer("paramlist"),
                       bypass.registerer("multi-sentence-paragraph"),
                       bypass.registerer("param-with-comma"),
                       AddressOf param.register,
                       AddressOf namespace_.register,
                       AddressOf paragraph.register,
                       bypass.registerer("sentence"),
                       bypass.registerer("sentence-with-semi-colon"),
                       bypass.registerer("sentence-without-semi-colon"),
                       bypass.registerer("value-definition"),
                       bypass.registerer("value-declaration"),
                       bypass.registerer("heap-declaration"),
                       bypass.registerer("value-definition-with-semi-colon"),
                       bypass.registerer("value-declaration-with-semi-colon"),
                       bypass.registerer("heap-declaration-with-semi-colon"),
                       bypass.registerer("value-clause"),
                       bypass.registerer("heap-clause"),
                       bypass.registerer("heap-name"),
                       AddressOf heap_struct_name.register,
                       AddressOf self_value_clause.register,
                       bypass.registerer("return-clause"),
                       bypass.registerer("ignore-result-function-call"),
                       bypass.registerer("logic"),
                       bypass.registerer("logic-with-semi-colon"),
                       bypass.registerer("condition"),
                       bypass.registerer("while"),
                       bypass.registerer("for-loop"),
                       bypass.registerer("value"),
                       bypass.registerer("else-condition"),
                       bypass.registerer("value-with-bracket"),
                       bypass.registerer("raw-value"),
                       bypass.registerer("value-without-bracket"),
                       bypass.registerer("value-with-operation"),
                       bypass.registerer("unary-operation-value"),
                       AddressOf binary_operation_value.register,
                       AddressOf pre_operation_value.register,
                       AddressOf post_operation_value.register,
                       bypass.registerer("variable-name"),
                       bypass.registerer("function-call"),
                       bypass.registerer("value-list"),
                       bypass.registerer("value-with-comma"),
 _
                       bypass.registerer("include"),
                       AddressOf include_with_string.register,
                       AddressOf include_with_file.register,
 _
                       leaf.registerer("kw-ifndef"),
                       leaf.registerer("kw-define"),
                       leaf.registerer("kw-endif"),
                       bypass.registerer("ifndef-wrapped"),
                       bypass.registerer("define"),
 _
                       leaf.registerer("kw-typedef"),
                       bypass.registerer("typedef-type"),
                       bypass.registerer("typedef"),
                       bypass.registerer("typedef-with-semi-colon"),
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
                                          prefixes_t,
                                          suffixes_t,
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
