
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports compiler = osi.service.compiler.b2style
Imports executor = osi.service.interpreter.primitive.executor

Public Module b2style
    Public Sub main(ByVal args() As String)
        global_init.execute(global_init_level.functor)
        Dim c As compiler.parse_wrapper = Nothing
        c = compiler.with_default_functions()
        Dim e As executor = Nothing
        If isemptyarray(args) Then
            assert(c.parse(Console.In().ReadToEnd(), e))
        Else
            assert(c.parse(IO.File.ReadAllText(args(0)), e))
        End If
        e.execute()
    End Sub
End Module
