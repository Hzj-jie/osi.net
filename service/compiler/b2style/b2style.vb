
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

        ' TODO: May provide default behavior in rewrite-rule.
        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of typed_node_writer)))
            Return New typed_node_writer_code_gens_registrar().
                           with(code_gen.of_all_children_with_wrapper _
                                    (Of scope_wrapper, typed_node_writer) _
                                    (AddressOf scope.wrap, "multi-sentence-paragraph")).
                           with(code_gen.of_all_children_with_wrapper _
                                    (Of scope_wrapper, typed_node_writer) _
                                    (AddressOf scope.wrap, "for-loop")).
                           with(Of _namespace)().
                           with(heap_struct_name.instance).
                           with(Of self_value_clause)().
                           with(Of binary_operation_value)().
                           with(Of pre_operation_value)().
                           with(Of post_operation_value)().
                           with(Of function_call)().
                           with(Of heap_struct_function_call)().
                           with(include_with_string.instance).
                           with(include_with_file.instance).
                           with(Of struct)().
                           with(_class.instance).
                           with(template.instance).
                           with(Of template_type_name)().
                           with(Of name)().
                           with(name.of("raw-type-name")).
                           with(Of _function)().
                           with(code_gen.of_first_child(Of typed_node_writer)("type-param-with-comma")).
                           with(code_gen.of_children_word_str(Of typed_node_writer)("type-param")).
                           with(code_gen.of_children_word_str(Of typed_node_writer)("reference")).
                           with_of_only_childs(
                               "base-root-type",
                               "root-type",
                               "paragraph",
                               "sentence-with-semi-colon",
                               "ignore-result-function-call",
                               "ignore-result-heap-struct-function-call",
                               "for-increase",
                               "base-for-increase",
                               "base-value-without-bracket",
                               "typedef-type",
                               "type-name",
                               "self-operator").
                           with_of_leaf_nodes(
                               "kw-if",
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
                               "assignment",
                               "kw-ifndef",
                               "kw-define",
                               "kw-endif",
                               "kw-typedef",
                               "kw-struct",
                               "kw-delegate").
                           with_of_all_childrens(
                               "root-type-with-semi-colon",
                               "paramlist",
                               "param-with-comma",
                               "param",
                               "sentence",
                               "base-sentence-with-semi-colon",
                               "b2style-sentence-with-semi-colon",
                               "value-definition",
                               "value-declaration",
                               "heap-declaration",
                               "value-clause",
                               "heap-name",
                               "return-clause",
                               "logic",
                               "condition",
                               "while",
                               "value",
                               "else-condition",
                               "value-with-bracket",
                               "raw-value",
                               "value-without-bracket",
                               "value-with-operation",
                               "unary-operation-value",
                               "variable-name",
                               "value-list",
                               "value-with-comma",
                               "include",
                               "ifndef-wrapped",
                               "define",
                               "typedef-type-name",
                               "typedef-type-str",
                               "typedef",
                               "type-param-list",
                               "reinterpret-cast",
                               "delegate",
                               "paramtypelist",
                               "paramtype-with-comma",
                               "paramtype").
                           with_of_names(
                               "add",
                               "minus",
                               "multiply",
                               "divide",
                               "mod",
                               "power",
                               "bit-and",
                               "bit-or",
                               "and",
                               "or",
                               "left-shift",
                               "right-shift",
                               "less-than",
                               "greater-than",
                               "less-or-equal",
                               "greater-or-equal",
                               "equal",
                               "not-equal",
                               "self-add",
                               "self-minus",
                               "self-multiply",
                               "self-divide",
                               "self-mod",
                               "self-power",
                               "self-bit-and",
                               "self-bit-or",
                               "self-and",
                               "self-or",
                               "self-left-shift",
                               "self-right-shift",
                               "self-inc",
                               "self-dec")
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
