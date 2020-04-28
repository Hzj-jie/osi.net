
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
Public NotInheritable Class b2style_test
    <test>
    Private Shared Sub nlp_parsable()
        assertion.is_true(nlp.of(b2style.nlexer_rule, b2style.syntaxer_rule, Nothing))
    End Sub

    <test>
    Private Shared Sub case1()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.case1.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), "Hello World")
    End Sub

    <test>
    Private Shared Sub case2()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.case2.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strncat("", "False", 100))
    End Sub

    <test>
    Private Shared Sub bool_and_bool()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.bool_and_bool.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), "TrueFalseFalseFalseTrueTrueTrueFalse")
    End Sub

    <test>
    Private Shared Sub str_unescape()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.str_unescape.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("abc", character.tab, "def", character.newline))
    End Sub

    <test>
    Private Shared Sub _1_to_100()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data._1_to_100.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("5050", character.newline))
    End Sub

    <test>
    Private Shared Sub self_add()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.self_add.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("101"))
    End Sub

    <test>
    Private Shared Sub biguint()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.biguint.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("429496729642949672961"))
    End Sub

    <test>
    Private Shared Sub negative_int()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.negative_int.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("-1-2-3"))
    End Sub

    <test>
    Private Shared Sub another_1_to_100()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.another_1_to_100.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("5050", character.newline))
    End Sub

    <test>
    Private Shared Sub loaded_method()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.loaded_method.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strncat("", "False", 100))
    End Sub

    <test>
    Private Shared Sub ufloat_std_out()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.ufloat_std_out.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), "1.1")
    End Sub

    <test>
    Private Shared Sub ufloat_operators()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.ufloat_operators.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), "1.10.5")
    End Sub

    <test>
    Private Shared Sub while_1_to_100()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.while_1_to_100.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("5050", character.newline))
    End Sub

    <test>
    Private Shared Sub while_0_to_1()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.while_0_to_1.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("50.5", character.newline))
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub pi_integral_0_1()
        Dim io As console_io.test_wrapper = Nothing
        io = New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.pi_integral_0_1.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), strcat("3.304518326248318338508394330371205830216509246678390049350471115"))
        '                                    3.14159265
    End Sub

    Private Sub New()
    End Sub
End Class
