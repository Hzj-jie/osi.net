
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.resource

Public NotInheritable Class zh
    Public Shared Function words() As lazier(Of vector(Of String))
        Return file_resource.text("resources\zh\words.txt").
                             map(Function(ByVal i As String) As vector(Of String)
                                     Return vector.of(i.Split({character.newline},
                                                              StringSplitOptions.RemoveEmptyEntries))
                                 End Function)
    End Function

    Private Sub New()
    End Sub
End Class