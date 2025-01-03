
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.constructor
Imports typed_node_writer = osi.service.compiler.rewriters.typed_node_writer

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Inherits scope(Of typed_node_writer, code_builder_proxy, code_gens_proxy, scope)

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

            defines().define("B2STYLE")
        End Sub

        Public Shared Function wrap() As scope
            Return current().start_scope()
        End Function

        Protected Overrides Function get_accessor() As _
                scope(Of typed_node_writer, code_builder_proxy, code_gens_proxy, scope).accessor_t
            Return New accessor_t(Me)
        End Function

        Private NotInheritable Shadows Class accessor_t
            Inherits scope(Of typed_node_writer, code_builder_proxy, code_gens_proxy, scope).accessor_t

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

            Public Overrides Function call_hierarchy() As _
                    scope(Of typed_node_writer, code_builder_proxy, code_gens_proxy, scope).call_hierarchy_t
                Return s.fc
            End Function

            Public Overrides Function current_namespace() As current_namespace_t
                Return s.cn
            End Function

            Public Overrides Function variables() As variable_t
                Return s.v
            End Function

            Public Overrides Function root_type_injector() As root_type_injector_t
                Return s.i
            End Function

            Public Overrides Function classes() As class_t
                Return s.c
            End Function

            Public Overrides Function template() As template_t
                Return s.t
            End Function
        End Class

        Protected Overrides Function get_features() As _
                scope(Of typed_node_writer, code_builder_proxy, code_gens_proxy, scope).features_t
            Return New features_t()
        End Function

        Protected NotInheritable Shadows Class features_t
            Inherits scope(Of typed_node_writer, code_builder_proxy, code_gens_proxy, scope).features_t

            Public Overrides Function with_type_alias() As Boolean
                Return False
            End Function
        End Class
    End Class
End Class
