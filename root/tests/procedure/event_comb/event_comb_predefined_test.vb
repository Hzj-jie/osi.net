
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class event_comb_predefined_test
    <test>
    Private Shared Sub suppress_error()
        assertion.is_false(async_sync(New event_comb(Function() As Boolean
                                                   Return False
                                               End Function)))
        assertion.is_true(async_sync(New event_comb(Function() As Boolean
                                                  Return False
                                              End Function).suppress_error()))
        assertion.is_false(async_sync(New event_comb(Function() As Boolean
                                                   Return waitfor_yield() AndAlso
                                                          goto_next()
                                               End Function,
                                               Function() As Boolean
                                                   Return False
                                               End Function)))
        assertion.is_true(async_sync(New event_comb(Function() As Boolean
                                                  Return waitfor_yield() AndAlso
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  Return False
                                              End Function).suppress_error()))
        Dim ec As event_comb = Nothing
        ec = event_comb.failed()
        For i As Int32 = 0 To 10
            assertion.is_true(async_sync(ec.suppress_error()))
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
