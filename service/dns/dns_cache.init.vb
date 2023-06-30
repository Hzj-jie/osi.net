
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

Partial Public NotInheritable Class dns_cache
    Private Shared max_size As UInt64
    Private Shared retire_ticks As UInt64

    Public Shared Sub init(ByVal max_size As UInt64, ByVal retire_ticks As UInt64)
        If max_size > 0 Then
            dns_cache.max_size = max_size
        End If
        dns_cache.retire_ticks = retire_ticks
    End Sub

    Public Shared Sub reset()
        reset_max_size()
        reset_retire_ticks()
    End Sub

    Public Shared Sub reset_max_size()
        max_size = 1024
    End Sub

    Public Shared Sub reset_retire_ticks()
        retire_ticks = CULng(seconds_to_ticks(10 * minute_second))
    End Sub

    Private Shared Sub default_init()
        Dim s As String = Nothing
        If Not env_value(env_keys("dns", "cache", "size"), s) OrElse
           Not UInt64.TryParse(s, max_size) Then
            reset_max_size()
        End If
        If Not env_value(env_keys("dns", "cache", "retire", "ticks"), s) OrElse
           Not UInt64.TryParse(s, retire_ticks) Then
            reset_retire_ticks()
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
