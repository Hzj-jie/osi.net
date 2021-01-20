
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Class colorful_console_error_writer
    Shared Sub New()
        error_writer_ignore_types(Of colorful_console_error_writer).ignore(
            error_type.performance, error_type.information, error_type.deprecated)
        AddHandler error_event.r3,
                   Sub(err_type As error_type, s As String)
                       If error_writer_ignore_types(Of colorful_console_error_writer).valued(err_type) Then
#If PocketPC OrElse Smartphone Then
                           write(s)
#Else
                           colorful_write(err_type, s)
#End If
                       End If
                   End Sub
    End Sub

    Private Sub New()
    End Sub

    Private Shared Sub write(ByVal s As String)
        Console.WriteLine(s)
    End Sub

#If Not (PocketPC OrElse Smartphone) Then
    Private Shared Sub colorful_write(ByVal errtype As error_type, ByVal s As String)
        Dim oldcolor As ConsoleColor = Nothing
        oldcolor = Console.ForegroundColor()
        Try
            If errtype = error_type.application Then
                Console.ForegroundColor() = ConsoleColor.Blue
            ElseIf errtype = error_type.critical Then
                Console.ForegroundColor() = ConsoleColor.Red
            ElseIf errtype = error_type.exclamation Then
                Console.ForegroundColor() = ConsoleColor.Yellow
            ElseIf errtype = error_type.information Then
                Console.ForegroundColor() = ConsoleColor.White
            ElseIf errtype = error_type.other Then
                Console.ForegroundColor() = ConsoleColor.Magenta
            ElseIf errtype = error_type.user Then
                Console.ForegroundColor() = ConsoleColor.Cyan
            ElseIf errtype = error_type.system Then
                Console.ForegroundColor() = ConsoleColor.Gray
            ElseIf errtype = error_type.performance Then
                Console.ForegroundColor() = ConsoleColor.DarkYellow
            Else
                Console.ForegroundColor() = ConsoleColor.Green
            End If
            Console.WriteLine(s)
        Finally
            Console.ForegroundColor() = oldcolor
        End Try
    End Sub
#End If
End Class
