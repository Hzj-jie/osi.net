
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

Public MustInherit Class bstyle_test_runner
    Protected MustOverride Function parse(ByVal io As console_io.test_wrapper,
                                          ByVal content As String,
                                          ByRef o As executor) As Boolean

    <test>
    Private Sub case1()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.case1.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "Hello World")
    End Sub

    <test>
    Private Sub case2()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.case2.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), strncat("", "False", 100))
    End Sub

    <test>
    Private Sub global_variable()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.global_variable.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "TrueFalse")
    End Sub

    <test>
    Private Sub overload_function()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.overload_function.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "TrueFalseFalseTrue")
    End Sub

    <test>
    Private Sub single_level_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.single_level_struct.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "dabc")
    End Sub

    <test>
    Private Sub nested_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.nested_struct.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "dd")
    End Sub

    <test>
    Private Sub function_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.function_struct.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abcdef")
    End Sub

    <test>
    Private Sub return_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.return_struct.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abcdef")
    End Sub

    <test>
    Private Sub call_struct_on_heap()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.call_struct_on_heap.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "abcd")
    End Sub

    <test>
    Private Sub for_loop()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.for_loop.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), character.newline)
    End Sub

    <test>
    Private Sub empty_struct_overloads()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.empty_struct_overloads.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "DE")
    End Sub

    <test>
    Private Sub delegate_()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.[delegate].as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "fg")
    End Sub

    <test>
    Private Sub statement()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _bstyle_test_data.statement.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "std_out ( __STATEMENT__ ) ;string s = __STATEMENT__ ;")
    End Sub

    Protected Sub New()
    End Sub
End Class

<test>
Public NotInheritable Class bstyle_test
    Inherits bstyle_test_runner

    Protected Overrides Function parse(ByVal io As console_io.test_wrapper,
                                       ByVal content As String,
                                       ByRef o As executor) As Boolean
        Return bstyle.with_functions(New interrupts(+io)).compile(content, o)
    End Function

    <test>
    Private Sub nlp_parsable()
        assertion.is_true(nlp.of_file(bstyle.nlexer_rule, bstyle.syntaxer_rule, Nothing))
    End Sub

    <test>
    Private Sub real__file__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        Using bstyle.parse_wrapper.with_current_file("real__file__.txt")
            assertion.is_true(bstyle.with_functions(New interrupts(+io)).
                                     compile(_bstyle_test_data.real__file__.as_text(), e))
        End Using
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output(), "real__file__.txt")
    End Sub

    <test>
    Private Sub predefined_def()
        Dim e As executor = Nothing
        assertion.is_true(bstyle.with_default_functions().compile(_bstyle_test_data.predefined_def.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
    End Sub

    ' b3style treat f.f() as f(f) / class function.
    <test>
    Private Sub func_name_with_dot()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(bstyle.with_functions(New interrupts(+io)).
                                 compile(_bstyle_test_data.func_name_with_dot.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output().Length(), 2)
        assertion.equal(Convert.ToInt32(io.output()(0)), 1)
        assertion.equal(Convert.ToInt32(io.output()(1)), 2)
    End Sub

    Private Sub New()
    End Sub
End Class
