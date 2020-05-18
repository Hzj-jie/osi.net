
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)
Imports evaluator = osi.service.ml.wordbreaker.cjk.evaluator

Namespace wordbreaker.cjk
    <test>
    Public NotInheritable Class evaluator_test
        <test>
        <command_line_specified>
        Private Shared Sub from_model()
            Dim m As model = Nothing
            m = model.load("cjk.model.bin")
            Dim e As evaluator = Nothing
            e = New evaluator(m)
            Using o As StreamWriter = New StreamWriter("cjk.result.txt")
                e.break(File.ReadLines("cjk.evaluation.txt"),
                        Sub(ByVal s As String)
                            o.WriteLine(s)
                            o.Flush()
                        End Sub)
            End Using
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
