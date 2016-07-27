
Imports osi.root.utt

Public Class type_attribute_perf_test
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(chained(New type_attribute_test(),
                                  New type_attribute_forward_test(),
                                  New type_attribute_signal_test()), 1024 * 64))
    End Sub
End Class
