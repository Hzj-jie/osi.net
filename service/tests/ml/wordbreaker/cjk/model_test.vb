
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)

Namespace wordbreaker.cjk
    <test>
    Public NotInheritable Class model_test
        <test>
        <command_line_specified>
        Private Shared Sub dump()
            Console.WriteLine(model.load("cjk.model.bin"))
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub search()
            Dim m As model = Nothing
            m = model.load("cjk.model.bin")
            While True
                Dim s As String = Nothing
                s = Console.ReadLine()
                If s Is Nothing Then
                    Return
                End If
                If s.Length() <= 1 Then
                    Continue While
                End If
                For i As Int32 = 0 To s.Length() - 2 Step 2
                    Console.WriteLine(strcat(s(i), " -> ", s(i + 1), " = ", m.affinity(s(i), s(i + 1))))
                Next
            End While
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
