Option Explicit On
Option Infer Off
Option Strict On
Imports osi.root.connector
Public NotInheritable Class b3style_lib
    Private Sub New()
    End Sub
    Public Shared ReadOnly data() As Byte = 
        Convert.FromBase64String(strcat_hint(CUInt(122), 
            "BwAAAHJ1bi5jbWRrAAAADQpkZWwgL3MgKi51bn4NCi4uXC4uXC4uXHJlc291cmNlXGdlblx0YXJfZ2VuXG9zaS5yb290LnV0dCB0YXJfZ2VuIC0tb3V0cHV0PWIzc3R5bGVfbGliDQptb3ZlIC9ZICoudmIgLi5cDQo="
        ))
End Class
