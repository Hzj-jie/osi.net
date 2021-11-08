
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class b2style_compile_error_test
    <test>
    Private Shared Sub dollar_in_number()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_functions(New interrupts()).
                                           parse(_b2style_test_data.errors_dollar_in_number.as_text(), Nothing))
            End Sub)).contains("nlexer", "$")
    End Sub

    <test>
    Private Shared Sub include_needs_wraps()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_functions(New interrupts()).
                                           parse(_b2style_test_data.errors_include_needs_wraps.as_text(), Nothing))
            End Sub)).contains(syntaxer.debug_str("abc.h", "name"))
    End Sub

    <test>
    Private Shared Sub three_pluses()
        ' TODO: It's possible to move the longest match to the "i++ >>> + <<<" if the syntaxer_rule does not consider
        ' "i++;" but "i++" to be a sentence.
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_functions(New interrupts()).
                                           parse(_b2style_test_data.errors_three_pluses.as_text(), Nothing))
            End Sub)).contains(syntaxer.debug_str("void", "name"),
                               syntaxer.debug_str("main", "name"),
                               syntaxer.debug_str("i", "name"),
                               syntaxer.debug_str("++", "self-inc"),
                               syntaxer.debug_str("+", "add"))
    End Sub

    <test>
    Private Shared Sub cycle_typedef()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.with_functions(New interrupts()).
                                           parse(_b2style_test_data.errors_cycle_typedef.as_text(), Nothing))
            End Sub)).contains("CYCLE_TYPEDEF__A", "CYCLE_TYPEDEF__C")
    End Sub

    <test>
    Private Shared Sub value_clause_struct_type_mismatch()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_functions(New interrupts()).
                    parse(_b2style_test_data.errors_value_clause_struct_type_mismatch.as_text(), Nothing))
            End Sub)).contains("S2", "s", "S1")
    End Sub

    <test>
    Private Shared Sub value_clause_struct_type_mismatch2()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_functions(New interrupts()).
                    parse(_b2style_test_data.errors_value_clause_struct_type_mismatch2.as_text(), Nothing))
            End Sub)).contains("S2", "s", "S1")
    End Sub

    <test>
    Private Shared Sub function_return_struct_type_mismatch()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(b2style.
                    with_functions(New interrupts()).
                    parse(_b2style_test_data.errors_function_return_struct_type_mismatch.as_text(), Nothing))
            End Sub)).contains("S2", "f", "S1")
    End Sub

    Private Sub New()
    End Sub
End Class
