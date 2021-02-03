
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports osi.service.resource
Imports tracer = osi.service.ml.wordtracer.cjk.tracer

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class tracer_test
        Private Shared input As argument(Of String)
        Private Shared input2 As argument(Of String)
        Private Shared output As argument(Of String)
        Private Shared percentage As argument(Of Double)

        <test>
        <command_line_specified>
        Private Shared Sub from_training_file()
            tracer.train(File.ReadLines(input Or "cjk.training.txt")).dump(output Or "cjk.words.2.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            tracer.train(tar.reader.unzip(
                             vector.of(Directory.GetFiles(Environment.CurrentDirectory(),
                                                          input Or "tar_manual_test.zip_*",
                                                          SearchOption.AllDirectories)))).
                   dump(output Or "cjk.words.2.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub raw_from_training_file()
            tracer.train(New trainer.config() With {.compare = trainer.config.comparison.raw},
                         File.ReadLines(input Or "cjk.training.txt")).
                   dump(output Or "cjk.words.2.raw.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_small_training_file()
            tracer.train(File.ReadLines(input Or "cjk.training.small.txt")).
                   dump(output Or "cjk.words.2.small.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump()
            model.load(input Or "cjk.words.2.bin").
                  flat_map().
                  sort(Function(ByVal i As first_const_pair(Of const_pair(Of Char, Char), Double),
                                ByVal j As first_const_pair(Of const_pair(Of Char, Char), Double)) As Int32
                           assert(Not i Is Nothing)
                           assert(Not j Is Nothing)
                           If i.first.first <> j.first.first Then
                               Return i.first.first.CompareTo(j.first.first)
                           End If
                           If i.second <> j.second Then
                               Return i.second.CompareTo(j.second)
                           End If
                           Return i.first.second.CompareTo(j.first.second)
                       End Function).
                  foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of Char, Char), Double))
                              Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                          End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub run_exponential_selector()
            selector.exponential(model.load(input Or "cjk.words.2.bin"), percentage Or 0.9).
                     dump(output Or "cjk.words.2.bin.e0.9")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub reverse()
            model.load(input Or "cjk.words.2.bin").reverse().dump(output Or "cjk.words.2.bin.reverse")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub merge()
            model.load(input Or "cjk.words.2.bin.e0.9").
                  merge(model.load(input2 Or "cjk.words.2.bin.reverse.e0.9").reverse()).
                  dump(output Or "cjk.words.2.bin.e0.9.bidirectional")
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
