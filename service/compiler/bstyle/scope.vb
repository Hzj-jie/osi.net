
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly d As define_t
        Private ReadOnly ta As New type_alias_t()
        Private ReadOnly s As New struct_t()
        Private ReadOnly v As New variable_t()
        Private ReadOnly f As function_t
        Private ReadOnly fc As call_hierarchy_t
        Private ReadOnly vt As New value_target_t()
        Private ReadOnly ps As New params_t()
        Private cf As current_function_t
        Private ReadOnly de As New delegate_t()

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            d = New define_t()
            f = New function_t()
            fc = New call_hierarchy_t()
        End Sub
    End Class
End Class
