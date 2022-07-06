
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.resource

<test>
Public NotInheritable Class b3style_nlp_test
    Inherits compiler_self_test_runner

    ' These compile errors are coming from compiler implementation but not the nlexer or syntaxer, nlp should be able to
    ' handle them correctly.
    Private Shared ReadOnly ignored_compile_errors As unordered_set(Of String) = unordered_set.emplace_of(
        "errors_class_initializer_for_non_class",
        "errors_cycle_typedef",
        "errors_duplicate_template_type_parameters",
        "errors_function_return_struct_type_mismatch",
        "errors_reinterpret_cast_heap_with_index",
        "errors_reinterpret_cast_unknown_type",
        "errors_reinterpret_cast_unknown_variable",
        "errors_reinterpret_cast_without_type_id",
        "errors_undefined_value_clause",
        "errors_value_clause_struct_type_mismatch",
        "errors_value_clause_struct_type_mismatch2"
    )

    Public Sub New()
        MyBase.New("*", b2style_self_test_cases.data)
    End Sub

    Private Shared Sub run_test(ByVal s As String, ByVal name As String)
        assertion.is_true(b3style.nlp().parse(s), name)
    End Sub

    Protected Overrides Sub execute(ByVal name As String, ByVal text As String)
        run_test(text, name)
    End Sub

    <test>
    Private Shadows Sub run()
        MyBase.run()
    End Sub

    Private Shared Sub run_tests(ByVal t As Type)
        For Each v As FieldInfo In t.GetFields(binding_flags.static_public)
            Dim s As String = direct_cast(Of Byte())(v.GetValue(Nothing)).as_text()
            If v.Name().StartsWith("errors_") AndAlso
               ignored_compile_errors.find(v.Name()) = ignored_compile_errors.end() Then
                assertion.is_false(b3style.nlp().parse(s), v.full_name())
            Else
                run_test(s, v.full_name())
            End If
        Next
    End Sub

    <test>
    Private Shared Sub bstyle_tests()
        run_tests(GetType(_bstyle_test_data))
    End Sub

    <test>
    Private Shared Sub b2style_tests()
        run_tests(GetType(_b2style_test_data))
    End Sub
End Class
