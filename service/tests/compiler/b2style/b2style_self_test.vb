
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

Public MustInherit Class b2style_self_test_runner
    Inherits compiler_self_test_runner

    Public Const total_assertions As String = "Total assertions: "
    Public Const failure As String = "Failure: "
    Public Const success As String = "Success: "
    Public Const no_extra_inforamtion As String = "no extra information."
    Private Shared ReadOnly ignored_test As unordered_set(Of String) = unordered_set.of(
        "delegate_in_class.txt",
        "delegate_in_class_on_heap.txt")

    Public Sub New()
        MyBase.New(b2style_self_test_cases.data)
    End Sub

    <test>
    Private Shadows Sub run()
        MyBase.run()
    End Sub

    Protected NotOverridable Overrides Sub execute(ByVal name As String, ByVal content As String)
        If ignored_test.find(name) <> ignored_test.end() Then
            Return
        End If
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(New interrupts(+io), content, e), name)
        assertion.is_not_null(e, name)
        e.assert_execute_without_errors(name)
        Dim v As vector(Of String) = streams.of(io.output().Trim().Split(character.newline)).
                                             collect_to(Of vector(Of String))()
        If Not assertions.of(v).not_empty(name) Then
            Return
        End If
        If Not assertions.of(v.back()).starts_with(total_assertions, name) Then
            Return
        End If
        Dim exp_assertions As UInt32 = 0
        If Not assertion.is_true(UInt32.TryParse(v.back().Substring(total_assertions.Length()), exp_assertions),
                                 name) Then
            Return
        End If
        assert(v.size() >= uint32_1)
        assertion.equal(exp_assertions, v.size() - uint32_1, name)
        Dim i As UInt32 = 0
        While i < v.size() - uint32_1
            assertions.of(v(i)).starts_with(success, name, "@", i)
            i += uint32_1
        End While
    End Sub

    Protected MustOverride Function parse(ByVal functions As interrupts,
                                          ByVal content As String,
                                          ByRef e As executor) As Boolean
End Class

<test>
Public NotInheritable Class b2style_self_test
    Inherits b2style_self_test_runner

    Protected Overrides Function parse(ByVal functions As interrupts,
                                       ByVal content As String,
                                       ByRef e As executor) As Boolean
        Return b2style.with_functions(functions).compile(content, e)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper.with_current_file(filename)
    End Function

    Protected Overrides Function ignore_case(ByVal name As String) As Boolean
        assert(Not name.null_or_whitespace())
        Return name.Equals("struct-and-primitive-type-with-same-name.txt")
    End Function
End Class
