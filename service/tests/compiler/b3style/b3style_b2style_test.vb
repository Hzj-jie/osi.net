
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_test
    Inherits b2style_test_runner

    Protected Overrides Function parse(ByVal io As console_io.test_wrapper,
                                       ByVal content As String,
                                       ByRef o As executor) As Boolean
        Return New b3style.parse_wrapper(New interrupts(+io)).compile(content, o)
    End Function

    Private Sub New()
    End Sub
End Class
