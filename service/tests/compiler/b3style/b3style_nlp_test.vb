
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
Public NotInheritable Class b3style_nlp_tests
    <test>
    Public NotInheritable Class non_self_tests
        ' These compile errors are coming from compiler implementation but not the nlexer or syntaxer, nlp should be
        ' able to handle them correctly.
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

        Private Shared Sub run_tests(ByVal t As Type)
            For Each v As FieldInfo In t.GetFields(binding_flags.static_public)
                Dim s As String = direct_cast(Of Byte())(v.GetValue(Nothing)).as_text()
                If v.Name().StartsWith("errors_") AndAlso
                   ignored_compile_errors.find(v.Name()) = ignored_compile_errors.end() Then
                    assertion.is_false(b3style.nlp().parse(s), v.full_name())
                Else
                    assertion.is_true(b3style.nlp().parse(s), v.full_name())
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

    <test>
    Public NotInheritable Class b2style_self
        Inherits compiler_self_test_runner

        Public Sub New()
            MyBase.New("*", b2style_self_test_cases.data)
        End Sub

        Protected Overrides Sub execute(ByVal name As String, ByVal text As String)
            assertion.is_true(b3style.nlp().parse(text), name)
        End Sub

        Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
            Return b3style.parse_wrapper.with_current_file(filename)
        End Function

        <test>
        Private Shadows Sub run()
            MyBase.run()
        End Sub
    End Class

    <test>
    Public NotInheritable Class b2style_self_compile
        Inherits compiler_self_test_runner

        ' These compile errors are coming from compiler implementation but not the nlexer or syntaxer, nlp should be
        ' able to handle them correctly.
        Private Shared ReadOnly ignored_compile_errors As unordered_set(Of String) = unordered_set.emplace_of(
            "copy-struct-to-primitive.txt",
            "copy-primitive-to-struct.txt",
            "duplicate-var-name-in-inherited-class.txt",
            "duplicate-func-name-in-class.txt",
            "struct-with-myself.txt",
            "templates-with-same-name.txt",
            "type-parameters-with-same-name.txt"
        )

        Public Sub New()
            MyBase.New("*", b2style_self_compile_error_cases.data)
        End Sub

        Protected Overrides Sub execute(ByVal name As String, ByVal text As String)
            If ignored_compile_errors.find(name) = ignored_compile_errors.end() Then
                assertion.is_false(b3style.nlp().parse(text), name)
            Else
                assertion.is_true(b3style.nlp().parse(text), name)
            End If
        End Sub

        Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
            Return b3style.parse_wrapper.with_current_file(filename)
        End Function

        <test>
        Private Shadows Sub run()
            MyBase.run()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
