
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.template
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
                       bypass_registerer("root-type"),
 _
                       leaf_registerer("kw-if"),
                       leaf_registerer("kw-else"),
                       leaf_registerer("kw-for"),
                       leaf_registerer("kw-while"),
                       leaf_registerer("kw-do"),
                       leaf_registerer("kw-loop"),
                       leaf_registerer("kw-return"),
                       leaf_registerer("kw-break"),
                       leaf_registerer("kw-logic"),
                       leaf_registerer("bool"),
                       leaf_registerer("integer"),
                       leaf_registerer("biguint"),
                       leaf_registerer("ufloat"),
                       leaf_registerer("string"),
                       leaf_registerer("semi-colon"),
                       leaf_registerer("comma"),
                       leaf_registerer("start-paragraph"),
                       leaf_registerer("end-paragraph"),
                       leaf_registerer("start-bracket"),
                       leaf_registerer("end-bracket"),
                       leaf_registerer("assignment"),
                       AddressOf name.register,
 _
                       bypass_registerer("function"),
                       bypass_registerer("paramlist"),
                       bypass_registerer("multi-sentence-paragraph"),
                       bypass_registerer("param-with-comma"),
                       bypass_registerer("param"),
                       AddressOf namespace_.register,
                       AddressOf paragraph.register,
                       bypass_registerer("sentence"),
                       bypass_registerer("sentence-with-semi-colon"),
                       bypass_registerer("sentence-without-semi-colon"),
                       bypass_registerer("value-definition"),
                       bypass_registerer("value-declaration"),
                       bypass_registerer("value-definition-with-semi-colon"),
                       bypass_registerer("value-declaration-with-semi-colon"),
                       bypass_registerer("value-clause"),
                       AddressOf self_value_clause.register,
                       bypass_registerer("return-clause"),
                       bypass_registerer("ignore-result-function-call"),
                       bypass_registerer("logic"),
                       bypass_registerer("logic-with-semi-colon"),
                       bypass_registerer("condition"),
                       bypass_registerer("while"),
                       bypass_registerer("for-loop"),
                       bypass_registerer("value"),
                       bypass_registerer("else-condition"),
                       bypass_registerer("value-with-bracket"),
                       bypass_registerer("raw-value"),
                       bypass_registerer("value-without-bracket"),
                       bypass_registerer("value-with-operation"),
                       bypass_registerer("unary-operation-value"),
                       AddressOf binary_operation_value.register,
                       AddressOf pre_operation_value.register,
                       AddressOf post_operation_value.register,
                       bypass_registerer("variable-name"),
                       bypass_registerer("function-call"),
                       bypass_registerer("value-list"),
                       bypass_registerer("value-with-comma"),
 _
                       bypass_registerer("include"),
                       AddressOf include_with_string.register,
                       AddressOf include_with_file.register,
 _
                       leaf_registerer("kw-ifndef"),
                       leaf_registerer("kw-define"),
                       leaf_registerer("kw-endif"),
                       bypass_registerer("ifndef-wrapped"),
                       bypass_registerer("define"),
 _
                       leaf_registerer("kw-typedef"),
                       bypass_registerer("typedef-type"),
                       bypass_registerer("typedef"),
                       bypass_registerer("typedef-with-semi-colon"),
 _
                       AddressOf struct.register,
                       leaf_registerer("kw-struct")
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
