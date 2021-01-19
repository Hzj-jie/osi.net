
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation

Public NotInheritable Class file_resource
    Private Shared Function find_file(ByVal file As String) As String
        file = IO.Path.Combine(application_directory, file)
        assert(IO.File.Exists(file))
        Return file
    End Function

    Public Shared Function bytes(ByVal file As String) As lazier(Of Byte())
        file = find_file(file)
        Return lazier.of(Function() As Byte()
                             Return IO.File.ReadAllBytes(file)
                         End Function)
    End Function

    Public Shared Function text(ByVal file As String) As lazier(Of String)
        file = find_file(file)
        Return lazier.of(Function() As String
                             Return IO.File.ReadAllText(file)
                         End Function)
    End Function

    Public Shared Function lines(ByVal file As String) As lazier(Of String())
        file = find_file(file)
        Return lazier.of(Function() As String()
                             Return IO.File.ReadAllLines(file)
                         End Function)
    End Function

    Private Sub New()
    End Sub
End Class
