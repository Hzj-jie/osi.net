
Public Class commandline_specific_test
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New failure_case())
    End Sub

    Private Class failure_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Return assert_true(False,
                               "should never called only when commandline specific commandline_specific_test")
        End Function
    End Class
End Class
