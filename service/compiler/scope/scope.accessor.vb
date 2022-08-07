
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Private ReadOnly _accessor As lazier(Of accessor_t) = lazier.of(AddressOf Me.get_accessor)

    Private Function accessor() As accessor_t
        Return +_accessor
    End Function

    Protected Overridable Function get_accessor() As accessor_t
        assert(False)
        Return Nothing
    End Function

    Protected Class accessor_t
        Public Overridable Function includes() As includes_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function defines() As define_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function type_alias() As type_alias_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function current_function() As current_function_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Sub current_function(ByVal c As current_function_t)
            assert(False)
        End Sub

        ' TODO: Return string
        Public Overridable Function current_function_name() As [optional](Of String)
            Return scope(Of T).current().current_function().name_opt()
        End Function

        ' TODO: Return struct_proxy.
        Public Overridable Function is_struct_type(ByVal i As String) As Boolean
            assert(False)
            Return False
        End Function

        Public Overridable Function delegates() As delegate_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function structs() As struct_t
            assert(False)
            Return Nothing
        End Function
    End Class
End Class
