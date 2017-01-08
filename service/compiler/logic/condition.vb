
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public Class condition
        Implements exportable

        Private ReadOnly v As String
        Private ReadOnly true_path As paragraph
        Private ReadOnly false_path As paragraph

        Public Sub New(ByVal v As String,
                       ByVal true_path As paragraph,
                       Optional ByVal false_path As paragraph = Nothing)
            assert(Not String.IsNullOrEmpty(v))
            assert(Not true_path Is Nothing)
            Me.v = v
            Me.true_path = true_path
            Me.false_path = false_path
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export

        End Function
    End Class
End Namespace
