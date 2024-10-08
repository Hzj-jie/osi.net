
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates
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
            With New oneplus(onebound(Of Char).model.load(model_file Or "cjk.words.2.bin.e0.9.bidirectional").
                                                         filter(filter Or 0),
                             sample_rate Or 1)
                .train(File.ReadLines(input Or "cjk.training.txt")).dump(output Or "cjk.words.3.bin")
            End With
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            With New oneplus(onebound(Of Char).model.load(model_file Or "cjk.words.2.bin.e0.9.bidirectional").
                                                         filter(filter Or 0),
                                 sample_rate Or 1)
                .train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"})).
                dump(output Or "cjk.words.3.bin")
            End With
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar2()
            With New oneplus(onebound(Of String).model.load(model_file Or "cjk.words.3.bin.e0.9.bidirectional").
                                                           filter(filter Or 0),
                             sample_rate Or 1)
                .train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"})).
                 dump(output Or "cjk.words.4.bin")
            End With
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
        Private Shared Sub multiply()
            onebound(Of String).model.load(input Or "cjk.words.3.bin.e0.9").
                                      multiply(onebound(Of String).model.load(input2 Or "cjk.words.3.bin.reverse.e0.9").
                                                                      reverse()).
                                      dump(output Or "cjk.words.3.bin.e0.9.bidirectional")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub filter_out_1()
            onebound(Of String).model.load(input Or "cjk.words.3.bin").
                                      filter(2).
                                      dump(output Or "cjk.words.3.bin.gt1")
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
