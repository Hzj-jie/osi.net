Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b3style_runnable_test_cases
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(185), 
            "GAAAAGluY2x1ZGUtYnN0eWxlLXR5cGVzLnR4dHQAAADvu78NCiNpbmNsdWRlIDxic3R5bGUvdHlwZXMuaD4NCi8vIEluY2x1ZGUgaXQgdHdpY2UgdG8gdGVzdCB0aGUgaWZuZGVmLg0KI2luY2x1ZGUgPGJzdHlsZS90eXBlcy5oPg0KDQp2b2lkIG1haW4oKSB7fQkAAABjYXNlMS50eHQUAAAA77u/DQp0eXBlMCBtYWluKCkge30="
        ))
End Class
