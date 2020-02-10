﻿
Imports osi.root.constants
Imports osi.root.connector

<global_init(global_init_level.log_and_counter_services)>
Friend Class error_counter
    Private Shared ReadOnly CRITICAL_ERROR_COUNTER As Int64
    Private Shared ReadOnly EXCLAMATION_ERROR_COUNTER As Int64
    Private Shared ReadOnly SYSTEM_ERROR_COUNTER As Int64
    Private Shared ReadOnly OTHER_ERROR_COUNTER As Int64

    Shared Sub New()
        CRITICAL_ERROR_COUNTER = counter.register_counter("CRITICAL_ERROR_COUNTER")
        EXCLAMATION_ERROR_COUNTER = counter.register_counter("EXCLAMATION_ERROR_COUNTER")
        SYSTEM_ERROR_COUNTER = counter.register_counter("SYSTEM_ERROR_COUNTER")
        OTHER_ERROR_COUNTER = counter.register_counter("OTHER_ERROR_COUNTER")

        AddHandler error_event.r5,
                   Sub(err_type As error_type)
                       If err_type = error_type.critical Then
                           counter.increase(CRITICAL_ERROR_COUNTER)
                       ElseIf err_type = error_type.exclamation Then
                           counter.increase(EXCLAMATION_ERROR_COUNTER)
                       ElseIf err_type = error_type.other Then
                           counter.increase(OTHER_ERROR_COUNTER)
                       ElseIf err_type = error_type.system Then
                           counter.increase(SYSTEM_ERROR_COUNTER)
                       End If
                   End Sub
    End Sub

    Private Shared Sub init()
    End Sub
End Class
