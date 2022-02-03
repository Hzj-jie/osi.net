
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler

<test>
Public NotInheritable Class b2style_self_compile_error_test
    Inherits b2style_self_test_runner

    Private Const error_prefix As String = "// ERROR: "
    Private Shared filter As argument(Of String)

    Public Sub New()
        MyBase.New(filter Or "*", b2style_self_compile_error_cases.data)
    End Sub

    <test>
    Private Shadows Sub run()
        MyBase.run()
    End Sub

    Protected Overrides Sub execute(ByVal name As String, ByVal content As String)
        Dim err As String = error_event.capture_log(error_type.user,
                                                    Sub()
                                                        assertion.is_false(b2style.with_default_functions.
                                                                                   parse(content, Nothing))
                                                    End Sub)
        streams.of(content.Split(character.newline)).
                map(Function(ByVal s As String) As String
                        If s Is Nothing Then
                            Return ""
                        End If
                        Return s.Trim()
                    End Function).
                filter(Function(ByVal s As String) As Boolean
                           Return s.StartsWith(error_prefix)
                       End Function).
                map(Function(ByVal s As String) As String
                        Return s.Substring(error_prefix.Length())
                    End Function).
                foreach(Sub(ByVal exp_err As String)
                            assertions.of(err).contains(exp_err)
                        End Sub)
    End Sub
End Class
