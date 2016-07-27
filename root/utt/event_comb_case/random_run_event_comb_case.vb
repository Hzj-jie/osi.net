
Imports osi.root.procedure

Public MustInherit Class random_run_event_comb_case
    Inherits event_comb_case

    Private ReadOnly r As random_run(Of event_comb)

    Public Sub New()
        r = New random_run(Of event_comb)()
    End Sub

    Public NotOverridable Overrides Function create() As event_comb
        Return r.select()
    End Function
End Class
