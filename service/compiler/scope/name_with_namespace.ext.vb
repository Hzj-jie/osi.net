
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Partial Public Structure name_with_namespace
        Public Shared Function [of](ByVal i As String) As name_with_namespace
            Return New name_with_namespace(scope(Of T).current_namespace_t.of_namespace_and_name(i))
        End Function

        Public Shared Function of_global_namespace(ByVal i As String) As name_with_namespace
            assert(Not i.null_or_whitespace())
            Return New name_with_namespace(tuple.of("", i))
        End Function

        Public Function [namespace]() As String
            Return T._1()
        End Function

        Public Function name() As String
            Return T._2()
        End Function

        Public Function in_global_namespace() As String
            Return scope(Of T).current_namespace_t.with_global_namespace(
                       scope(Of T).current_namespace_t.with_namespace([namespace](), name()))
        End Function
    End Structure
End Class
