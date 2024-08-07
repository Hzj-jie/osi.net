
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root
Imports osi.root.procedure
Imports osi.root.utt

Public Class event_comb_repeat_test
    Inherits utt.event_comb_case

    Public Overrides Function create() As event_comb
        Const size As Int32 = 1024
        Dim i As Int32 = 0
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  i = 0
                                  ec = event_comb.succeeded(Sub()
                                                                i += 1
                                                            End Sub).repeat(size)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.equal(i, size)
                                  i = 0
                                  ec = event_comb.one_step(Function() As Boolean
                                                               i += 1
                                                               Return False
                                                           End Function).repeat(size)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_false(ec.end_result())
                                  assertion.equal(i, 1)
                                  i = 0
                                  ec = event_comb.succeeded(Sub()
                                                                i += 1
                                                            End Sub).repeat(size)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(ec.end_result())
                                  assertion.equal(i, size)
                                  i = 0
                                  ec = event_comb.one_step(Function() As Boolean
                                                               i += 1
                                                               Return False
                                                           End Function).repeat(size)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_false(ec.end_result())
                                  assertion.equal(i, 1)
                                  Return goto_end()
                              End Function)
    End Function
End Class
