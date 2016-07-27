
Imports osi.root.constants

Public Module _processor_affinity
    Public ReadOnly processor_affinity As Int32

    Sub New()
        If Not (env_value(env_keys("processor", "affinity"), processor_affinity) OrElse
                env_value(env_keys("affinity"), processor_affinity)) OrElse
           processor_affinity < 0 OrElse
           processor_affinity >= Environment.ProcessorCount() Then
            processor_affinity = npos
        End If
    End Sub
End Module
