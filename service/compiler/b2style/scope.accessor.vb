
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class b2style
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

            Public Overrides Function current_function_name() As [optional](Of String)
                Return scope.current().current_function().name()
            End Function
        End Class
    End Class
End Class
