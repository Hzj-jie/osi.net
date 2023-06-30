
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Module _processor_affinity
    Public ReadOnly processor_affinity As Int32 =
        Function() As Int32
            Dim processor_affinity As Int32
            If Not (env_value(env_keys("processor", "affinity"), processor_affinity) OrElse
                    env_value(env_keys("affinity"), processor_affinity)) OrElse
               processor_affinity < 0 OrElse
               processor_affinity >= Environment.ProcessorCount() Then
                Return npos
            End If
            Return processor_affinity
        End Function()
End Module
