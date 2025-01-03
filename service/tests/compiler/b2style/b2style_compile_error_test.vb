
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

Public Class b2style_compile_error_test(Of _PARSE As __do(Of String, executor, Boolean))
    Private Shared ReadOnly parse As _do(Of String, executor, Boolean) = -(alloc(Of _PARSE)())

    <test>
    Private Shared Sub dollar_in_number()
        run(_b2style_test_data.errors_dollar_in_number, "nlexer", "$")
    End Sub

    <test>
    Private Shared Sub include_needs_wraps()
        run(_b2style_test_data.errors_include_needs_wraps,
            syntaxer.debug_str("abc", "raw-name"),
            syntaxer.debug_str(".", "dot"),
            syntaxer.debug_str("h", "raw-name"))
    End Sub

    <test>
    Private Shared Sub three_pluses()
        ' TODO: It's possible to move the longest match to the "i++ >>> + <<<" if the syntaxer_rule does not consider
        ' "i++;" but "i++" to be a sentence.
        run(_b2style_test_data.errors_three_pluses,
            syntaxer.debug_str("void", "raw-name"),
            syntaxer.debug_str("main", "raw-name"),
            syntaxer.debug_str("i", "raw-name"),
            syntaxer.debug_str("++", "self-inc"),
            syntaxer.debug_str("+", "add"))
    End Sub

    <test>
    Private Shared Sub value_clause_struct_type_mismatch()
        run(_b2style_test_data.errors_value_clause_struct_type_mismatch, "S2", "s", "S1")
    End Sub

    <test>
    Private Shared Sub value_clause_struct_type_mismatch2()
        run(_b2style_test_data.errors_value_clause_struct_type_mismatch2, "S2", "s", "S1")
    End Sub

    <test>
    Private Shared Sub function_return_struct_type_mismatch()
        run(_b2style_test_data.errors_function_return_struct_type_mismatch, "S2", "f", "S1")
    End Sub

    <test>
    Private Shared Sub function_name_ends_with_dot()
        run(_b2style_test_data.errors_function_name_ends_with_dot,
            syntaxer.debug_str("c", "raw-name"),
            syntaxer.debug_str(".", "dot"))
    End Sub

    <test>
    Private Shared Sub missing_ending_quota()
        run(_b2style_test_data.errors_missing_ending_quota, "[nlexer]", """")
    End Sub

    <test>
    Private Shared Sub undefined_value_clause()
        run(_b2style_test_data.errors_undefined_value_clause, "this_is_an_undefined_value_clause")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_unknown_variable()
        run(_b2style_test_data.errors_reinterpret_cast_unknown_variable, "this_is_an_unknown_variable")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_unknown_type()
        run(_b2style_test_data.errors_reinterpret_cast_unknown_type, "this_is_an_unknown_type")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_heap_with_index()
        run(_b2style_test_data.errors_reinterpret_cast_heap_with_index, "s[0]")
    End Sub

    <test>
    Private Shared Sub template_without_type_parameter()
        run(_b2style_test_data.errors_template_without_type_parameter, "ThisTemplateHasNoTypeParameter")
    End Sub

    <test>
    Private Shared Sub class_initializer_for_non_class()
        run(_b2style_test_data.errors_class_initializer_for_non_class, "ooops_this_is_not_a_class")
    End Sub

    <test>
    Private Shared Sub duplicate_template_type_paramters()
        run(_b2style_test_data.errors_duplicate_template_type_parameters, "T, T")
    End Sub

    Protected Shared Sub run(ByVal d() As Byte, ParamArray ByVal errs() As String)
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(parse(d.as_text(), Nothing))
            End Sub)).contains(errs)
    End Sub

    Protected Sub New()
    End Sub
End Class

<test>
Public NotInheritable Class b2style_compile_error_test
    Inherits b2style_compile_error_test(Of parse)

    Public NotInheritable Class parse
        Inherits __do(Of String, executor, Boolean)

        Public Overrides Function at(ByRef j As String, ByRef k As executor) As Boolean
            Return b2style.with_default_functions().compile(j, k)
        End Function
    End Class

    <test>
    Private Shared Sub cycle_typedef()
        run(_b2style_test_data.errors_cycle_typedef, "CYCLE_TYPEDEF__A", "CYCLE_TYPEDEF__C")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_without_type_id()
        run(_b2style_test_data.errors_reinterpret_cast_without_type_id, "s.S2__struct__type__id")
    End Sub

    Private Sub New()
    End Sub
End Class
