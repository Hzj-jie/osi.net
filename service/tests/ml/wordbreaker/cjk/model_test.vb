
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound(Of Char)

Namespace wordbreaker.cjk
    <test>
    Public NotInheritable Class model_test
        Private Shared input As argument(Of String)

        <test>
        <command_line_specified>
        Private Shared Sub dump()
            model.load(input Or "cjk.model.bin").
                  flat_map().
                  foreach(Sub(ByVal p As first_const_pair(Of const_pair(Of Char, Char), Double))
                              Console.WriteLine(strcat(p.first.first, p.first.second, ": ", p.second))
                          End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub search()
            Dim m As model = model.load(input Or "cjk.model.bin")
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
