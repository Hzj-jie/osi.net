
Imports osi.root.connector

Partial Public MustInherit Class expiration_controller
    Private NotInheritable Class func_bool
        Inherits expiration_controller

        Private ReadOnly stopping As Func(Of Boolean)

        Public Sub New(ByVal stopping As Func(Of Boolean))
            assert(stopping IsNot Nothing)
            Me.stopping = stopping
        End Sub

        Public Overrides Function expired() As Boolean
            Return stopping()
        End Function
    End Class
End Class
