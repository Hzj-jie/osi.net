
Imports osi.root.utt

Public Class type_name_perf
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(r(New get_type_case(Of String)()), r(New cache_case(Of String)()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 6},
                {0.4, 0}}
    End Function

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 200000000)
    End Function

    Private Class get_type_case(Of T)
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim s As String = Nothing
            s = GetType(T).FullName()
            Return True
        End Function
    End Class

    Private Class cache_case(Of T)
        Inherits [case]

        ' Simulate type_info(Of T).fullname
        Private Class cache
            Public Shared ReadOnly fullname As String

            Shared Sub New()
                fullname = GetType(T).FullName()
            End Sub
        End Class

        Public Overrides Function run() As Boolean
            Dim s As String = Nothing
            s = cache.fullname
            Return True
        End Function
    End Class
End Class
