
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_compile_error_test
    Inherits b2style_compile_error_test(Of parse)

    Public NotInheritable Class parse
        Inherits __do(Of String, executor, Boolean)

        Public Overrides Function at(ByRef j As String, ByRef k As executor) As Boolean
            Return New b3style.parse_wrapper(interrupts.default).compile(j, k)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
