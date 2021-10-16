
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class scope
        Inherits scope(Of scope)

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub
    End Class

    Public NotInheritable Class scope_wrapper
        Inherits scope_wrapper(Of scope)

        Public Sub New(ByVal scope As scope)
            MyBase.New(scope)
        End Sub
    End Class
End Class
