
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Private ReadOnly accessor As lazier(Of accessor_t) = lazier.of(AddressOf Me.get_accessor)

    Protected Overridable Function get_accessor() As accessor_t
        assert(False)
        Return Nothing
    End Function

    Protected Class accessor_t
        ' TODO: Return type_aliases
        Public Overridable Function type_alias(ByVal i As String) As String
            assert(False)
            Return Nothing
        End Function

        Public ReadOnly current_function As lazier(Of current_function_accessor) =
                lazier.of(AddressOf Me.get_current_function)

        Public Class current_function_accessor
            Public Overridable Function getter() As current_function_proxy
                assert(False)
                Return Nothing
            End Function

            Public Overridable Sub setter(ByVal c As current_function_t)
                assert(False)
            End Sub
        End Class

        Protected Overridable Function get_current_function() As current_function_accessor
            assert(False)
            Return Nothing
        End Function

        ' TODO: Return string
        Public Overridable Function current_function_name() As [optional](Of String)
            Return (+current_function).getter().name_opt()
        End Function

        ' TODO: Return struct_proxy.
        Public Overridable Function is_struct_type(ByVal i As String) As Boolean
            assert(False)
            Return False
        End Function

        Public Overridable Function delegates() As delegate_proxy
            assert(False)
            Return Nothing
        End Function
    End Class
End Class
