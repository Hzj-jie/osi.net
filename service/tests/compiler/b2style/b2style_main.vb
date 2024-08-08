
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports executor = osi.service.interpreter.primitive.executor

<test>
Public NotInheritable Class b2style_main
    Private Shared input As argument(Of String)

    <command_line_specified>
    <test>
    Private Shared Sub run()
        Dim c As b2style.parse_wrapper = b2style.with_default_functions()
        Dim e As executor = Nothing
        If (+input).empty_or_whitespace() Then
            assert(c.compile(Console.In().ReadToEnd(), e))
        Else
            assert(c.compile_file(+input, e))
        End If
        e.execute()
    End Sub

    Private Sub New()
    End Sub
End Class
