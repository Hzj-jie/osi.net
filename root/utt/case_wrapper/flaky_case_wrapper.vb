
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class flaky_case_wrapper
    Inherits case_wrapper

    Public Sub New(ByVal c As [case])
        MyBase.New(c)
    End Sub

    Public NotOverridable Overrides Function prepare() As Boolean
        If commandline_specified() OrElse env_vars.run_flaky_tests Then
            Return MyBase.prepare()
        Else
            Return True
        End If
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        If commandline_specified() OrElse env_vars.run_flaky_tests Then
            Return MyBase.run()
        Else
            raise_error("Ignore the flaky test case ", full_name)
            Return True
        End If
    End Function

    Public NotOverridable Overrides Function finish() As Boolean
        If commandline_specified() OrElse env_vars.run_flaky_tests Then
            Return MyBase.finish()
        Else
            Return True
        End If
    End Function
End Class
