
Imports System.IO
Imports System.Text
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.delegates
Imports osi.root.threadpool
Imports osi.root.connector

Public Class content_dataloader
    Inherits streamreader_dataloader(Of String)

    Public Shared ReadOnly instance As content_dataloader

    Shared Sub New()
        instance = New content_dataloader()
    End Sub

    Private Sub New()
    End Sub

    Protected NotOverridable Overrides Function load(ByVal s As StreamReader,
                                                     ByRef r As String) As Boolean
        Try
            r = s.ReadToEnd()
            Return True
        Catch
            Return False
        End Try
    End Function
End Class
