﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.resource

<test>
Public NotInheritable Class b2style_compile_error_test
    <test>
    Private Shared Sub dollar_in_number()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_default_functions().
                                           parse(_b2style_test_data.errors_dollar_in_number.as_text(), Nothing))
            End Sub)).contains("nlexer", "$")
    End Sub

    <test>
    Private Shared Sub include_needs_wraps()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_default_functions.
                                           parse(_b2style_test_data.errors_include_needs_wraps.as_text(), Nothing))
            End Sub)).contains(syntaxer.debug_str("abc", "raw-name"),
                               syntaxer.debug_str(".", "dot"),
                               syntaxer.debug_str("h", "raw-name"))
    End Sub

    <test>
    Private Shared Sub three_pluses()
        ' TODO: It's possible to move the longest match to the "i++ >>> + <<<" if the syntaxer_rule does not consider
        ' "i++;" but "i++" to be a sentence.
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_default_functions.
                                           parse(_b2style_test_data.errors_three_pluses.as_text(), Nothing))
            End Sub)).contains(syntaxer.debug_str("void", "raw-name"),
                               syntaxer.debug_str("main", "raw-name"),
                               syntaxer.debug_str("i", "raw-name"),
                               syntaxer.debug_str("++", "self-inc"),
                               syntaxer.debug_str("+", "add"))
    End Sub

    <test>
    Private Shared Sub cycle_typedef()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_default_functions.
                                           parse(_b2style_test_data.errors_cycle_typedef.as_text(), Nothing))
            End Sub)).contains("CYCLE_TYPEDEF__A", "CYCLE_TYPEDEF__C")
    End Sub

    <test>
    Private Shared Sub value_clause_struct_type_mismatch()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_value_clause_struct_type_mismatch.as_text(), Nothing))
            End Sub)).contains("S2", "s", "S1")
    End Sub

    <test>
    Private Shared Sub value_clause_struct_type_mismatch2()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_value_clause_struct_type_mismatch2.as_text(), Nothing))
            End Sub)).contains("S2", "s", "S1")
    End Sub

    <test>
    Private Shared Sub function_return_struct_type_mismatch()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_function_return_struct_type_mismatch.as_text(), Nothing))
            End Sub)).contains("S2", "f", "S1")
    End Sub

    <test>
    Private Shared Sub function_name_ends_with_dot()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_function_name_ends_with_dot.as_text(), Nothing))
            End Sub)).contains(syntaxer.debug_str("c", "raw-name"),
                               syntaxer.debug_str(".", "dot"))
    End Sub

    <test>
    Private Shared Sub missing_ending_quota()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_missing_ending_quota.as_text(), Nothing))
            End Sub)).contains("[nlexer]", """")
    End Sub

    <test>
    Private Shared Sub undefined_value_clause()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_undefined_value_clause.as_text(), Nothing))
            End Sub)).contains("this_is_an_undefined_value_clause")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_unknown_variable()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_reinterpret_cast_unknown_variable.as_text(), Nothing))
            End Sub)).contains("this_is_an_unknown_variable")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_unknown_type()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_reinterpret_cast_unknown_type.as_text(), Nothing))
            End Sub)).contains("this_is_an_unknown_type")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_heap_with_index()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_reinterpret_cast_heap_with_index.as_text(), Nothing))
            End Sub)).contains("s[0]")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_without_type_id()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_reinterpret_cast_without_type_id.as_text(), Nothing))
            End Sub)).contains("s.S2__struct__type__id")
    End Sub

    <test>
    Private Shared Sub template_without_type_parameter()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_template_without_type_parameter.as_text(), Nothing))
            End Sub)).contains("ThisTemplateHasNoTypeParameter")
    End Sub

    <test>
    Private Shared Sub class_initializer_for_non_class()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_default_functions.
                    parse(_b2style_test_data.errors_class_initializer_for_non_class.as_text(), Nothing))
            End Sub)).contains("ooops_this_is_not_a_class")
    End Sub

    Private Sub New()
    End Sub
End Class
