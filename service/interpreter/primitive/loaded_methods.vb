
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace primitive
    Public NotInheritable Class loaded_methods
        Public Shared Function current_ms(ByVal i() As Byte) As Byte()
            Return int64_bytes(nowadays.milliseconds())
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
