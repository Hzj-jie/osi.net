
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Private ReadOnly _accessor As lazier(Of accessor_t) = lazier.of(AddressOf Me.get_accessor)

    Private Function myself() As accessor_t
        Return +_accessor
    End Function

    Protected Overridable Function get_accessor() As accessor_t
        assert(False)
        Return Nothing
    End Function

    Protected Class accessor_t
        Public Overridable Function includes() As includes_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function defines() As define_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function type_alias() As type_alias_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function current_function() As current_function_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Sub current_function(ByVal c As current_function_t)
            assert(False)
        End Sub

        Public Overridable Function current_function_name() As [optional](Of String)
            Return scope(Of T).current().current_function().name()
        End Function

        Public Overridable Function delegates() As delegate_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function structs() As struct_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function variables() As variable_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function temp_logic_name() As temp_logic_name_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function value_target() As value_target_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function call_hierarchy() As call_hierarchy_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function current_namespace() As current_namespace_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function root_type_injector() As root_type_injector_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function classes() As class_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function template() As template_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function functions() As function_t
            assert(False)
            Return Nothing
        End Function

        Public Overridable Function params() As params_t
            assert(False)
            Return Nothing
        End Function
    End Class
End Class
