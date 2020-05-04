
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports trainer = osi.service.ml.wordbreaker_cjk.trainer

Namespace wordbreaker.cjk
    <test>
    Public NotInheritable Class trainer_test
        <test>
        <command_line_specified>
        Private Shared Sub from_training_file()
            Dim m As model = Nothing
            m = trainer.train(IO.File.ReadAllText("cjk.training.txt"))
            m.dump("cjk.model.txt")
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
