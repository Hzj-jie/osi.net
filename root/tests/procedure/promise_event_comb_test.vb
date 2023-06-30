
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt

Public Class promise_event_comb_test
    Inherits osi.root.utt.event_comb_case

    Public Overrides Function create() As event_comb
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim ec As event_comb = Nothing
        ec = New promise(Function() As Object
                             Return CType(New event_comb(Function() As Boolean
                                                             Return waitfor(10) AndAlso
                                                                    goto_next()
                                                         End Function,
                                                         Function() As Boolean
                                                             Return goto_end()
                                                         End Function), promise)
                         End Function)
        Return New event_comb(Function() As Boolean
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.more_or_equal(nowadays.milliseconds() - start_ms, 10)
                                  Return goto_end()
                              End Function)
    End Function
End Class
