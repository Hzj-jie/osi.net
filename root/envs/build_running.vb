
Imports osi.root.connector

Public Module _build_running
    Public ReadOnly build As String
    Public ReadOnly running_mode As String

    Sub New()
        If isdebugbuild() Then
            build = "debugbuild"
        ElseIf isreleasebuild() Then
            build = "releasebuild"
        Else
            build = "unknownbuild"
        End If

        If isdebugmode() Then
            running_mode = "debugmode"
        Else
            running_mode = "normalmode"
        End If
    End Sub
End Module
