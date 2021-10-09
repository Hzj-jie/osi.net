
Option Explicit On
Option Infer Off
Option Strict On

Public Module _event_driver
    Public ReadOnly event_driver_begin_in_current_thread As Boolean = False
    ' it's not working, since in event_driver the order of running chained event_comb and goto will be impacted.
#If 0 Then
        event_driver_begin_in_current_thread = env_bool(env_keys("event",
                                                                 "driver",
                                                                 "begin",
                                                                 "in",
                                                                 "current",
                                                                 "thread"))
#End If
End Module
