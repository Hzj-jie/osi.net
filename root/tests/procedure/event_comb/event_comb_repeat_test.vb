
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
                                  ec = event_comb.repeat(size,
                                                         Function() As Boolean
                                                             i += 1
                                                             Return True
                                                         End Function)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(i, size)
                                  i = 0
                                  ec = event_comb.repeat(size,
                                                         Function() As Boolean
                                                             i += 1
                                                             Return False
                                                         End Function)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_false(ec.end_result())
                                  assert_equal(i, 1)
                                  i = 0
                                  ec = event_comb.repeat(size,
                                                         Function() As event_comb
                                                             Return New event_comb(Function() As Boolean
                                                                                       i += 1
                                                                                       Return goto_end()
                                                                                   End Function)
                                                         End Function)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_true(ec.end_result())
                                  assert_equal(i, size)
                                  i = 0
                                  ec = event_comb.repeat(size,
                                                         Function() As event_comb
                                                             Return New event_comb(Function() As Boolean
                                                                                       i += 1
                                                                                       Return False
                                                                                   End Function)
                                                         End Function)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert_false(ec.end_result())
                                  assert_equal(i, 1)
                                  Return goto_end()
                              End Function)
    End Function
End Class
