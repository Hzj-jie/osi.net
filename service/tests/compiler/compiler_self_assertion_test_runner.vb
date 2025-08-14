
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Public MustInherit Class compiler_self_assertion_test_runner
    Inherits compiler_self_test_runner

    Public Const total_assertions As String = "Total assertions: "
    Public Const failure As String = "Failure: "
    Public Const success As String = "Success: "
    Public Const no_extra_inforamtion As String = "no extra information."

    Protected Sub New(ByVal d() As Byte)
        MyBase.New(d)
    End Sub

    Protected NotOverridable Overrides Sub execute(ByVal name As String, ByVal content As String)
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
