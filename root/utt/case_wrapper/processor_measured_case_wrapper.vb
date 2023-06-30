
Imports osi.root.envs
Imports osi.root.constants

Public Class processor_measured_case_wrapper
    Inherits measurement_case_wrapper

    Public Sub New(ByVal c As [case], Optional ByVal interval_ms As Int64 = second_milli)
        MyBase.New(c, interval_ms)
    End Sub

    Protected NotOverridable Overrides Function sample() As Int64
        Return recent_processor_usage() * 100
    End Function
End Class
