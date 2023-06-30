
Option Explicit On
Option Infer Off
Option Strict On

Namespace constants
    Public NotInheritable Class mapheap_cache
        Public Const default_max_size As UInt64 = 1024
        Public Const no_retire_ticks As UInt64 = 0
        Public Const default_retire_ticks As UInt64 = no_retire_ticks
        Public Const default_update_ticks_when_refer As Boolean = True

        Private Sub New()
        End Sub
    End Class
End Namespace