
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

            Public Overrides Function type_alias(ByVal i As String) As String
                Return s.type_alias()(i)
            End Function

            Public Shadows Class current_function_accessor
                Inherits scope(Of scope).accessor_t.current_function_accessor

                Private ReadOnly s As scope

                Public Sub New(ByVal s As scope)
                    assert(Not s Is Nothing)
                    Me.s = s
                End Sub

                Public Overrides Function getter() As current_function_proxy
                    Return scope.current().current_function()
                End Function

                Public Overrides Sub setter(ByVal c As current_function_t)
                    s.cf = c
                End Sub
            End Class

            Protected Overrides Function get_current_function() As scope(Of scope).accessor_t.current_function_accessor
                Return New current_function_accessor(s)
            End Function

            ' TODO: Return struct_proxy.
            Public Overrides Function is_struct_type(ByVal i As String) As Boolean
                Return s.structs().types().defined(i)
            End Function

            Public Overrides Function delegates() As delegate_proxy
                Return s.delegates()
            End Function
        End Class
    End Class
End Class
