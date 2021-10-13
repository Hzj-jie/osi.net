
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Namespace logic
    Public NotInheritable Class errors
        Public Shared Sub variable_undefined(ByVal variable As String)
            raise_error(error_type.user, "Variable ", variable, " is undefined.")
        End Sub

        Public Shared Sub interrupt_undefined(ByVal name As String)
            raise_error(error_type.user, "Interrupt function ", name, " is undefined.")
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
                        "] have different sizes, unassignable.")
        End Sub

        Public Shared Sub unassignable_zero_type(ByVal target As variable)
            assert(Not target Is Nothing)
            raise_error(error_type.user,
                        "target ",
                        target.name,
                        " [",
                        target.type,
                        "] is zero-size, unassignable.")
        End Sub

        Public Shared Sub no_return_value_provided(ByVal target As variable)
            assert(Not target Is Nothing)
            raise_error(error_type.user,
                        "target ",
                        target.name,
                        " [",
                        target.type,
                        "] as return value is expected but not provided.")
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

        Public Shared Sub type_alias_redefined(ByVal [alias] As String,
                                               ByVal canonical As String,
                                               ByVal last_type As String)
            raise_error(error_type.user,
                        "Alias ",
                        [alias],
                        " of canonical type ",
                        canonical,
                        " has been defined already as ",
                        last_type,
                        ".")
        End Sub

        Public Shared Sub type_undefined(ByVal type As String, ByVal target As String)
            raise_error(error_type.user, "Type ", type, " referred by target ", target, " is undefined.")
        End Sub

        Public Shared Sub type_undefined(ByVal type As String)
            raise_error(error_type.user, "Type ", type, " is undefined.")
        End Sub

        Public Shared Sub anchor_undefined(ByVal name As String, ByVal origin As String)
            raise_error(error_type.user, "Anchor ", name, " derived from ", origin, " is not defined.")
        End Sub

        Public Shared Sub anchor_undefined(ByVal name As String)
            raise_error(error_type.user, "Anchor ", name, " is not defined.")
        End Sub

        Public Shared Sub anchor_redefined(ByVal name As String, ByVal current_pos As UInt32, ByVal last_pos As UInt32)
            raise_error(error_type.user,
                        "Anchor ",
                        name,
                        " redefined at ",
                        current_pos,
                        ", last position is ",
                        last_pos)
        End Sub

        Public Shared Sub unexpected_token(ByVal s As String)
            raise_error(error_type.user, "Unexpected token ", s)
        End Sub

        Public Shared Sub typed_parameters_is_not_closed()
            raise_error(error_type.user, "Right parenthesis is not found, typed_parameters is not closed.")
        End Sub

        Public Shared Sub parameters_is_not_closed()
            raise_error(error_type.user, "Right parenthesis is not found, parameters is not closed.")
        End Sub

        Public Shared Sub paragraph_is_not_closed()
            raise_error(error_type.user, "Right bracket is not found, paragraph is not closed.")
        End Sub

        Public Shared Sub mismatch_callee_parameters(ByVal callee_name As String, ByVal parameters() As String)
            raise_error(error_type.user,
                        "Parameters [",
                        parameters,
                        "] does not match the parameters of callee ",
                        callee_name)
        End Sub

        Public Shared Sub unexpected_macro(ByVal n As String)
            raise_error(error_type.user, "Unexpected format of macro ", n)
        End Sub

        Public Shared Sub unknown_macro(ByVal n As String, ByVal s As String)
            raise_error(error_type.user, "Unknown macro ", n, " against ", s)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
