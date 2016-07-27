
Imports osi.root.connector

Public Class exec_failure_case_1
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(New exec_failure_case())
    End Sub

    Friend Class exec_failure_case
        Inherits exec_case

        Public Sub New()
            MyBase.New(guid_str())
        End Sub
    End Class
End Class
