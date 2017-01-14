
Imports osi.root.connector

Namespace logic
    Public NotInheritable Class scope_define
        Public Shared Function define(ByVal scope As scope, ByVal name As String, ByVal type As String) As Boolean
            assert(Not scope Is Nothing)
            If scope.define(name, type) Then
                Return True
            Else
                errors.redefine(name, type, scope.type(name))
                Return False
            End If
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
