
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.utt

Public Class lowlevel_keyboard_hook_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New lowlevel_keyboard_hook_case())
    End Sub

    Private Class lowlevel_keyboard_hook_case
        Inherits [case]

        Private Shared Sub log(ByVal e As lowlevel_keyboard_hook.event)
            Console.WriteLine(strcat(e.virtual_keycode, " : ", e.scan_code, " in thread ", current_process_thread_id()))
        End Sub

        Public Overrides Function run() As Boolean
            Dim are As AutoResetEvent = Nothing
            are = New AutoResetEvent(False)
            Using lowlevel_keyboard_hook.[New](Function(ByVal e As lowlevel_keyboard_hook.event) As Boolean
                                                   log(e)
                                                   If e.virtual_keycode = windows_virtual_key.VK_ESCAPE Then
                                                       assert(are.force_set())
                                                   End If
                                                   Return e.virtual_keycode = windows_virtual_key.VK_A
                                               End Function)
                assert(are.wait())
            End Using
            Return True
        End Function
    End Class
End Class
