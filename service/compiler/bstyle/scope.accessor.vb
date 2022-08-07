
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Protected Overrides Function get_accessor() As scope(Of scope).accessor_t
            Return New accessor_t(Me)
        End Function

        Protected Shadows Class accessor_t
            Inherits scope(Of scope).accessor_t

            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Overrides Function includes() As includes_t
                Return s.incs
            End Function

            Public Overrides Function defines() As define_t
                Return s.d
            End Function

            Public Overrides Function type_alias() As type_alias_t
                Return s.ta
            End Function

            Public Overrides Function current_function() As current_function_t
                Return s.cf
            End Function

            Public Overrides Sub current_function(ByVal c As current_function_t)
                s.cf = c
            End Sub

            ' TODO: Return struct_proxy.
            Public Overrides Function is_struct_type(ByVal i As String) As Boolean
                Return s.structs().types().defined(i)
            End Function

            Public Overrides Function delegates() As delegate_t
                Return s.de
            End Function
        End Class
    End Class
End Class
