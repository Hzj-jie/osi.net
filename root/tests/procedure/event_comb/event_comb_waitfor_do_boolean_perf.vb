
Imports System.DateTime
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.connector

Public Class event_comb_waitfor_do_boolean_perf
    Inherits processor_measured_case_wrapper

    Public Sub New()
        MyBase.New(multi_procedure(repeat(New event_comb_waitfor_do_boolean_case(),
                                          10 * If(isdebugbuild(), 1, 2)),
                                   256 * If(isdebugbuild(), 1, 2)))
    End Sub

    Private Class event_comb_waitfor_do_boolean_case
        Inherits osi.root.utt.event_comb_case

        Public Overrides Function create() As event_comb
            Return New event_comb(Function() As Boolean
                                      Dim start_ms As Int64 = 0
                                      start_ms = Now().milliseconds()
                                      Return waitfor(Function() As Boolean
                                                         Return Now().milliseconds() - start_ms >= _
                                                                seconds_to_milliseconds(10)
                                                     End Function) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
