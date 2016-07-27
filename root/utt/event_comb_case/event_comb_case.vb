
Imports osi.root.connector
Imports osi.root.procedure

Public MustInherit Class event_comb_case
    Inherits [case]

    Public MustOverride Function create() As event_comb

    Public NotOverridable Overrides Function run() As Boolean
        Dim ec As event_comb = Nothing
        ec = create()
        Using New thread_lazy()
            Return async_sync(ec)
        End Using
    End Function
End Class
