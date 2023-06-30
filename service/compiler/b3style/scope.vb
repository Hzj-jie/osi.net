
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.constructor

Partial Public NotInheritable Class b3style
    Partial Public NotInheritable Class scope
        Inherits scope(Of logic_writer, code_builder_proxy, code_gens_proxy, scope)

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
        Private ReadOnly cn As current_namespace_t
        Private ReadOnly tt As New template_t()
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
            d = New define_t()
            f = New function_t()
            t = New temp_logic_name_t()
            cn = New current_namespace_t()
            i = New root_type_injector_t()
        End Sub

        Protected Overrides Function get_accessor() As _
                scope(Of logic_writer, code_builder_proxy, code_gens_proxy, scope).accessor_t
            Return New accessor_t(Me)
        End Function

        Private Shadows Class accessor_t
            Inherits scope(Of logic_writer, code_builder_proxy, code_gens_proxy, scope).accessor_t

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

            Public Overrides Function delegates() As delegate_t
                Return s.de
            End Function

            Public Overrides Function structs() As struct_t
                Return s.s
            End Function

            Public Overrides Function variables() As variable_t
                Return s.v
            End Function

            Public Overrides Function temp_logic_name() As temp_logic_name_t
                Return s.t
            End Function

            Public Overrides Function value_target() As value_target_t
                Return s.vt
            End Function

            Public Overrides Function call_hierarchy() As call_hierarchy_t
                Return s.fc
            End Function

            Public Overrides Function current_namespace() As current_namespace_t
                Return s.cn
            End Function

            Public Overrides Function root_type_injector() As root_type_injector_t
                Return s.i
            End Function

            Public Overrides Function classes() As class_t
                Return s.c
            End Function

            Public Overrides Function template() As template_t
                Return s.tt
            End Function

            Public Overrides Function functions() As function_t
                Return s.f
            End Function

            Public Overrides Function params() As params_t
                Return s.ps
            End Function
        End Class
    End Class
End Class