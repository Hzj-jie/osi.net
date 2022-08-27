
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Partial Public Class scope(Of T As scope(Of T))
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

    Private Function call_hierarchy() As call_hierarchy_t
        Return call_hierarchy(Of call_hierarchy_t)()
    End Function

    Public Function current_namespace() As current_namespace_t
        Return from_root(Function(ByVal i As T) As current_namespace_t
                             assert(Not i Is Nothing)
                             Return i.myself().current_namespace()
                         End Function)
    End Function

    Protected Function root_type_injector(Of WRITER As {lazy_list_writer, New})() As root_type_injector_t(Of WRITER)
        Return from_root(Function(ByVal i As T) As root_type_injector_t(Of WRITER)
                             assert(Not i Is Nothing)
                             Return i.myself().root_type_injector(Of WRITER)()
                         End Function)
    End Function

    Public Function classes() As class_proxy
        Return New class_proxy()
    End Function

    Protected Function template(Of WRITER As {lazy_list_writer, New},
                                   BUILDER As func_t(Of String, WRITER, Boolean),
                                   CODE_GENS As func_t(Of code_gens(Of WRITER)))() _
                           As template_proxy(Of WRITER, BUILDER, CODE_GENS)
        Return New template_proxy(Of WRITER, BUILDER, CODE_GENS)()
    End Function
End Class
