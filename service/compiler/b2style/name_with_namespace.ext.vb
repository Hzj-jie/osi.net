
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Partial Public Structure name_with_namespace
        Public Shared Function [of](ByVal i As String) As name_with_namespace
            Return New name_with_namespace(scope.current_namespace_t.of_namespace_and_name(i))
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
            Return _namespace.in_global_namespace(Me)
        End Function

        Public Function bstyle_format() As String
            Return _namespace.bstyle_format.of(Me)
        End Function
    End Structure
End Class
