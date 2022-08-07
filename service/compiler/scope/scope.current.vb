
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class scope(Of T As scope(Of T))
    Protected Shared Function current_accessor() As accessor_t
        Return scope(Of T).current().accessor()
    End Function

    Public Function includes() As includes_t
        Return from_root(Function(ByVal i As T) As includes_t
                             assert(Not i Is Nothing)
                             Return i.accessor().includes()
                         End Function)
    End Function

    Public Function defines() As define_t
        Return from_root(Function(ByVal i As T) As define_t
                             assert(Not i Is Nothing)
                             Return i.accessor().defines()
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
End Class
