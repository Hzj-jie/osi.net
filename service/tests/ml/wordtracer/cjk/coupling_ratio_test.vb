
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.utt.attributes
Imports osi.service.ml

<test>
Public NotInheritable Class coupling_ratio_test
    Private Shared input As argument(Of String)
    Private Shared output As argument(Of String)

    <command_line_specified>
    <test>
    Private Shared Sub run()
        coupling_ratio.execute(onebound(Of String).model.load(input Or "cjk.words.2.str.bin")).
                       dump(output Or "cjk.words.2.str.coupling_ratio.bin")
    End Sub

    Private Sub New()
    End Sub
End Class
