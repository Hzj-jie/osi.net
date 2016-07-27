
Imports osi.root.constants

Public Module _exit
    Public Sub suicide(Optional ByVal ext_code As Int32 = npos)
        If on_mono() Then
            [exit](ext_code)
        Else
            Environment.FailFast(Convert.ToString(ext_code))
        End If
    End Sub

    Public Sub [exit](Optional ByVal ext_code As Int32 = 0)
        Environment.Exit(ext_code)
    End Sub
End Module
