
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.procedure

' MustInherit to avoid utt-host to load this empty case.
Public MustInherit Class random_run_event_comb_case
    Inherits event_comb_case

    Private ReadOnly r As random_run(Of event_comb)

    Protected Sub New()
        r = New random_run(Of event_comb)()
    End Sub

    Public NotOverridable Overrides Function create() As event_comb
        Return r.select()
    End Function

    Public Sub insert_calls(ByVal ParamArray cs() As pair(Of Double, Func(Of event_comb)))
        r.insert_calls(cs)
    End Sub

    Public Sub insert_call(ByVal percentage As Double, ByVal d As Func(Of event_comb))
        r.insert_call(percentage, d)
    End Sub
End Class
