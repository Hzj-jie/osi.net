
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly incs As includes_t
        Private ReadOnly fc As call_hierarchy_t
        Private ReadOnly cn As current_namespace_t
        Private f As String
        Private ReadOnly t As New template_t()
        Private ReadOnly v As New variable_t()
        Private ReadOnly d As define_t
        Private ReadOnly c As New class_t()
        Private ReadOnly i As root_type_injector_t

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            incs = New includes_t()
            fc = New call_hierarchy_t()
            cn = New current_namespace_t()
            d = New define_t()
            i = New root_type_injector_t()
        End Sub

        Public Shared Function wrap() As scope
            Return current().start_scope()
        End Function

        Public Function includes() As includes_t
            Return from_root(Function(ByVal i As scope) As includes_t
                                 assert(Not i Is Nothing)
                                 Return i.incs
                             End Function)
        End Function
    End Class
End Class
