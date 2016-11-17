
Imports osi.root.constants
Imports osi.root.connector

Public Module _utt
    Public ReadOnly utt_concurrency As Int32
    Public ReadOnly utt_file_pattern As String

    Sub New()
        If Not env_value(env_keys("utt", "concurrency"), utt_concurrency) OrElse
           utt_concurrency < 0 OrElse
           utt_concurrency > Environment.ProcessorCount() Then
            utt_concurrency = npos
        End If
        If Not env_value(env_keys("utt", "file", "pattern"), utt_file_pattern) OrElse
           utt_file_pattern.null_or_whitespace() Then
            utt_file_pattern = "osi.*.dll"
        End If
    End Sub
End Module
