
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Public MustInherit Class compiler_self_compile_error_test_runner
    Inherits compiler_self_test_runner

    Private Const error_prefix As String = "// ERROR: "
    Private Const error_pattern_prefix As String = "// ERROR PATTERN: "

    Protected Sub New(ByVal d() As Byte)
        MyBase.New(d)
    End Sub

    Protected NotOverridable Overrides Sub execute(ByVal name As String, ByVal content As String)
        Dim err As String = error_event.capture_log(error_type.user,
                                                    Sub()
                                                        assertion.is_false(parse(content), name)
                                                    End Sub)
        Dim find_all_errors As Func(Of String, stream(Of String)) =
               Function(ByVal prefix As String) As stream(Of String)
                   assert(Not prefix.null_or_whitespace())
                   Return streams.of(content.Split(character.newline)).
                                                   map(Function(ByVal s As String) As String
                                                           If s Is Nothing Then
                                                               Return ""
                                                           End If
                                                           Return s.Trim()
                                                       End Function).
                                                   filter(Function(ByVal s As String) As Boolean
                                                              Return s.StartsWith(prefix)
                                                          End Function).
                                                   map(Function(ByVal s As String) As String
                                                           Return s.Substring(prefix.Length())
                                                       End Function)
               End Function
        find_all_errors(error_prefix).foreach(Sub(ByVal exp_err As String)
                                                  assertions.of(err).contains(exp_err)
                                              End Sub)
        find_all_errors(error_pattern_prefix).foreach(Sub(ByVal exp_err_pattern As String)
                                                          assertions.of(err).match_pattern(exp_err_pattern)
                                                      End Sub)
    End Sub

End Class