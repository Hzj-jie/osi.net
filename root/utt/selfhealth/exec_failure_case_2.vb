
Public Class exec_failure_case_2
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New exec_failure_case())
    End Sub

    Friend Class exec_failure_case
        Inherits exec_case_test

        Public Sub New()
            MyBase.New("false")
        End Sub
    End Class
End Class
