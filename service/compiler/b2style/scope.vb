
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly fc As call_hierarchy_t
        Private cn As String
        Private f As String
        Private ReadOnly t As New template_t()
        Private ReadOnly v As New variable_t()
        Private ReadOnly d As define_t

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            fc = New call_hierarchy_t()
            d = New define_t()
        End Sub

        Public Shared Function wrap() As scope
            Return current().start_scope()
        End Function
    End Class
End Class
