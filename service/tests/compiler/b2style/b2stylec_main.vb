
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports executor = osi.service.interpreter.primitive.executor

<test>
Public NotInheritable Class b2stylec_main
    Private Shared input As argument(Of String)
    Private Shared output As argument(Of String)

    <command_line_specified>
    <test>
    Private Shared Sub run()
        Dim e As executor = Nothing
        Using code_block
            Dim parser As b2style.parse_wrapper = b2style.with_default_functions()
            Dim p As _do(Of executor, Boolean) = Nothing
            If (+input).empty_or_whitespace() Then
                p = Function(ByRef o As executor) As Boolean
                        Return parser.parse(Console.In().ReadToEnd(), o)
                    End Function
            Else
                p = Function(ByRef o As executor) As Boolean
                        Return parser.parse_file(+input, o)
                    End Function
            End If
            assert(p(e))
        End Using
        Dim text_output As TextWriter = Nothing
        Dim binary_output As Stream = Nothing
        If (+output).empty_or_whitespace() Then
            text_output = Console.Out()
        Else
            binary_output = New FileStream(+output, FileMode.Create)
        End If
        If Not text_output Is Nothing Then
            Dim s As String = Nothing
            assert(e.export(s))
            Using text_output
                text_output.Write(s)
            End Using
        Else
            assert(Not binary_output Is Nothing)
            Dim b() As Byte = Nothing
            assert(e.export(b))
            Using binary_output
                binary_output.Write(b, 0, array_size_i(b))
            End Using
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
