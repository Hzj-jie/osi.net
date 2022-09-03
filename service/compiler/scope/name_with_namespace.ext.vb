
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
    Partial Public Structure name_with_namespace
        Public Shared Function [of](ByVal i As String) As name_with_namespace
            Return New name_with_namespace(current_namespace_t.of_namespace_and_name(i))
        End Function

        Public Shared Function of_global_namespace(ByVal i As String) As name_with_namespace
            assert(Not i.null_or_whitespace())
            Return New name_with_namespace(tuple.of("", i))
        End Function

        Public Function [namespace]() As String
            Return t._1()
        End Function

        Public Function name() As String
            Return t._2()
        End Function

        Public Function in_global_namespace() As String
            Return current_namespace_t.with_global_namespace(
                       current_namespace_t.with_namespace([namespace](), name()))
        End Function
    End Structure
End Class
