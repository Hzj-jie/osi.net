
Imports osi.root.connector

Namespace logic
    Public NotInheritable Class return_value_of
        Public Shared Function define(ByVal scope As scope, ByVal name As String) As Boolean
            Return scope_define.define(scope, return_value_of(name), types.variable_type)
        End Function

        Public Shared Function retrieve(ByVal scope As scope,
                                        ByVal name As String,
                                        ByRef o As variable) As Boolean
            Return variable.[New](scope, return_value_of(name), o)
        End Function

        Private Shared Function return_value_of(ByVal name As String) As String
            Return strcat("@return_value_of_", name)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
