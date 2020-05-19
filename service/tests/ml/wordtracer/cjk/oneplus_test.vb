
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports oneplus = osi.service.ml.wordtracer.cjk.oneplus
Imports tracer = osi.service.ml.wordtracer.cjk.tracer
Imports model_str = osi.service.ml.onebound(Of String).model

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class oneplus_test
        <test>
        <command_line_specified>
        Private Shared Sub from_training_file2()
            Dim m As model = Nothing
            m = model.load("cjk.words.2.bin").filter(0.1)
            Dim t As oneplus = Nothing
            t = New oneplus(tracer.flat_map(m))
            t.train(IO.File.ReadLines("cjk.training.txt")).dump("cjk.words.3.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_small_training_file2()
            Dim m As model = Nothing
            m = model.load("cjk.words.2.small.bin").filter(0.1)
            Dim t As oneplus = Nothing
            t = New oneplus(tracer.flat_map(m))
            t.train(IO.File.ReadLines("cjk.training.small.txt")).dump("cjk.words.3.small.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_training_file3()
            Dim m As model_str = Nothing
            m = model_str.load("cjk.words.3.bin").filter(0.1)
            Dim t As oneplus = Nothing
            t = New oneplus(tracer.flat_map(m))
            t.train(IO.File.ReadLines("cjk.training.txt")).dump("cjk.words.4.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_small_training_file3()
            Dim m As model_str = Nothing
            m = model_str.load("cjk.words.3.small.bin").filter(0.1)
            Dim t As oneplus = Nothing
            t = New oneplus(tracer.flat_map(m))
            t.train(IO.File.ReadLines("cjk.training.small.txt")).dump("cjk.words.4.small.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump3()
            model_str.load("cjk.words.3.bin").
                      filter(0.1).
                      flat_map().
                      foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of String, String), Double))
                                  Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                              End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump4()
            model_str.load("cjk.words.4.bin").
                      filter(0.1).
                      flat_map().
                      foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of String, String), Double))
                                  Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                              End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump3_small()
            model_str.load("cjk.words.3.small.bin").
                      filter(0.1).
                      flat_map().
                      foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of String, String), Double))
                                  Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                              End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub dump4_small()
            model_str.load("cjk.words.4.small.bin").
                      filter(0.1).
                      flat_map().
                      foreach(Sub(ByVal x As first_const_pair(Of const_pair(Of String, String), Double))
                                  Console.WriteLine(strcat(x.first.first, " ", x.first.second, ", ", x.second))
                              End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
