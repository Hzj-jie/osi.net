﻿
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
            fc = New call_hierarchy_t()
            d = New define_t()
            f = New function_t()
            t = New temp_logic_name_t()
        End Sub

        Public Function functions() As function_t
            Return from_root(Function(ByVal i As scope) As function_t
                                 assert(Not i Is Nothing)
                                 Return i.f
                             End Function)
        End Function

        Public Function params() As params_t
            Return ps
        End Function

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

            Public Overrides Function type_alias() As type_alias_t
                Return s.ta
            End Function

            Public Overrides Function current_function() As current_function_t
                Return s.cf
            End Function

            Public Overrides Sub current_function(ByVal c As current_function_t)
                s.cf = c
            End Sub

            ' TODO: Return struct_proxy.
            Public Overrides Function is_struct_type(ByVal i As String) As Boolean
                Return s.structs().types().defined(i)
            End Function

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

            Public Overrides Function call_hierarchy() As scope(Of scope).call_hierarchy_t
                Return s.fc
            End Function
        End Class
    End Class
End Class
