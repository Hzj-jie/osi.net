
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports compiler = osi.service.compiler.b2style
Imports executor = osi.service.interpreter.primitive.executor

Public Module b2stylec
    Public Sub main(ByVal args() As String)
        global_init.execute(global_init_level.functor)
        Dim source As String = Nothing
        Dim text_output As TextWriter = Nothing
        Dim binary_output As Stream = Nothing
        If isemptyarray(args) Then
            source = Console.In().ReadToEnd()
            text_output = Console.Out()
        ElseIf array_size(args) = 1 Then
            source = File.ReadAllText(args(0))
            text_output = Console.Out()
        Else
            source = File.ReadAllText(args(0))
            binary_output = New FileStream(args(1), FileMode.Create)
        End If
        Dim e As executor = Nothing
        assert(compiler.with_default_functions().parse(source, e))
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
End Module
