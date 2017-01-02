
Imports System.Diagnostics
Imports osi.root.constants

Public Module _process
    Public ReadOnly current_process As Process
    Public ReadOnly end_of_file As Char

    Sub New()
        current_process = Process.GetCurrentProcess()
        end_of_file = If(os.family = os.family_t.windows OrElse os.family = os.family_t.xbox,
                         character.sub,
                         character.eot)
    End Sub
End Module
