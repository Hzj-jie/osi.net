
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template
Imports osi.service.interpreter.primitive
Imports statements = osi.service.compiler.rewriters.statements

Public NotInheritable Class b2style
    Inherits rewriter_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, rewriter_gens_t)

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
        Inherits __do(Of vector(Of Action(Of statements, rewriter_rule_wrapper)))

        Protected Overrides Function at() As vector(Of Action(Of statements, rewriter_rule_wrapper))
            Return vector.of(
                       ignore_parameters(AddressOf prefix.register))
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements, rewriter_rule_wrapper)))

        Protected Overrides Function at() As vector(Of Action(Of statements, rewriter_rule_wrapper))
            Return vector.of(
                       ignore_parameters(AddressOf suffix.register))
        End Function
    End Class

    Public NotInheritable Class rewriter_gens_t
        Inherits __do(Of vector(Of Action(Of rewriters, rewriter_rule_wrapper)))

        Protected Overrides Function at() As vector(Of Action(Of rewriters, rewriter_rule_wrapper))
            Return vector.of(
                       default_registerer("less-or-equal"),
                       default_registerer("greater-or-equal"),
                       default_registerer("equal"),
                       default_registerer("less-than"),
                       default_registerer("greater-than"),
                       default_registerer("add"),
                       default_registerer("minus"),
                       default_registerer("multiply"),
                       default_registerer("divide"),
                       default_registerer("mod"),
                       default_registerer("power"),
                       default_registerer("bit-and"),
                       default_registerer("bit-or"),
                       default_registerer("and"),
                       default_registerer("or"),
                       default_registerer("not"),
                       default_registerer("self-inc"),
                       default_registerer("self-dec"),
                       default_registerer("self-add"),
                       default_registerer("self-minus"),
                       default_registerer("self-multiply"),
                       default_registerer("self-divide"),
                       default_registerer("self-mod"),
                       default_registerer("self-power"),
                       default_registerer("self-bit-and"),
                       default_registerer("self-bit-or"),
                       default_registerer("self-and"),
                       default_registerer("self-or"),
 _
                       default_registerer("kw-if"),
                       default_registerer("kw-else"),
                       default_registerer("kw-for"),
                       default_registerer("kw-while"),
                       default_registerer("kw-do"),
                       default_registerer("kw-loop"),
                       default_registerer("kw-return"),
                       default_registerer("kw-break"),
                       default_registerer("kw-logic"),
                       default_registerer("blank"),
                       default_registerer("bool"),
                       default_registerer("integer"),
                       default_registerer("float"),
                       default_registerer("string"),
                       default_registerer("comma"),
                       default_registerer("colon"),
                       default_registerer("question-mark"),
                       default_registerer("start-paragraph"),
                       default_registerer("end-paragraph"),
                       default_registerer("start-bracket"),
                       default_registerer("end-bracket"),
                       default_registerer("start-square-bracket"),
                       default_registerer("end-square-bracket"),
                       default_registerer("semi-colon"),
                       default_registerer("assignment"),
                       ignore_parameters(AddressOf name.register),
 _
                       default_registerer("function"),
                       default_registerer("paramlist"),
                       default_registerer("multi-sentence-paragraph"),
                       default_registerer("param-with-comma"),
                       default_registerer("param"),
                       default_registerer("value-definition-with-semi-colon"),
                       ignore_parameters(AddressOf kw_namespace.register),
                       default_registerer("paragraph"),
                       default_registerer("sentence"),
                       default_registerer("sentence-with-semi-colon"),
                       default_registerer("sentence-without-semi-colon"),
                       default_registerer("value-definition"),
                       default_registerer("value-declaration"),
                       default_registerer("value-clause"),
                       ignore_parameters(AddressOf self_value_clause.register),
                       default_registerer("return-clause"),
                       default_registerer("ignore-return-function-call"),
                       default_registerer("logic"),
                       default_registerer("condition"),
                       default_registerer("while"),
                       default_registerer("for-loop"),
                       default_registerer("value"),
                       default_registerer("else-condition"),
                       default_registerer("value-with-bracket"),
                       default_registerer("raw-value"),
                       default_registerer("value-without-bracket"),
                       default_registerer("value-with-operation"),
                       default_registerer("unary-operation-value"),
                       ignore_parameters(AddressOf binary_operation_value.register),
                       ignore_parameters(AddressOf pre_operation_value.register),
                       ignore_parameters(AddressOf post_operation_value.register),
                       default_registerer("variable-name"),
                       default_registerer("function-call"),
                       default_registerer("value-list"),
                       default_registerer("value-with-comma")
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
                                          rewriter_gens_t).parse_wrapper

        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function logic_parse(ByVal s As String, ByRef e() As logic.exportable) As Boolean
            Dim w As logic.writer = Nothing
            w = New logic.writer()
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
