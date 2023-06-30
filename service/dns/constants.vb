
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class constants
    Public Const default_resolve_timeout_ms As Int64 = 30 * second_milli
    Public Const default_connectivity_expiration_time_ms As Int64 = 30 * second_milli

    Private Sub New()
    End Sub
End Class
