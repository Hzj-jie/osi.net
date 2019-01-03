
Public Class commandline_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New failure_case())
    End Sub

    Private Class failure_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Return assertion.is_true(False,
                               "should never called only when commandline specific commandline_specified_test")
        End Function
    End Class
End Class
