
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class utt
    Public Shared ReadOnly concurrency As Int32
    Public Shared ReadOnly file_pattern As String
    Public Shared ReadOnly is_current As Boolean

    Shared Sub New()
        If Not env_value(env_keys("utt", "concurrency"), concurrency) OrElse
           concurrency < 0 OrElse
           concurrency > Environment.ProcessorCount() Then
            concurrency = npos
        End If
        If Not env_value(env_keys("utt", "file", "pattern"), file_pattern) OrElse
           file_pattern.null_or_whitespace() Then
            file_pattern = "osi.*.dll"
        End If
        is_current = strsame(application_name, "osi.root.utt")
    End Sub

    Private Sub New()
    End Sub
End Class
