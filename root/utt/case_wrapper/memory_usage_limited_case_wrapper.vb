
Imports osi.root.connector
Imports osi.root.envs

' A temporary solution to disable cases on lowend machines.
Public Class memory_usage_limited_case_wrapper
    Inherits case_wrapper

    Private ReadOnly expected_memory_usage As Int64

    Public Sub New(ByVal c As [case], ByVal expected_memory_usage As Int64)
        MyBase.New(c)
        Me.expected_memory_usage = expected_memory_usage
    End Sub

    Private Function not_exceed_memory_limit() As Boolean
        Return max_physical_memory_usage >= expected_memory_usage AndAlso
               max_virtual_memory_usage >= expected_memory_usage
    End Function

    Public NotOverridable Overrides Function prepare() As Boolean
        If not_exceed_memory_limit() Then
            Return MyBase.prepare()
        Else
            Return True
        End If
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        If not_exceed_memory_limit() Then
            Return MyBase.run()
        Else
            raise_error("expected memory usage of case ", name, " is larger than physical limitation, ignore")
            Return True
        End If
    End Function

    Public NotOverridable Overrides Function finish() As Boolean
        If not_exceed_memory_limit() Then
            Return MyBase.finish()
        Else
            Return True
        End If
    End Function
End Class
