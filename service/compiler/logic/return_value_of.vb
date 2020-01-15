
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Shared between callee and return.
    Public NotInheritable Class return_value_of
        Public Shared Function define(ByVal scope As scope,
                                      ByVal name As String,
                                      ByVal parameters() As pair(Of String, String),
                                      ByVal type As String) As Boolean
            Return scope.define(variable_name(name), array_size(parameters), type)
        End Function

        Public Shared Function retrieve(ByVal scope As scope,
                                        ByVal types As types,
                                        ByVal name As String,
                                        ByRef o As variable) As Boolean
            Return variable.[New](scope, types, variable_name(name), o)
        End Function

        Private Shared Function variable_name(ByVal name As String) As String
            Return strcat("@return_value_of_", name)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
