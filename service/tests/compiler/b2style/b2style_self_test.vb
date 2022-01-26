
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class b2style_self_test
    Public Const total_assertions As String = "Total assertions: "
    Public Const failure As String = "Failure: "
    Public Const success As String = "Success: "
    Public Const no_extra_inforamtion As String = "no extra information."
    Private Shared filter As argument(Of String)

    <test>
    Private Shared Sub run()
        Dim c As UInt32 = 0
        tar.gen.reader_of(b2style_self_test_cases.data).foreach(Sub(ByVal name As String,
                                                                    ByVal enc_precision As Double,
                                                                    ByVal content As StreamReader)
                                                                    execute(name, content.ReadToEnd())
                                                                    c += uint32_1
                                                                End Sub)
        assertion.more(c, uint32_0)
    End Sub

    Private Shared Sub execute(ByVal name As String, ByVal content As String)
        If Not name.match_pattern(filter Or "*") AndAlso
           Not name.match_pattern((filter Or "*") + ".txt") Then
            raise_error(error_type.user, "Ignore test case ", name)
            Return
        End If
        raise_error(error_type.user, "Execute test case ", name)
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(+io)).parse(content, e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        Dim v As vector(Of String) = Nothing
        If Not assertion.is_true(io.output().strsplit(New String() {character.newline}, New String() {}, v), name) Then
            Return
        End If
        assert(Not v Is Nothing)
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
            assertions.of(v(i)).starts_with(success, name)
            i += uint32_1
        End While
    End Sub
End Class
