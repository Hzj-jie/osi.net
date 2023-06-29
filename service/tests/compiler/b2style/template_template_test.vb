
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

<test>
Public NotInheritable Class template_template_test
    <test>
    Private Shared Sub case1()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.template_template_case1.as_text(), root:=n))
        assertion.is_not_null(n)
        Using New scope()
            Dim t As scope.template_template = Nothing
            assertion.is_true(b2style.scope.template_t.of(b2style.code_gens(), n.child().child(), t))
            Dim impl As String = Nothing
            assertion.is_true(t.apply(vector.of("int"), impl))
            assertion.equal(impl, "class C__int { int x ; void f ( int y ) { } } ;")
        End Using
    End Sub

    ' <test> This becomes an invalid scenario after naming the templates with the number of type parameters.
    Private Shared Sub mismatch_type_count()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.template_template_case1.as_text(), root:=n))
        assertion.is_not_null(n)
        Using New scope()
            Dim t As scope.template_template = Nothing
            assertion.is_true(b2style.scope.template_t.of(b2style.code_gens(), n.child().child(), t))
            assertion.is_false(t.apply(vector.of("a", "b"), Nothing))
        End Using
    End Sub

    <test>
    Private Shared Sub disallow_duplicated_template_type_parameters()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.errors_duplicate_template_type_parameters.as_text(),
                                              root:=n))
        assertion.is_not_null(n)
        assertions.of(error_event.capture_log(
            error_type.user,
            Sub()
                Using New scope()
                    assertion.is_false(b2style.scope.template_t.of(b2style.code_gens(),
                                                                         n.child().child(),
                                                                         Nothing))
                End Using
            End Sub)).
            contains("T, T")
    End Sub

    <test>
    Private Shared Sub two_template_type_params()
        Dim n As typed_node = Nothing
        assertion.is_true(b2style.nlp().parse(_b2style_test_data.two_template_type_parameters.as_text(), root:=n))
        assertion.is_not_null(n)
        Using New scope()
            Dim t As scope.template_template = Nothing
            assertion.is_true(b2style.scope.template_t.of(b2style.code_gens(), n.child().child(), t))
            Dim impl As String = Nothing
            assertion.is_true(t.apply(vector.of("int", "string"), impl))
            assertion.equal(impl, "class C__int__string { int x ; string y ; void p ( int x , string y ) { } } ;")
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
