
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class big_int_calculator
    Private Shared ReadOnly e As New big_int_expression()
    Private Shared input As argument(Of String)

    Private Shared Sub calculate(ByVal arg As String, ByVal o As TextWriter)
        Dim r As expression_result(Of big_int) = e.execute(arg)
        o.WriteLine(r.str())
    End Sub

    Private Shared Sub calculate(ByVal args() As String, ByVal o As TextWriter)
        calculate(strcat(args), o)
    End Sub

    <command_line_specified>
    <test>
    Private Shared Sub run()
        If Not (+input).empty_or_whitespace() Then
            calculate(+input, Console.Out())
            Return
        End If
        Dim s As String = Nothing
        While True
            s = Console.ReadLine()
            If s Is Nothing Then
                Exit While
            Else
                calculate(s, Console.Out())
            End If
        End While
    End Sub

    Private Sub New()
    End Sub
End Class
