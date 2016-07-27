
Imports osi.root.constants

Public Module _utt
    Public ReadOnly utt_concurrency As Int32

    Sub New()
        If Not env_value(env_keys("utt", "concurrency"), utt_concurrency) OrElse
           utt_concurrency < 0 OrElse
           utt_concurrency > Environment.ProcessorCount() Then
            utt_concurrency = npos
        End If
    End Sub
End Module
