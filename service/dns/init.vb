
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

Public Module _init
    Friend max_size As UInt64
    Friend retire_ticks As UInt64

    Public Sub init(ByVal max_size As UInt64,
                    ByVal retire_ticks As UInt64)
        If max_size > 0 Then
            _init.max_size = max_size
        End If
        _init.retire_ticks = retire_ticks
    End Sub

    Sub New()
        Dim s As String = Nothing
        If Not env_value(env_keys("dns", "cache", "size"), s) OrElse
           Not UInt64.TryParse(s, max_size) Then
            max_size = 1024
        End If
        If Not env_value(env_keys("dns", "cache", "retire", "ticks"), s) OrElse
           Not UInt64.TryParse(s, retire_ticks) Then
            retire_ticks = seconds_to_ticks(10 * minute_second)
        End If
    End Sub
End Module
