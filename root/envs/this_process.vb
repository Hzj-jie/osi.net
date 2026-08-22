
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class this_process
    Public Shared ReadOnly ref As Process = Process.GetCurrentProcess()
    Public Shared ReadOnly end_of_file As Char =
        If(os.is_windows OrElse os.family = os.family_t.xbox,
           character.sub,
           character.eot)

    Public Shared Sub suicide(Optional ByVal ext_code As Int32 = npos)
        Environment.FailFast(Convert.ToString(ext_code))
    End Sub

    Public Shared Sub [exit](Optional ByVal ext_code As Int32 = 0)
        Environment.Exit(ext_code)
    End Sub

    Private Sub New()
    End Sub
End Class
