
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes

<test>
Public NotInheritable Class console_behavior_test
    <command_line_specified>
    <test>
    Private Shared Sub close_console_out()
        Console.Out().Close()
        Console.Error().Close()
        Console.Out().WriteLine("HERE WE GO")
    End Sub
End Class
