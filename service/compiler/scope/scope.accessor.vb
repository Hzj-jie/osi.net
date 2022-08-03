
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Private ReadOnly acc As lazier(Of accessor_t) = lazier.of(AddressOf Me.accessor)

    Protected Overridable Function accessor() As accessor_t
        assert(False)
        Return Nothing
    End Function

    Protected Class accessor_t
        ' TODO: Return type_aliases
        Public Overridable Function type_alias(ByVal i As String) As String
            assert(False)
            Return Nothing
        End Function

        Public Class current_function_accessor
            Public Overridable Function ctor(ByVal name As String,
                                             ByVal return_type As String,
                                             ByVal params As vector(Of builders.parameter)) As current_function_t
                assert(False)
                Return Nothing
            End Function

            Public Overridable Function getter() As current_function_t
                assert(False)
                Return Nothing
            End Function

            Public Overridable Sub setter(ByVal c As current_function_t)
                assert(False)
            End Sub
        End Class

        Public Overridable Function current_function() As current_function_accessor
            assert(False)
            Return Nothing
        End Function

        ' TODO: Return struct_proxy.
        Public Overridable Function is_struct_type(ByVal i As String) As Boolean
            assert(False)
            Return False
        End Function

        Public Overridable Function [delegate]() As delegate_t
            assert(False)
            Return Nothing
        End Function
    End Class
End Class
