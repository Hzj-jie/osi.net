Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b3style_runnable_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(37), 
            "CQAAAGNhc2UxLnR4dBQAAADvu78NCnR5cGUwIG1haW4oKSB7fQ=="
        ))
End Class
