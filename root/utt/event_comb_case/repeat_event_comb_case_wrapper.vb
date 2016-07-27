
Imports osi.root.procedure
Imports osi.root.constants
Imports osi.root.connector

Public Class repeat_event_comb_case_wrapper
    Inherits event_comb_case_wrapper

    Private ReadOnly size As Int64 = 0

    Public Sub New(ByVal c As event_comb_case, Optional ByVal test_size As Int64 = npos)
        MyBase.New(c)
        size = test_size
    End Sub

    Protected Overridable Function test_size() As Int64
        Return size
    End Function

    Public NotOverridable Overrides Function create() As event_comb
        assert(test_size() > 0)
        Return event_comb.repeat(test_size(), Function() MyBase.create())
    End Function
End Class
