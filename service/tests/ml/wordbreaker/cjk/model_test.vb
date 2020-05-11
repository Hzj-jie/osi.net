
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)

Namespace wordbreaker.cjk
    <test>
    Public NotInheritable Class model_test
        <test>
        <command_line_specified>
        Private Shared Sub dump()
            Console.WriteLine(model.load("cjk.model.bin"))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
