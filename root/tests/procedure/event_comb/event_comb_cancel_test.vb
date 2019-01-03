
Imports osi.root
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt

Public Class event_comb_cancel_test
    Inherits multi_procedure_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New event_comb_cancel_case, 1024 * 4), Environment.ProcessorCount() << 4)
    End Sub

    Private Class event_comb_cancel_case
        Inherits utt.event_comb_case

        Private Shared Function dummy_event_comb() As event_comb
            Return New event_comb(Function() As Boolean
                                      fake_processor_work(rnd_int(-1, 3))
                                      Return waitfor(rnd_int(0, 10))
                                  End Function)
        End Function

        Private Shared Function cancel_event_comb(ByVal ec As event_comb) As event_comb
            assert(Not ec Is Nothing)
            Return New event_comb(Function() As Boolean
                                      Return waitfor(Function() Not ec.not_started()) AndAlso
                                             waitfor(rnd_int(0, 20)) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.working())
                                      ec.cancel()
                                      assertion.is_true(ec.ending() OrElse ec.end())
                                      assertion.is_false(ec.working())
                                      assertion.is_false(ec.end_result())
                                      If rnd_bool() Then
                                          Return waitfor(Function() ec.end()) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end())
                                      ec.cancel()
                                      assertion.is_false(ec.ending())
                                      assertion.is_true(ec.end())
                                      assertion.is_false(ec.working())
                                      assertion.is_false(ec.end_result())
                                      Return goto_end()
                                  End Function)
        End Function

        Public Overrides Function create() As event_comb
            Dim cec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      Dim ec As event_comb = Nothing
                                      ec = dummy_event_comb()
                                      assert_begin(ec)
                                      cec = cancel_event_comb(ec)
                                      Return waitfor(cec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return cec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function
    End Class
End Class
