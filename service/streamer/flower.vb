
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.procedure

Public MustInherit Class flower(Of T)
    Private ReadOnly s As ref(Of singleentry)

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

    Public Shared Sub register_is_eos(ByVal f As Func(Of T, Boolean))
        global_resolver(Of Func(Of T, Boolean), flower(Of T)).register(f)
    End Sub

    Protected Shared Function is_eos(ByVal i As T) As Boolean
        If i Is Nothing Then
            Return True
        End If
        Dim f As Func(Of T, Boolean) = Nothing
        f = global_resolver(Of Func(Of T, Boolean), flower(Of T)).resolve_or_null()
        Return f IsNot Nothing AndAlso f(i)
    End Function
End Class
