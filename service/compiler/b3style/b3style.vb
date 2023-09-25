
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.automata
Imports osi.service.resource
Imports builders = osi.service.compiler.logic.builders
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic_writer)

Partial Public NotInheritable Class b3style
    Inherits logic_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, logic_gens_t, scope)

    Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "service/compiler/b3style")

    ' TODO: Also disable the namespace of function / function_call.
    <ThreadStatic>
    Private Shared _disable_namespace As Boolean

    Public Shared Function disable_namespace() As IDisposable
        _disable_namespace = True
        Return defer.to(Sub()
                            _disable_namespace = False
                        End Sub)
    End Function

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "nlexer_rule.txt")

        Shared Sub New()
            static_constructor(Of b2style.nlexer_rule_t).execute()
            b3style_rules.nlexer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "syntaxer_rule.txt")

        Shared Sub New()
            static_constructor(Of b2style.syntaxer_rule_t).execute()
            b3style_rules.syntaxer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class prefixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf bstyle.code_types.register)
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf bstyle.main.register,
                                                               AddressOf scope.call_hierarchy_t.calculator.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of logic_writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of logic_writer)))
            Return New code_gens_registrar(Of logic_writer)().
                with_of_only_childs(
                    "root-type",
                    "base-root-type",
                    "include",
                    "typedef-type",
                    "paragraph",
                    "sentence",
                    "sentence-with-semi-colon",
                    "value-without-bracket",
                    "base-value-without-bracket",
                    "variable-name",
                    "for-increase",
                    "base-for-increase",
                    "unary-operation-value"
                ).
                with_of_all_childrens(
                    "paramtypelist",
                    "raw-value",
                    "paramlist",
                    "value-with-operation"
                ).
                with(code_gen.of_ignore_last_child(Of logic_writer)("root-type-with-semi-colon")).
                with(code_gen.of_ignore_last_child(Of logic_writer)("base-sentence-with-semi-colon")).
                with(code_gen.of_ignore_last_child(Of logic_writer)("b2style-sentence-with-semi-colon")).
                with(code_gen.of_first_child(Of logic_writer)("param-with-comma")).
                with(code_gen.of_first_child(Of logic_writer)("value-with-comma")).
                with(code_gen.of_first_child(Of logic_writer)("paramtype-with-comma")).
                with(code_gen.of_children(Of logic_writer)("else-condition", 1)).
                with(code_gen.of_children(Of logic_writer)("value-with-bracket", 1)).
                with(code_gen.of_input_without_ignored(Of logic_writer)("paramtype")).
 _
                with(Of bstyle.logic)().
                with(Of bstyle.typedef_type_name)().
                with(Of bstyle.typedef_type_str)().
 _
                with(Of include_with_file)().
                with(Of include_with_string)().
                with(scope.define_t.code_gens.ifdef_wrapped(AddressOf code_gen_of)).
                with(scope.define_t.code_gens.ifndef_wrapped(AddressOf code_gen_of)).
                with(scope.define_t.code_gens.define()).
 _
                with(Of biguint)().
                with(Of bool)().
                with(Of _integer)().
                with(Of _string)().
                with(Of ufloat)().
                with(Of _function)().
                with(Of function_call)().
                with_delegate("ignore-result-function-call",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  Return function_call.without_return(n.child(), o)
                              End Function).
                with(Of param)().
                with(Of return_clause)().
                with(Of value_clause)().
                with(Of value_declaration)().
                with(Of heap_declaration)().
                with(Of value_definition)().
                with(Of heap_name)().
                with(Of struct)().
                without(Of raw_variable_name)().
                with(Of value_list)().
                with_delegate("typedef",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  assert(n.child_count() = 3)
                                  Return scope.current().type_alias().define(
                                             code_gen_of(n.child(2)).dump(),
                                             code_gen_of(n.child(1)).dump())
                              End Function).
                with(Of static_cast)().
                with(Of multi_sentence_paragraph)().
                with_delegate("value",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  assert(strsame(n.type_name, "value") OrElse
                                         (strsame(n.type_name, "ignore-result-function-call") AndAlso
                                          strsame(n.child().type_name, "function-call")))
                                  Return code_gen_of(n.child()).build(o)
                              End Function).
                with_delegate("kw-file",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  Return _string.build(parse_wrapper.current_file(), o)
                              End Function).
                with_delegate("kw-func",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  Return _string.build(scope.current().current_function().signature(), o)
                              End Function).
                with_delegate("kw-line",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  Return _integer.build(CInt(n.char_start()), o)
                              End Function).
                with_delegate("kw-statement",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  Return _string.build(n.ancestor_of("sentence").input(), o)
                              End Function).
                with(Of condition)().
                with(Of for_loop)().
                with(Of _while)().
                with(Of _delegate)().
                with(Of reinterpret_cast)().
                with_delegate("dealloc",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  assert(n.child_count() = 4)
                                  Dim name As String = n.child(2).input_without_ignored()
                                  Return struct.dealloc_from_heap(name, o) OrElse
                                         builders.of_dealloc_heap(name).to(o)
                              End Function).
                with_delegate("undefine",
                              Function(ByVal n As typed_node, ByVal o As logic_writer) As Boolean
                                  assert(Not n Is Nothing)
                                  assert(Not o Is Nothing)
                                  assert(n.child_count() = 4)
                                  Dim name As String = n.child(2).input_without_ignored()
                                  Return struct.undefine(name, o) OrElse
                                         (builders.of_undefine(name).to(o) AndAlso
                                          scope.current().variables().undefine(name))
                              End Function).
 _
                with(Of name)().
                with(Of _namespace)().
                with(Of binary_operation_value)().
                with("pre-operation-value", New unary_operation_value(0, "_pre")).
                with("post-operation-value", New unary_operation_value(1, "_post")).
                with_delegate("self-value-clause", AddressOf binary_operation_value.without_return)
        End Function
    End Class
End Class