
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.interpreter.primitive
Imports osi.service.resource
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.rewriters.typed_node_writer)

Partial Public NotInheritable Class b2style
    Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                      syntaxer_rule_t,
                                      __do.default_of(Of vector(Of Action(Of statements))),
                                      suffixes_t,
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

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf scope.call_hierarchy_t.calculator.register)
        End Function
    End Class

    Public NotInheritable Class rewriter_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of typed_node_writer))))

        ' TODO: May provide default behavior in rewrite-rule.
        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of typed_node_writer)))
            Return New typed_node_writer_code_gens_registrar().
                           with(Of multi_sentence_paragraph)().
                           with(code_gen.of_all_children_with_wrapper(Of scope, typed_node_writer) _
                                                                     (AddressOf scope.wrap, "for-loop")).
                           with(code_gen.of_all_children_with_wrapper(Of scope, typed_node_writer) _
                                                                     (AddressOf scope.wrap, "struct")).
                           with(Of _namespace)().
                           with(Of heap_struct_name)().
                           with(Of binary_operation_value)("self-value-clause").
                           with(Of binary_operation_value)().
                           with("pre-operation-value", New unary_operation_value(0, "_pre")).
                           with("post-operation-value", New unary_operation_value(1, "_post")).
                           with(Of function_call)().
                           with_delegate("heap-struct-function-call",
                                         Function(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                                             assert(Not n Is Nothing)
                                             Return function_call.build(
                                                        heap_struct_name.bstyle_function(n.child(0)), n, o)
                                         End Function).
                           with(Of include_with_string)().
                           with(Of include_with_file)().
                           with(Of _class)().
                           with_delegate("template",
                                         Function(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                                             Dim name As String = Nothing
                                             Dim t As scope.template_template = Nothing
                                             Return scope.template_t.of(n, name, t) AndAlso
                                                    scope.current().template().define(name, t)
                                         End Function).
                           with(Of template_type_name).
                           with(Of function_call_with_template).
                           with(Of function_name_with_template).
                           with(Of name)().
                           with(Of name)("raw-type-name").
                           with(Of _function)().
                           with(Of delegate_with_semi_colon)().
                           with(code_gen.of_first_child(Of typed_node_writer)("type-param-with-comma")).
                           with(code_gen.of_first_child(Of typed_node_writer)("type-name-with-comma")).
                           with(code_gen.of_only_descendant_str(Of typed_node_writer)("type-param")).
                           with(code_gen.of_only_descendant_str(Of typed_node_writer)("reference")).
                           with(code_gen.of_all_children_with_precondition(Of typed_node_writer)(
                                     "value-definition",
                                     scope.variable_proxy.define(),
                                     scope.call_hierarchy_t.from_value_clause())).
                           with(code_gen.of_all_children_with_precondition(Of typed_node_writer)(
                                    scope.variable_proxy.define(), "value-declaration")).
                           with(code_gen.of_all_children_with_precondition(Of typed_node_writer)(
                                    scope.call_hierarchy_t.from_value_clause(), "value-clause")).
                           with(scope.define_t.code_gens.ifdef_wrapped(AddressOf code_gen_of)).
                           with(scope.define_t.code_gens.ifndef_wrapped(AddressOf code_gen_of)).
                           with(scope.define_t.code_gens.define()).
                           with(Of paramtype_with_comma)().
                           with(Of class_initializer)().
                           with(code_gen.of_ignore(Of typed_node_writer)("colon")).
                           with_delegate("root-type",
                                         Function(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                                             scope.current().root_type_injector()._new(o)
                                             Return code_gen_of(n.child()).build(o)
                                         End Function).
                           with_delegate("kw-statement",
                                         Function(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                                             assert(Not n Is Nothing)
                                             assert(Not o Is Nothing)
                                             Return assert(
                                                 o.append("""" + n.ancestor_of("sentence").input().c_escape() + """"))
                                         End Function).
                           with_delegate("kw-file",
                                         Function(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
                                             assert(Not n Is Nothing)
                                             assert(Not o Is Nothing)
                                             Return assert(
                                                 o.append("""" + compile_wrapper.current_file().c_escape() + """"))
                                         End Function).
                           with(code_gen.of_input(Of typed_node_writer)("kw-func")).
                           with(code_gen.of_input(Of typed_node_writer)("kw-line")).
                           with_of_only_childs(
                               "base-root-type",
                               "paragraph",
                               "sentence-with-semi-colon",
                               "ignore-result-function-call",
                               "ignore-result-heap-struct-function-call",
                               "ignore-result-function-call-with-template",
                               "for-increase",
                               "base-for-increase",
                               "base-value-without-bracket",
                               "typedef-type",
                               "type-name",
                               "unary-operation-value").
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
                               "kw-undefine",
                               "kw-dealloc",
                               "kw-static-cast",
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
                               "heap-declaration",
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
                               "variable-name",
                               "value-list",
                               "value-with-comma",
                               "include",
                               "typedef-type-name",
                               "typedef-type-str",
                               "typedef",
                               "type-param-list",
                               "reinterpret-cast",
                               "delegate",
                               "paramtypelist",
                               "paramtype",
                               "struct-body",
                               "undefine",
                               "dealloc",
                               "static-cast")
        End Function
    End Class

    Public Shared Function with_functions(ByVal functions As interrupts) As compile_wrapper
        Return New compile_wrapper(functions)
    End Function

    Public Shared Function with_default_functions() As compile_wrapper
        Return with_functions(interrupts.default)
    End Function

    Public NotInheritable Shadows Class compile_wrapper
        Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                          syntaxer_rule_t,
                                          __do.default_of(Of vector(Of Action(Of statements))),
                                          suffixes_t,
                                          rewriter_gens_t,
                                          scope).compile_wrapper

        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function text_import(ByVal s As String, ByVal o As exportable) As Boolean
            Return bstyle.with_functions(functions).compile(s, o)
        End Function
    End Class

    Public NotInheritable Shadows Class compile_wrapper_b3style
        Inherits rewriter_rule_wrapper(Of nlexer_rule_t,
                                          syntaxer_rule_t,
                                          __do.default_of(Of vector(Of Action(Of statements))),
                                          suffixes_t,
                                          rewriter_gens_t,
                                          scope).compile_wrapper

        Public Sub New(ByVal functions As interrupts)
            MyBase.New(functions)
        End Sub

        Protected Overrides Function text_import(ByVal s As String, ByVal o As exportable) As Boolean
            Using b3style.disable_namespace()
                Return b3style.with_functions(functions).compile(s, o)
            End Using
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
