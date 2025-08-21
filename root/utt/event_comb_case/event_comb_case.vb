
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure

Public MustInherit Class event_comb_case
    Inherits [case]

    Public MustOverride Function create() As event_comb

    Public NotOverridable Overrides Function run() As Boolean
        ' TODO: Find a way to forward the case name without creating a wrapper event_comb.
        Dim ec As event_comb = create()
        Using New thread_lazy()
            Return async_sync(ec)
        End Using
    End Function
End Class
