
Imports System.Diagnostics

Public Module _process
    Public ReadOnly current_process As Process = Nothing

    Sub New()
        current_process = Process.GetCurrentProcess()
    End Sub
End Module
