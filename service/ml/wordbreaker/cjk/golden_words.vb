
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class golden_words
    Public Shared Function zh() As unordered_set(Of String)
        Return (+service.resources.zh.words()).stream().
                                               filter(Function(ByVal i As String) As Boolean
                                                          Return i.strlen() > 1
                                                      End Function).
                                               collect_to(Of unordered_set(Of String))()
    End Function

    Private Sub New()
    End Sub
End Class
