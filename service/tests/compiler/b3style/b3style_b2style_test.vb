
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<command_line_specified>
<test>
Public NotInheritable Class b3style_b2style_test
    Inherits b2style_test(Of parse)

    Public NotInheritable Class parse
        Inherits __do(Of console_io.test_wrapper, String, executor, Boolean)

        Public Overrides Function at(ByRef i As console_io.test_wrapper,
                                     ByRef j As String,
                                     ByRef k As executor) As Boolean
            Return New b3style.parse_wrapper(New interrupts(+i)).parse(j, k)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
