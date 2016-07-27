
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.lock

Public MustInherit Class flower(Of T)
    Public Shared ReadOnly is_eos As binder(Of Func(Of T, Boolean), flower(Of T))
    Private ReadOnly s As ref(Of singleentry)

    Shared Sub New()
        is_eos = New binder(Of Func(Of T, Boolean), flower(Of T))()
    End Sub

    Public Sub New()
        s = New ref(Of singleentry)()
    End Sub

    Protected MustOverride Function flow() As event_comb

    Public MustOverride Function [stop]() As Boolean

    Public Function stopped() As Boolean
        Return s.in_use()
    End Function

    Public Shared Operator +(ByVal this As flower(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If this Is Nothing Then
                                      Return False
                                  Else
                                      ec = this.flow()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return assert(this.s.mark_in_use()) AndAlso
                                         ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Operator
End Class
