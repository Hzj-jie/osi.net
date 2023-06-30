
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.procedure

Public MustInherit Class filtered_procedure_module
    Inherits filtered_module

    Protected Sub New(ByVal filter As context_filter)
        MyBase.New(filter)
    End Sub

    Protected MustOverride Function execute(ByVal context As server.context) As event_comb

    Protected NotOverridable Overrides Sub process(ByVal context As server.context)
        procedure_handle.process_context(context, execute(context))
    End Sub
End Class
