
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports osi.service.resource
Imports tracer = osi.service.ml.wordtracer.cjk.tracer

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class tracer_test
        <test>
        <command_line_specified>
        Private Shared Sub from_training_file()
            Dim m As model = Nothing
            m = tracer.train(IO.File.ReadLines("cjk.training.txt"))
            m.dump("cjk.words.2.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            tracer.train(tar.reader.unzip(
                             vector.emplace_of(Directory.GetFiles(Environment.CurrentDirectory(),
                                                                  "tar_manual_test.zip_*",
                                                                  SearchOption.AllDirectories)))).
                   dump("cjk.words.2.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub raw_from_training_file()
            Dim config As trainer.config = Nothing
            config = New trainer.config()
            config.compare = trainer.config.comparison.raw
            Dim m As model = Nothing
            m = tracer.train(config, IO.File.ReadLines("cjk.training.txt"))
            m.dump("cjk.words.2.raw.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_small_training_file()
            Dim m As model = Nothing
            m = tracer.train(IO.File.ReadLines("cjk.training.small.txt"))
            m.dump("cjk.words.2.small.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump()
            model.load("cjk.words.2.bin").
                  filter(0.1).
                  flat_map().
                  foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of Char, Char), Double))
                              Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                          End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump_raw()
            model.load("cjk.words.2.raw.bin").
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
        Private Shared Sub dump_small()
            model.load("cjk.words.2.small.bin").
                  filter(0.1).
                  flat_map().
                  foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of Char, Char), Double))
                              Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                          End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
