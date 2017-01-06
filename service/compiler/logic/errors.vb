
Imports osi.root.constants
Imports osi.root.connector

Namespace logic
    Public NotInheritable Class errors
        Public Shared Sub variable_undefined(ByVal variable As String)
            raise_error(error_type.user, "Variable ", variable, " is undefined.")
        End Sub

        Public Shared Sub extern_function_undefined(ByVal name As String)
            raise_error(error_type.user, "Extern function ", name, " is undefined.")
        End Sub

        Public Shared Sub unassignable_bool(ByVal variable As String, ByVal type As String)
            raise_error(error_type.user, variable, " [", type, "] is not assignable from bool.")
        End Sub

        Public Shared Sub redefine(ByVal variable As String, ByVal type As String, ByVal last_type As String)
            raise_error(error_type.user, "Variable ", variable, " [", type, "] redefined, last type ", last_type)
        End Sub

        Public Shared Sub unassignable(ByVal target As String,
                                       ByVal target_type As String,
                                       ByVal source As String,
                                       ByVal source_type As String)
            raise_error(error_type.user,
                        "Source ",
                        source,
                        " [",
                        source_type,
                        "] and target ",
                        target,
                        " [",
                        target_type,
                        "] have different size, unassignable.")
        End Sub

        Public Shared Sub unassignable_array(ByVal target As String, ByVal target_type As String, ByVal size As UInt32)
            raise_error(error_type.user,
                        "Target ",
                        target,
                        " [",
                        target_type,
                        "] is not assignable from byte array with size ",
                        size)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
