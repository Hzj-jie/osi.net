
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.resource

<test>
Public NotInheritable Class bstyle_compiler_error_test
    <test>
    Private Shared Sub case1()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(bstyle.with_default_functions().
                                          parse(_bstyle_test_data.errors_dot_is_disallowed_as_the_end_of_name.as_text(),
                                                Nothing))
            End Sub)).contains("[syntaxer]",
                               syntaxer.debug_str("x", "raw-name"),
                               syntaxer.debug_str(".", "dot"))
    End Sub

    Private Sub New()
    End Sub
End Class
