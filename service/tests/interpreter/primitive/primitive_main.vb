
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt.attributes
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class primitive_main
    Private Shared input As argument(Of String)

    <command_line_specified>
    <test>
    Private Shared Sub run()
        Dim s As New simulator()
        If (+input).empty_or_whitespace() Then
            assert(s.import(Console.In().ReadToEnd()))
        Else
            assert(s.import(File.ReadAllBytes(+input)) OrElse
                   s.import(File.ReadAllText(+input)))
        End If
        s.execute()
    End Sub

    Private Sub New()
    End Sub
End Class
