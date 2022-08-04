
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly incs As includes_t
        Private ReadOnly fc As call_hierarchy_t
        Private ReadOnly d As define_t
        Private ReadOnly ta As New type_alias_t()
        Private ReadOnly s As New struct_t()
        Private ReadOnly v As New variable_t()
        Private ReadOnly f As function_t
        Private ReadOnly vt As New value_target_t()
        Private ReadOnly ps As New params_t()
        Private cf As current_function_t
        Private ReadOnly de As New delegate_t()
        Private ReadOnly t As temp_logic_name_t

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            incs = New includes_t()
            fc = New call_hierarchy_t(Me)
            d = New define_t()
            f = New function_t(Me)
            t = New temp_logic_name_t()
        End Sub

        Public Function includes() As includes_t
            Return from_root(Function(ByVal i As scope) As includes_t
                                 assert(Not i Is Nothing)
                                 Return i.incs
                             End Function)
        End Function

        Public Function current_function() As current_function_proxy
            Return New current_function_proxy(Me)
        End Function

        Public Function defines() As define_t
            Return from_root(Function(ByVal i As scope) As define_t
                                 assert(Not i Is Nothing)
                                 Return i.d
                             End Function)
        End Function

        Public Function delegates() As delegate_proxy
            Return New delegate_proxy(Me)
        End Function

        Public Function functions() As function_t
            Return from_root(Function(ByVal i As scope) As function_t
                                 assert(Not i Is Nothing)
                                 Return i.f
                             End Function)
        End Function

        Public Function params() As params_t
            Return ps
        End Function
    End Class
End Class
