
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Public Class accessor
        ' TODO: Return type_alias_proxy.
        Public Overridable Function type_alias(ByVal s As scope, ByVal i As String) As String
            assert(False)
            Return Nothing
        End Function

        Public Class current_function_proxy
            Public Overridable Function ctor(ByVal name As String,
                                             ByVal return_type As String,
                                             ByVal params As vector(Of builders.parameter)) As current_function_t
                assert(False)
                Return Nothing
            End Function

            Public Overridable Function getter(ByVal s As scope) As current_function_t
                assert(False)
                Return Nothing
            End Function

            Public Overridable Sub setter(ByVal s As scope, ByVal c As current_function_t)
                assert(False)
            End Sub
        End Class

        Public Overridable Function current_function(ByVal s As scope) As current_function_proxy
            assert(False)
            Return Nothing
        End Function

        ' TODO: Return struct_proxy.
        Public Overridable Function is_struct_type(ByVal s As scope, ByVal i As String) As Boolean
            assert(False)
            Return False
        End Function

        Public Overridable Function [delegate](ByVal s As scope) As delegate_t
            assert(False)
            Return Nothing
        End Function
    End Class
End Class
