
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public Class generic_object_perf(Of T)
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 100000000)
    End Function

    Public Sub New()
        MyBase.New(R(New generic_case()), R(New object_case()))
    End Sub

    Private Class generic_case
        Inherits [case]

        Private Function is_null(ByVal i As T) As Boolean
            Return i Is Nothing
        End Function

        Public Overrides Function run() As Boolean
            Dim x As T = Nothing
            Dim y As Boolean = False
            y = is_null(x)
            Return True
        End Function
    End Class

    Private Class object_case
        Inherits [case]

        Private Function is_null(ByVal i As Object) As Boolean
            Return i Is Nothing
        End Function

        Public Overrides Function run() As Boolean
            Dim x As T = Nothing
            Dim y As Boolean = False
            y = is_null(x)
            Return True
        End Function
    End Class
End Class

Public Class generic_object_valuetype_perf
    Inherits generic_object_perf(Of Int32)
End Class

Public Class generic_object_string_perf
    Inherits generic_object_perf(Of String)
End Class
