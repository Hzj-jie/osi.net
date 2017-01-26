
Imports osi.root.connector

Namespace logic
    ' Shared between callee and return.
    Public NotInheritable Class return_value_of
        Public Shared Function define(ByVal scope As scope, ByVal name As String) As Boolean
            Return scope_define(scope, variable_name(name), types.variable_type)
        End Function

        Public Shared Function retrieve(ByVal scope As scope,
                                        ByVal name As String,
                                        ByRef o As variable) As Boolean
            Return variable.[New](scope, variable_name(name), o)
        End Function

        Private Shared Function variable_name(ByVal name As String) As String
            Return strcat("@return_value_of_", name)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
