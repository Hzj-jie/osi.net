
Option Explicit On
Option Infer Off
Option Strict On

Public Module _performance
    Public Class performance_counter_categories
        Public Const process As String = "process"
        Public Const processor As String = "Processor"
        Public Const memory As String = "Memory"
    End Class

    Public Class performance_counter_counters
        Public Const processor_time As String = "% Processor Time"
        Public Const available_m_bytes As String = "Available MBytes"
        Public Const available_bytes As String = "Available Bytes"
        Public Const working_set As String = "Working Set"
        Public Const virtual_bytes As String = "Virtual Bytes"
    End Class
End Module
