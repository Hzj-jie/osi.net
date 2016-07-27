
Imports osi.root.envs
Imports osi.root.constants

Public Class memory_measured_case_wrapper
    Inherits measurement_case_wrapper

    Public Sub New(ByVal c As [case], Optional ByVal interval_ms As Int64 = second_milli)
        MyBase.New(c, interval_ms)
    End Sub

    Protected NotOverridable Overrides Function sample() As Int64
        Return private_bytes_usage()
    End Function
End Class
