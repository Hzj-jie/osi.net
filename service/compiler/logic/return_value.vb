
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Shared between callee, caller and return.
    Public NotInheritable Class return_value
        Public Shared Function define(ByVal name As String,
                                      ByVal type As String) As Boolean
            Return scope.current().define_stack(variable_name(name), type)
        End Function

        Public Shared Function export(ByVal types As types,
                                      ByVal name As String,
                                      ByVal type As String,
                                      ByRef o As vector(Of String)) As Boolean
            Return logic.define.export(types, variable_name(name), type, o)
        End Function

        Public Shared Function retrieve(ByVal types As types,
                                        ByVal name As String,
                                        ByRef o As variable) As Boolean
            Return variable.of_stack(types, variable_name(name), o)
        End Function

        Private Shared Function variable_name(ByVal name As String) As String
            Return strcat("@return_value_of_", name, "_place_holder")
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
