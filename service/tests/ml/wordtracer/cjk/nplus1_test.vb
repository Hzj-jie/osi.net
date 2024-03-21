
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports nplus1 = osi.service.ml.wordtracer.cjk.nplus1

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class nplus1_test
        Private Shared input As argument(Of String)
        Private Shared output As argument(Of String)
        Private Shared percentage As argument(Of Double)

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            Dim n As New nplus1(1)
            n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
            n.dump(percentage Or 0.9).
              dump(output Or "cjk.nplus1.2.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub raw()
            Dim n As New nplus1(1)
            n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
            n.forward().dump((output Or "cjk.nplus1.2.bin") + ".forward")
            n.forward().dump((output Or "cjk.nplus1.2.bin") + ".backward")
        End Sub
    End Class
End Namespace
