
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler

<test>
Public NotInheritable Class bstyle_compile_error_test
    Inherits compile_error_test_runner

    <test>
    Private Sub errors_dot_is_disallowed_as_the_end_of_name()
        run(_bstyle_test_data.errors_dot_is_disallowed_as_the_end_of_name,
            "[syntaxer]",
            syntaxer.debug_str("x", "raw-name"),
            syntaxer.debug_str(".", "dot"))
    End Sub

    Protected Overrides Function parse(ByVal content As String) As Boolean
        Return bstyle.with_default_functions().compile(content, Nothing)
    End Function

    Private Sub New()
    End Sub
End Class
