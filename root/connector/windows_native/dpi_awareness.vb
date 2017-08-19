
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class dpi_awareness
    Private Declare Function SetProcessDPIAware Lib "user32.dll" () As Boolean

    Public Shared Function set_aware() As Boolean
        Try
            Return SetProcessDPIAware()
        Catch ex As Exception
            raise_error(error_type.exclamation, "Failed to execute SetProcessDPIAware(), ex ", ex)
            Return False
        End Try
    End Function

    Private Sub New()
    End Sub
End Class
