
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
        Private Shared Sub dump()
            model.load(input Or "cjk.words.2.bin").to_console()
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub exponential_distributions()
            model.load(input Or "cjk.words.2.bin").exponential_distributions_to_consle()
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
