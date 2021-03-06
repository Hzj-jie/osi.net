﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public Module _processor_affinity
    Public ReadOnly processor_affinity As Int32 = calculate_processor_affinity()

    Private Function calculate_processor_affinity() As int32
        Dim processor_affinity As int32
        If Not (env_value(env_keys("processor", "affinity"), processor_affinity) OrElse
                env_value(env_keys("affinity"), processor_affinity)) OrElse
           processor_affinity < 0 OrElse
           processor_affinity >= Environment.ProcessorCount() Then
            processor_affinity = npos
        End If
        Return processor_affinity
    End Function
End Module
