
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class errors
        Public Shared Sub raise(ByVal ParamArray msg() As Object)
            raise_error(error_type.user, "[Logic] ", msg)
        End Sub

        Public Shared Sub variable_undefined(ByVal variable As String)
            raise("Variable ", variable, " is undefined.")
        End Sub

        Public Shared Sub interrupt_undefined(ByVal name As String)
            raise("Interrupt function ", name, " is undefined.")
        End Sub

        Public Shared Sub unassignable_from_bool(ByVal v As variable)
            assert(Not v Is Nothing)
            raise("Variable ", v.name, " [", v.type, "] is not assignable from bool.")
        End Sub

        Public Shared Sub unassignable_from_uint32(ByVal v As variable)
            assert(Not v Is Nothing)
            raise("Variable ", v.name, " [", v.type, "] is not assignable from uint32.")
        End Sub

        Public Shared Sub unassignable_to_uint32(ByVal v As variable)
            assert(Not v Is Nothing)
            raise("Variable ", v.name, " [", v.type, "] is not assignable to uint32.")
        End Sub

        Public Shared Sub redefine(ByVal name As String, ByVal type As String, ByVal last_type As String)
            raise("Variable ", name, " [", type, "] redefined, last type ", last_type)
        End Sub

        Public Shared Sub unassignable(ByVal target As variable, ByVal source As variable)
            assert(Not target Is Nothing)
            assert(Not source Is Nothing)
            raise("Source ",
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
            raise("target ",
                  target.name,
                  " [",
                  target.type,
                  "] is zero-size, unassignable.")
        End Sub

        Public Shared Sub no_return_value_provided(ByVal target As variable)
            assert(Not target Is Nothing)
            raise("target ",
                  target.name,
                  " [",
                  target.type,
                  "] as return value is expected but not provided.")
        End Sub

        Public Shared Sub unassignable_array(ByVal target As variable, ByVal size As UInt32)
            assert(Not target Is Nothing)
            raise("Target ",
                  target.name,
                  " [",
                  target.type,
                  "] is not assignable from byte array with size ",
                  size)
        End Sub

        Public Shared Sub unassignable_variable_size(ByVal target As variable)
            assert(Not target Is Nothing)
            raise("Target ",
                  target.name,
                  " [",
                  target.type,
                  "] is not a variable size type.")
        End Sub

        Public Shared Sub type_undefined(ByVal type As String, ByVal target As String)
            raise("Type ", type, " referred by target ", target, " is undefined.")
        End Sub

        Public Shared Sub type_undefined(ByVal type As String)
            raise("Type ", type, " is undefined.")
        End Sub

        Public Shared Sub anchor_undefined(ByVal name As String)
            raise("Anchor ", name, " is not defined.")
        End Sub

        Public Shared Sub anchor_ref_undefined(ByVal name As String)
            raise("AnchorRef ", name, " is not defined.")
        End Sub

        Public Shared Sub anchor_redefined(ByVal name As String,
                                           ByVal current_pos As data_ref,
                                           ByVal last_pos As data_ref)
            raise("Anchor ",
                  name,
                  " redefined at ",
                  current_pos,
                  ", last position is ",
                  last_pos)
        End Sub

        Public Shared Sub anchor_ref_redefined(ByVal name As String, ByVal last As scope.anchor_ref)
            raise("AnchorRef ", name, " has been defined already as ", last)
        End Sub

        Public Shared Sub unexpected_token(ByVal s As String)
            raise("Unexpected token ", s)
        End Sub

        Public Shared Sub typed_parameters_is_not_closed()
            raise("Right parenthesis is not found, typed_parameters is not closed.")
        End Sub

        Public Shared Sub parameters_is_not_closed()
            raise("Right parenthesis is not found, parameters is not closed.")
        End Sub

        Public Shared Sub paragraph_is_not_closed()
            raise("Right bracket is not found, paragraph is not closed.")
        End Sub

        Public Shared Sub mismatch_callee_parameters(ByVal callee_name As String, ByVal parameters() As String)
            raise("Parameters [",
                  parameters,
                  "] does not match the parameters of callee ",
                  callee_name)
        End Sub

        Public Shared Sub invalid_variable_name(ByVal name As String, ByVal ParamArray msgs() As Object)
            raise("Variable name ", name, " is invalid. ", msgs)
        End Sub

        Public Shared Sub not_a_stack_var(ByVal name As String)
            raise("Variable ", name, " is not a variable on stack, e.g. it's informat of var[index].")
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
