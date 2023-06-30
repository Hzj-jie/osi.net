
Imports osi.root.constants
Imports osi.root.envs

Public Class gc_memory_measured_case_wrapper
    Inherits measurement_case_wrapper

    Public Sub New(ByVal c As [case], Optional ByVal interval_ms As Int64 = second_milli)
        MyBase.New(c, interval_ms)
    End Sub

    Protected NotOverridable Overrides Function sample() As Int64
        Return gc_total_memory()
    End Function
End Class
