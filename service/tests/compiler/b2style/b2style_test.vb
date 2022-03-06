
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
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
        assertion.is_true(nlp.of_file(b2style.nlexer_rule, b2style.syntaxer_rule, Nothing))
    End Sub

    <test>
    Private Shared Sub case1()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.case1.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "Hello World")
    End Sub

    <test>
    Private Shared Sub case2()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.case2.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strncat("", "False", 100))
    End Sub

    <test>
    Private Shared Sub bool_and_bool()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.bool_and_bool.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "TrueFalseFalseFalseTrueTrueTrueFalse")
    End Sub

    <test>
    Private Shared Sub str_unescape()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.str_unescape.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("abc", character.tab, "def", character.newline))
    End Sub

    <test>
    Private Shared Sub _1_to_100()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data._1_to_100.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("5050", character.newline))
    End Sub

    <test>
    Private Shared Sub self_add()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.self_add.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "101101102")
    End Sub

    <test>
    Private Shared Sub biguint()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(_b2style_test_data.biguint.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "429496729642949672961")
    End Sub

    <test>
    Private Shared Sub negative_int()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.negative_int.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "-1-2-3")
    End Sub

    <test>
    Private Shared Sub another_1_to_100()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.another_1_to_100.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("5050", character.newline))
    End Sub

    <test>
    Private Shared Sub loaded_method()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.loaded_method.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strncat("", "False", 100))
    End Sub

    <test>
    Private Shared Sub ufloat_std_out()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.ufloat_std_out.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "1.1")
    End Sub

    <test>
    Private Shared Sub ufloat_operators()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.ufloat_operators.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "1.10.5")
    End Sub

    <test>
    Private Shared Sub while_1_to_100()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.while_1_to_100.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("5050", character.newline))
    End Sub

    <test>
    Private Shared Sub while_0_to_1()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.while_0_to_1.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("50.5", character.newline))
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub pi_integral_0_1()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.pi_integral_0_1.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("3.304518326248318338508394330371205830216509246678390049350471115"))
        '                                    3.14159265
    End Sub

    <test>
    Private Shared Sub shift()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.shift.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "40040012")
    End Sub

    <test>
    Private Shared Sub calculate_pi_bbp()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.calculate_pi_bbp.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "3.1415926535897932384626433832795028841971693993751058209749445923")
        '                             3.1415926535897932384626433832795028841971693993751058209749445923
    End Sub

    <test>
    Private Shared Sub order_of_operators()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.order_of_operators.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "15")
        ' TODO:
        ' Expected:
        ' 1 + 6 + 4 - 30912
        ' =-30901
        ' Actually:
        ' 1 + (2 * (3 + (4 - (5 * 6 * 7 * 8 * ( 9 / ( 10 * (11 + 12)))))))
        ' =1 + (2 * (3 + 4))
        ' =15
        ' But it may not be handled by b2style.
    End Sub

    <test>
    Private Shared Sub include()
        Dim io As New console_io.test_wrapper("abc")
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.include.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abcEOF")
    End Sub

    <test>
    Private Shared Sub include2()
        Dim io As New console_io.test_wrapper("def")
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.include2.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "defEoF")
    End Sub

    <test>
    Private Shared Sub ifndef()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.ifndef.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "good")
    End Sub

    <test>
    Private Shared Sub namespaces()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.namespaces.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("a::b::f1", character.newline,
                                            "a::f2", character.newline,
                                            "a::b::f2", character.newline,
                                            "a::f3", character.newline,
                                            "a::c::f4", character.newline))
    End Sub

    <test>
    Private Shared Sub multiline_string()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.multiline_string.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        ' I hate different newline characters.
        assertion.equal(io.output().Replace(newline.incode(), character.newline),
                        strcat("a", character.newline,
                               "    b", character.newline,
                               "    c", character.newline,
                               "    d"))
    End Sub

    <test>
    Private Shared Sub comments()
        Dim io As New console_io.test_wrapper("good")
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.comments.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "good")
    End Sub

    <test>
    Private Shared Sub typedef()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                          parse(_b2style_test_data.typedef.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abc")
    End Sub

    <test>
    Private Shared Sub legacy_biguint_to_str()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.legacy_biguint_to_str.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "4294967296429496729610")
    End Sub

    <test>
    Private Shared Sub struct_in_namespace()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.struct_in_namespace.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "100")
    End Sub

    <test>
    Private Shared Sub heap_declaration()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.heap_declaration.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
    End Sub

    <test>
    Private Shared Sub heap()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.heap.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abcdefghi100")
    End Sub

    <test>
    Private Shared Sub function_ref()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.function_ref.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "12345677")
    End Sub

    <test>
    Private Shared Sub struct_function_ref()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.struct_function_ref.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "100abc")
    End Sub

    <test>
    Private Shared Sub __i__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.__i__.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "3_8_19_20_21_22")
    End Sub

    <test>
    Private Shared Sub for_loop()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.for_loop.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "495099001485019800")
    End Sub

    <test>
    Private Shared Sub i__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.i__.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(Convert.ToInt32(io.output()(0)), 1)
    End Sub

    <test>
    Private Shared Sub i__2()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.i__2.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(Convert.ToInt32(io.output()(0)), 1)
        assertion.equal(Convert.ToInt32(io.output()(1)), 2)
        assertion.equal(Convert.ToInt32(io.output()(2)), 3)
        assertion.equal(Convert.ToInt32(io.output()(3)), 4)
    End Sub

    <test>
    Private Shared Sub __i()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.__i.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(Convert.ToInt32(io.output()(0)), 1)
        assertion.equal(Convert.ToInt32(io.output()(1)), 2)
        assertion.equal(Convert.ToInt32(io.output()(2)), 3)
        assertion.equal(Convert.ToInt32(io.output()(3)), 4)
    End Sub

    <test>
    Private Shared Sub heap_function_ref()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.heap_function_ref.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(),
                        streams.range_closed(1, 100).
                                map(Function(ByVal x As Int32) As String
                                        Return strcat(x, character.newline)
                                    End Function).
                                collect_by(stream(Of String).collectors.to_str("")).
                                ToString())
    End Sub

    <test>
    Private Shared Sub nested_paragraph()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.nested_paragraph.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strcat("hello world",
                                            character.newline,
                                            "hello again world",
                                            character.newline,
                                            "hello again again world"))
    End Sub

    <test>
    Private Shared Sub [class]()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.[class].as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "200100")
    End Sub

    <test>
    Private Shared Sub nested_class()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.nested_class.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "100200")
    End Sub

    <test>
    Private Shared Sub class_on_heap()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.class_on_heap.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), streams.range(0, 100).
                                             map(Function(ByVal x As Int32) As String
                                                     Return strcat(x, " ", x, " ", x)
                                                 End Function).
                                             collect_by(stream(Of String).collectors.to_str(character.newline)).
                                             Append(character.newline).
                                             ToString())
    End Sub

    <test>
    Private Shared Sub class_in_namespace()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.class_in_namespace.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "123")
    End Sub

    <test>
    Private Shared Sub function_with_global_namespace()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.function_with_global_namespace.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "f::f")
    End Sub

    <test>
    Private Shared Sub class_function_with_namespace()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.class_function_with_namespace.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "100200")
    End Sub

    <test>
    Private Shared Sub nested_heap_access()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.nested_heap_access.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "234")
    End Sub

    <test>
    Private Shared Sub heap_ptr_to_int64()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.heap_ptr_to_int64.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "0 4294967296 8589934592")
    End Sub

    <test>
    Private Shared Sub unused_functions_should_be_removed()
        Dim bstyle_str As String = Nothing
        assertion.is_true(b2style.parse(_b2style_test_data.case1.as_text(), bstyle_str))
        ' b2style does not handle overload itself, so b2style::std_out(ufloat) and b2style::ufloat_to_str are included
        ' in the output.
        assertions.of(bstyle_str).not_contains("b2style__ufloat__from")
        Dim logic_str As String = Nothing
        assertion.is_true(bstyle.parse(bstyle_str, logic_str))
        assertions.of(logic_str).not_contains("b2style__ufloat")
    End Sub

    <test>
    Private Shared Sub empty_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.empty_struct.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "f3ff2")
    End Sub

    <test>
    Private Shared Sub template()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.template.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "DE")
    End Sub

    <test>
    Private Shared Sub primitive_template()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.primitive_template.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "101101.11")
    End Sub

    <test>
    Private Shared Sub nested_template()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.nested_template.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "C")
    End Sub

    <test>
    Private Shared Sub template_wont_be_extended_twice()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.template_wont_be_extended_twice.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "24")
    End Sub

    <test>
    Private Shared Sub template_with_different_length()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.template_with_different_length.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "12abc")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.reinterpret_cast.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "-100100")
    End Sub

    <test>
    Private Shared Sub reinterpret_cast_heap()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.reinterpret_cast_heap.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "-1010")
    End Sub

    <test>
    Private Shared Sub lots_of_semi_colons()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.lots_of_semi_colons.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "100")
    End Sub

    <test>
    Private Shared Sub delegate_()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.[delegate].as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abc")
    End Sub

    <test>
    Private Shared Sub delegate2()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.delegate2.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "31")
    End Sub

    <test>
    Private Shared Sub delegate_template()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.delegate_template.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "2abc1")
    End Sub

    <test>
    Private Shared Sub function_ptr()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.function_ptr.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "10199")
    End Sub

    <test>
    Private Shared Sub delegate_ref()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.delegate_ref.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "100101")
    End Sub

    <test>
    Private Shared Sub class_constructor()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.class_constructor.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "10012002")
    End Sub

    <test>
    Private Shared Sub test_assert()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.test_assert.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertions.of(io.output().Split(character.newline)).equal(
            b2style_self_test.failure + b2style_self_test.no_extra_inforamtion,
            b2style_self_test.success + b2style_self_test.no_extra_inforamtion,
            b2style_self_test.total_assertions + "2",
            "")
    End Sub

    <test>
    Private Shared Sub class_inheritance()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.class_inheritance.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "3f2")
    End Sub

    <test>
    Private Shared Sub assert_()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.assert_.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "This assertion should not pass.")
    End Sub

    <test>
    Private Shared Sub vector_destructor()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).
                                  parse(_b2style_test_data.vector_destructor.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.array_equal(assertion.catch_thrown(Of executor_stop_error) _
                                                        (Sub()
                                                             direct_cast(Of simulator)(e).access_heap(0)
                                                         End Sub).error_types,
                              {executor.error_type.heap_access_out_of_boundary})
    End Sub

    Private Sub New()
    End Sub
End Class
