
Imports osi.root.connector

Partial Public MustInherit Class expiration_controller
    Private NotInheritable Class se
        Inherits expiration_controller.settable

        Private ReadOnly stopping As pointer(Of singleentry)

        Public Sub New(ByVal stopping As pointer(Of singleentry))
            assert(Not stopping Is Nothing)
            Me.stopping = stopping
        End Sub

        Public Sub New()
            Me.New(New pointer(Of singleentry)())
        End Sub

        Public Overrides Function [stop]() As Boolean
            Return stopping.mark_in_use()
        End Function

        Public Overrides Function expired() As Boolean
            Return stopping.in_use()
        End Function
    End Class
End Class
