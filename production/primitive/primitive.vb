
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.service.interpreter.primitive

Public Module primitive
    Public Sub main(ByVal args() As String)
        global_init.execute(global_init_level.functor)
        Dim s As simulator = Nothing
        s = New simulator()
        If isemptyarray(args) Then
            assert(s.import(Console.In().ReadToEnd()))
        Else
            assert(s.import(File.ReadAllBytes(args(0))) OrElse
                   s.import(File.ReadAllText(args(0))))
        End If
        s.execute()
    End Sub
End Module
