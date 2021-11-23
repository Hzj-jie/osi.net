
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private cn As String

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Shared Function wrapper() As scope_wrapper
            Return New scope_wrapper()
        End Function
    End Class

    Public NotInheritable Class scope_wrapper
        Inherits scope_wrapper(Of scope)

        Public Sub New()
            MyBase.New(b2style.scope.current())
        End Sub
    End Class
End Class
