
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports tracer = osi.service.ml.wordtracer.cjk.tracer

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class tracer_test
        <test>
        <command_line_specified>
        Private Shared Sub from_training_file()
            Dim m As model = Nothing
            m = tracer.train(IO.File.ReadLines("cjk.training.txt"))
            m.dump("cjk.words.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump()
            Console.WriteLine(model.load("cjk.words.bin").
                                    filter(0.1).
                                    flat_map().
                                    map(Function(x) const_pair.emplace_of(strcat(x.first.first, " ", x.first.second),
                                                                          x.second)).
                                    collect(Of vector(Of const_pair(Of String, Double)))())
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
