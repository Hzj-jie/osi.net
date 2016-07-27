
Imports osi.root.procedure

Public Class empty_datafetcher
    Implements idatafetcher

    Public Shared ReadOnly instance As empty_datafetcher

    Shared Sub New()
        instance = New empty_datafetcher()
    End Sub

    Private Sub New()
    End Sub

    Public Function fetch(ByVal localfile As String) As event_comb Implements idatafetcher.fetch
        Return event_comb.succeeded()
    End Function
End Class
