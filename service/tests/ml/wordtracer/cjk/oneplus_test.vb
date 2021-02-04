
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml
Imports osi.service.resource
Imports oneplus = osi.service.ml.wordtracer.cjk.oneplus

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class oneplus_test
        Private Shared model_file As argument(Of String)
        Private Shared filter As argument(Of Double)
        Private Shared sample_rate As argument(Of Double)
        Private Shared input As argument(Of String)
        Private Shared output As argument(Of String)
        Private Shared percentage As argument(Of Double)
        Private Shared input2 As argument(Of String)

        <test>
        <command_line_specified>
        Private Shared Sub from_training_file()
            Dim t As New oneplus(onebound(Of Char).model.load(model_file Or "cjk.words.2.bin.e0.9.bidirectional").
                                                         filter(filter Or 0),
                                 sample_rate Or 1)
            t.train(File.ReadLines(input Or "cjk.training.txt")).dump(output Or "cjk.words.3.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            Dim t As New oneplus(onebound(Of Char).model.load(model_file Or "cjk.words.2.bin.e0.9.bidirectional").
                                                         filter(filter Or 0),
                                 sample_rate Or 1)
            t.train(tar.reader.unzip(
                        vector.of(Directory.GetFiles(Environment.CurrentDirectory(),
                                                     input Or "tar_manual_test.zip_*",
                                                     SearchOption.AllDirectories)))).
              dump(output Or "cjk.words.3.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar2()
            Dim t As New oneplus(onebound(Of String).model.load(model_file Or "cjk.words.3.bin.e0.9.bidirectional").
                                                           filter(filter Or 0),
                                 sample_rate Or 1)
            t.train(tar.reader.unzip(
                        vector.of(Directory.GetFiles(Environment.CurrentDirectory(),
                                                     input Or "tar_manual_test.zip_*",
                                                     SearchOption.AllDirectories)))).
              dump(output Or "cjk.words.4.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump()
            onebound(Of String).model.load(input Or "cjk.words.3.bin").to_console()
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub run_exponential_selector()
            onebound(Of String).selector.exponential(onebound(Of String).model.load(input Or "cjk.words.3.bin"),
                                                     percentage Or 0.9).
                                         dump(output Or "cjk.words.3.bin.e0.9")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub reverse()
            onebound(Of String).model.load(input Or "cjk.words.3.bin").
                                      reverse().
                                      dump(output Or "cjk.words.3.bin.reverse")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub merge()
            onebound(Of String).model.load(input Or "cjk.words.3.bin.e0.9").
                                      merge(onebound(Of String).model.load(input2 Or "cjk.words.3.bin.reverse.e0.9").
                                                                      reverse()).
                                      dump(output Or "cjk.words.3.bin.e0.9.bidirectional")
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
