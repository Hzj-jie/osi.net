
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.utils
Imports compiler = osi.service.compiler.b2style
Imports executor = osi.service.interpreter.primitive.executor

Public Module b2style
    Public Sub main(ByVal args() As String)
        global_init.execute()
        Dim s As String = Nothing
        If isemptyarray(args) Then
            Dim b As StringBuilder = Nothing
            b = New StringBuilder()
            Dim line As String = Nothing
            line = System.Console.ReadLine()
            While Not line Is Nothing
                b.AppendLine(line)
                line = System.Console.ReadLine()
            End While
            s = b.ToString()
        Else
            s = System.IO.File.ReadAllText(args(0))
        End If
        Dim e As executor = Nothing
        assert(compiler.with_default_functions().parse(s, e))
        e.execute()
    End Sub
End Module
