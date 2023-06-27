
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
Imports osi.service.interpreter.primitive

Public MustInherit Class b2style_self_compile_error_test_runner
    Inherits compiler_self_test_runner

    Private Const error_prefix As String = "// ERROR: "
    Private Shared filter As argument(Of String)

    Public Sub New()
        MyBase.New(filter Or "*", b2style_self_compile_error_cases.data)
    End Sub

    <test>
    Private Shadows Sub run()
        MyBase.run()
    End Sub

    Protected NotOverridable Overrides Sub execute(ByVal name As String, ByVal content As String)
        Dim err As String = error_event.capture_log(error_type.user,
                                                    Sub()
                                                        assertion.is_false(parse(content, Nothing), name)
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

    Protected MustOverride Function parse(ByVal content As String, ByRef o As executor) As Boolean
End Class

<test>
Public NotInheritable Class b2style_self_compile_error_test
    Inherits b2style_self_compile_error_test_runner

    Protected Overrides Function parse(ByVal content As String, ByRef o As executor) As Boolean
        Return b2style.with_default_functions().parse(content, o)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.parse_wrapper.with_current_file(filename)
    End Function
End Class
