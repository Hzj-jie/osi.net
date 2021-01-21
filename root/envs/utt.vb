
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class utt
    Public Shared ReadOnly concurrency As Int32 = calculate_concurrency()
    Public Shared ReadOnly file_pattern As String = calculate_file_pattern()
    Public Shared ReadOnly is_current As Boolean = strsame(application_name, "osi.root.utt")

    Private Shared Function calculate_concurrency() As int32
        Dim concurrency As int32
        If Not env_value(env_keys("utt", "concurrency"), concurrency) OrElse
           concurrency < 0 OrElse
           concurrency > Environment.ProcessorCount() Then
            concurrency = npos
        End If
        Return concurrency
    End Function

    Private Shared Function calculate_file_pattern() As String
        Dim file_pattern As String = Nothing
        If Not env_value(env_keys("utt", "file", "pattern"), file_pattern) OrElse
           file_pattern.null_or_whitespace() Then
            file_pattern = "osi.*.dll"
        End If
        Return file_pattern
    End Function

    Private Sub New()
    End Sub
End Class
