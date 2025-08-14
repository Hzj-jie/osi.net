
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_b3style_compile_error_test
    Inherits b2style_compile_error_test_runner

    Protected Overrides Function parse(ByVal content As String) As Boolean
        Return New b2style.compile_wrapper_b3style(interrupts.default).compile(content, Nothing)
    End Function

    Private Sub New()
    End Sub
End Class
