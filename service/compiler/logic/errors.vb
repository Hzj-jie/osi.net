
Option Strict On

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

        Public Shared Sub unassignable_from_bool(ByVal v As variable)
            assert(Not v Is Nothing)
            raise_error(error_type.user, "Variable ", v.name, " [", v.type, "] is not assignable from bool.")
        End Sub

        Public Shared Sub unassignable_from_uint32(ByVal v As variable)
            assert(Not v Is Nothing)
            raise_error(error_type.user, "Variable ", v.name, " [", v.type, "] is not assignable from uint32.")
        End Sub

        Public Shared Sub unassignable_to_uint32(ByVal v As variable)
            assert(Not v Is Nothing)
            raise_error(error_type.user, "Variable ", v.name, " [", v.type, "] is not assignable to uint32.")
        End Sub

        Public Shared Sub redefine(ByVal name As String, ByVal type As String, ByVal last_type As String)
            raise_error(error_type.user, "Variable ", name, " [", type, "] redefined, last type ", last_type)
        End Sub

        Public Shared Sub unassignable(ByVal target As variable, ByVal source As variable)
            assert(Not target Is Nothing)
            assert(Not source Is Nothing)
            raise_error(error_type.user,
                        "Source ",
                        source.name,
                        " [",
                        source.type,
                        "] and target ",
                        target.name,
                        " [",
                        target.type,
                        "] have different size, unassignable.")
        End Sub

        Public Shared Sub unassignable_array(ByVal target As variable, ByVal size As UInt32)
            assert(Not target Is Nothing)
            raise_error(error_type.user,
                        "Target ",
                        target.name,
                        " [",
                        target.type,
                        "] is not assignable from byte array with size ",
                        size)
        End Sub

        Public Shared Sub unassignable_variable_size(ByVal target As variable)
            assert(Not target Is Nothing)
            raise_error(error_type.user,
                        "Target ",
                        target.name,
                        " [",
                        target.type,
                        "] is not a variable size type.")
        End Sub

        Public Shared Sub type_undefined(ByVal type As String, ByVal target As String)
            raise_error(error_type.user, "Type ", type, " referred by target ", target, " is undefined.")
        End Sub

        Public Shared Sub anchor_undefined(ByVal name As String)
            raise_error(error_type.user, "Anchor ", name, " is not defined.")
        End Sub

        Public Shared Sub anchor_redefined(ByVal name As String, ByVal last_pos As UInt32)
            raise_error(error_type.user, "Anchor ", name, " redefined, last position is ", last_pos)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
