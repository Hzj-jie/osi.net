
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public Function includes() As includes_t
        Return from_root(Function(ByVal i As T) As includes_t
                             assert(Not i Is Nothing)
                             Return i.myself().includes()
                         End Function)
    End Function

    Public Function defines() As define_t
        Return from_root(Function(ByVal i As T) As define_t
                             assert(Not i Is Nothing)
                             Return i.myself().defines()
                         End Function)
    End Function

    Public Function current_function() As current_function_proxy
        Return New current_function_proxy()
    End Function

    Public Function delegates() As delegate_proxy
        Return New delegate_proxy()
    End Function

    Public Function type_alias() As type_alias_proxy
        Return New type_alias_proxy()
    End Function

    Public Function structs() As struct_proxy
        Return New struct_proxy()
    End Function

    Public Function variables() As variable_proxy
        Return New variable_proxy()
    End Function

    Public Function temp_logic_name() As temp_logic_name_t
        Return from_root(Function(ByVal i As T) As temp_logic_name_t
                             assert(Not i Is Nothing)
                             Return i.myself().temp_logic_name()
                         End Function)
    End Function

    Public Function value_target() As value_target_t
        Return scope(Of T).current().myself().value_target()
    End Function

    Protected Function call_hierarchy(Of CT As call_hierarchy_t)() As CT
        Return from_root(Function(ByVal i As T) As CT
                             assert(Not i Is Nothing)
                             Return direct_cast(Of CT)(i.myself().call_hierarchy())
                         End Function)
    End Function

    Public Function call_hierarchy() As call_hierarchy_t
        Return call_hierarchy(Of call_hierarchy_t)()
    End Function

    Public Function current_namespace() As current_namespace_t
        Return from_root(Function(ByVal i As T) As current_namespace_t
                             assert(Not i Is Nothing)
                             Return i.myself().current_namespace()
                         End Function)
    End Function

    Public Function root_type_injector() As root_type_injector_t
        Return from_root(Function(ByVal i As T) As root_type_injector_t
                             assert(Not i Is Nothing)
                             Return i.myself().root_type_injector()
                         End Function)
    End Function

    Public Function classes() As class_proxy
        Return New class_proxy()
    End Function

    Public Function template() As template_proxy
        Return New template_proxy()
    End Function

    Public Function functions() As function_t
        Return from_root(Function(ByVal i As T) As function_t
                             assert(Not i Is Nothing)
                             Return i.myself().functions()
                         End Function)
    End Function

    Public Function params() As params_t
        Return scope(Of T).current().myself().params()
    End Function
End Class
