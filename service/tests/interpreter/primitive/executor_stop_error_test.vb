
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.interpreter.primitive

Namespace logic
    <test>
    Public NotInheritable Class executor_stop_error_test
        <test>
        Private Shared Sub to_string()
            assertions.of(New executor_stop_error(executor.error_type.instruction_ref_overflow).ToString()).
                       contains(executor.error_type.instruction_ref_overflow.ToString())
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
