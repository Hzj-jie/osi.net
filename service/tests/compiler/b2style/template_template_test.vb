
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
Imports osi.service.compiler.b2style
Imports osi.service.resource
Imports template_template = osi.service.compiler.b2style.template_template

<test>
Public NotInheritable Class template_template_test
    <test>
    Private Shared Sub case1()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.template_template_case1.as_text(), root:=n))
        assertion.is_not_null(n)
        Dim t As template_template = Nothing
        assertion.is_true(template_template.of(b2style.new_code_gens(), n.child().child(), t))
        Dim impl As String = Nothing
        assertion.is_true(t.apply(vector.of("int"), impl))
        assertion.equal(impl, "class C__1__int { int x ; void f ( int y ) { } } ;")
    End Sub

    <test>
    Private Shared Sub mismatch_type_count()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.template_template_case1.as_text(), root:=n))
        assertion.is_not_null(n)
        Dim t As template_template = Nothing
        assertion.is_true(template_template.of(b2style.new_code_gens(), n.child().child(), t))
        assertion.is_false(t.apply(vector.of("a", "b"), Nothing))
    End Sub

    <test>
    Private Shared Sub disallow_duplicated_template_type_parameters()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.errors_duplicate_template_type_parameters.as_text(),
                                              root:=n))
        assertion.is_not_null(n)
        assertions.of(error_event.capture_log(error_type.user,
                                              Sub()
                                                  assertion.is_false(template_template.of(b2style.new_code_gens(),
                                                                                          n.child().child(),
                                                                                          Nothing))
                                              End Sub)).
                      contains(vector.of("T", "T").ToString())
    End Sub

    <test>
    Private Shared Sub two_template_type_params()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.two_template_type_parameters.as_text(), root:=n))
        assertion.is_not_null(n)
        Dim t As template_template = Nothing
        assertion.is_true(template_template.of(b2style.new_code_gens(), n.child().child(), t))
        Dim impl As String = Nothing
        assertion.is_true(t.apply(vector.of("int", "string"), impl))
        assertion.equal(impl, "class C__2__int__string { int x ; string y ; void p ( int x , string y ) { } } ;")
    End Sub

    Private Sub New()
    End Sub
End Class
