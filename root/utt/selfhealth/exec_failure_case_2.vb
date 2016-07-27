
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Class exec_failure_case_2
    Inherits commandline_specific_case_wrapper

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
