
Imports osi.root.procedure

Public Interface ireceiver(Of CASE_T)
    Function receive(ByVal c As CASE_T) As event_comb
End Interface

Public Interface iagent(Of CASE_T)
    Event deliver(ByVal c As CASE_T, ByRef failed As Boolean)
End Interface

Public MustInherit Class agent(Of CASE_T)
    Implements iagent(Of CASE_T)

    Public Event deliver(ByVal c As CASE_T, ByRef failed As Boolean) Implements iagent(Of CASE_T).deliver

    Protected Sub received(ByVal c As CASE_T)
        Dim failed As Boolean = False
        received(c, failed)
        If failed Then
            deliver_failed(c)
        End If
    End Sub

    Protected Overridable Sub deliver_failed(ByVal c As CASE_T)
    End Sub

    Protected Sub received(ByVal c As CASE_T, ByRef failed As Boolean)
        RaiseEvent deliver(c, failed)
    End Sub
End Class
