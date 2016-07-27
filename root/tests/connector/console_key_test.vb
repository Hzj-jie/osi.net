
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class console_key_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New console_key_case())
    End Sub

    Private Class console_key_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Const exit_cmd As String = "exit"
            Dim c As ConsoleKeyInfo = Nothing
            Console.TreatControlCAsInput = True
            Dim s As Int32 = 0
            While True
                c = Console.ReadKey(True)
                If c.alt() Then
                    Console.Write("ALT+")
                End If
                If c.shift() Then
                    Console.Write("SHIFT+")
                End If
                If c.ctrl() Then
                    Console.Write("CTL+")
                End If
                Console.WriteLine(strcat(c.Key(),
                                         "[",
                                         CInt(c.Key()),
                                         "]",
                                         " -> ",
                                         "[",
                                         Convert.ToInt32(c.KeyChar()),
                                         "]",
                                         c.KeyChar()))
                If c.no_modifiers() Then
                    If c.KeyChar() = exit_cmd(s) Then
                        s += 1
                    ElseIf c.KeyChar() = exit_cmd(0) Then
                        s = 1
                    Else
                        s = 0
                    End If
                    If s = 4 Then
                        Exit While
                    End If
                End If
            End While
            Return True
        End Function
    End Class
End Class
