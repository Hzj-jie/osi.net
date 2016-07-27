
Imports System.IO
Imports osi.root.connector
Imports osi.service.math

Public Module big_uint_calculator
    Private ReadOnly e As big_uint_basic_expression

    Sub New()
        e = New big_uint_basic_expression()
    End Sub

    Private Sub calculate(ByVal arg As String, ByVal o As TextWriter)
        Dim r As expression_result(Of big_uint) = Nothing
        r = e.execute(arg)
        o.WriteLine(r.str())
    End Sub

    Private Sub calculate(ByVal args() As String, ByVal o As TextWriter)
        Dim r As expression_result(Of big_uint) = Nothing
        r = e.execute(args)
        o.WriteLine(r.str())
    End Sub

    Public Sub main(ByVal args() As String)
        If isemptyarray(args) Then
            Dim s As String = Nothing
            While True
                s = Console.ReadLine()
                If s Is Nothing Then
                    Exit While
                Else
                    calculate(s, Console.Out())
                End If
            End While
        Else
            calculate(args, Console.Out())
        End If
    End Sub
End Module
