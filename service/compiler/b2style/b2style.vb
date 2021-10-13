
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.interpreter.primitive
Imports statements = osi.service.compiler.rewriters.statements

Partial Public NotInheritable Class b2style
    Inherits rewriter_rule_wrapper(Of parameters_t,
                                      nlexer_rule_t,
                                      syntaxer_rule_t,
                                      prefixes_t,
                                      suffixes_t,
                                      rewriter_gens_t)
    Public NotInheritable Class parameters_t
        Public ReadOnly defines As New [set](Of String)()
    End Class

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
        Inherits __do(Of vector(Of Action(Of statements, parameters_t)))

        Protected Overrides Function at() As vector(Of Action(Of statements, parameters_t))
            Return vector.of(Of Action(Of statements, parameters_t))()
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements, parameters_t)))

        Protected Overrides Function at() As vector(Of Action(Of statements, parameters_t))
            Return New vector(Of Action(Of statements, parameters_t))()
        End Function
    End Class

    Public NotInheritable Class rewriter_gens_t
        Inherits __do(Of vector(Of Action(Of rewriters, parameters_t)))

        Protected Overrides Function at() As vector(Of Action(Of rewriters, parameters_t))
            Return vector.of(
                       default_registerer("root-type"),
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
                       default_registerer("bool"),
                       default_registerer("integer"),
                       default_registerer("biguint"),
                       default_registerer("ufloat"),
                       default_registerer("string"),
                       default_registerer("semi-colon"),
                       default_registerer("comma"),
                       default_registerer("start-paragraph"),
                       default_registerer("end-paragraph"),
                       default_registerer("start-bracket"),
                       default_registerer("end-bracket"),
                       default_registerer("assignment"),
                       ignore_parameters(AddressOf name.register),
 _
                       default_registerer("function"),
                       default_registerer("paramlist"),
                       default_registerer("multi-sentence-paragraph"),
                       default_registerer("param-with-comma"),
                       default_registerer("param"),
                       ignore_parameters(AddressOf namespace_.register),
                       default_registerer("paragraph"),
                       default_registerer("sentence"),
                       default_registerer("sentence-with-semi-colon"),
                       default_registerer("sentence-without-semi-colon"),
                       default_registerer("value-definition"),
                       default_registerer("value-declaration"),
                       default_registerer("value-definition-with-semi-colon"),
                       default_registerer("value-declaration-with-semi-colon"),
                       default_registerer("value-clause"),
                       ignore_parameters(AddressOf self_value_clause.register),
                       default_registerer("return-clause"),
                       default_registerer("ignore-result-function-call"),
                       default_registerer("logic"),
                       default_registerer("logic-with-semi-colon"),
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
                       default_registerer("value-with-comma"),
 _
                       default_registerer("include"),
                       ignore_parameters(AddressOf include_with_string.register),
                       ignore_parameters(AddressOf include_with_file.register),
 _
                       default_registerer("kw-ifndef"),
                       default_registerer("kw-define"),
                       default_registerer("kw-endif"),
                       default_registerer("ifndef-wrapped"),
                       default_registerer("define"),
 _
                       default_registerer("kw-typedef"),
                       default_registerer("typedef-type"),
                       default_registerer("typedef"),
                       default_registerer("typedef-with-semi-colon"),
 _
                       ignore_parameters(AddressOf struct.register)
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
        Inherits rewriter_rule_wrapper(Of parameters_t,
                                          nlexer_rule_t,
                                          syntaxer_rule_t,
                                          prefixes_t,
                                          suffixes_t,
                                          rewriter_gens_t).parse_wrapper

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
