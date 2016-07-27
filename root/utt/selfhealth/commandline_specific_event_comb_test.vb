
Imports osi.root.procedure

Public Class commandline_specific_event_comb_test
    Inherits commandline_specific_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(New failure_case())
    End Sub

    Private Class failure_case
        Inherits event_comb_case

        Public Overrides Function create() As event_comb
            Return New event_comb(Function() As Boolean
                                      Return assert_true(False,
                                                         "should never called only when ",
                                                         "commandline specific commandline_specific_event_comb_test")
                                  End Function)
        End Function
    End Class
End Class
