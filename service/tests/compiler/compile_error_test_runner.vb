
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.resource

Public MustInherit Class compile_error_test_runner
    Protected Sub run(ByVal d() As Byte, ParamArray ByVal errs() As String)
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(parse(d.as_text()))
            End Sub)).contains(errs)
    End Sub

    Protected MustOverride Function parse(ByVal content As String) As Boolean

    Protected Sub New()
    End Sub
End Class