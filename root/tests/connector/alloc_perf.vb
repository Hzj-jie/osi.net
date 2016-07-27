
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.formation

Public Class alloc_perf
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New alloc_test(), 1024 * 64))
    End Sub

    Public NotOverridable Overrides Function prepare() As Boolean
        suppress.alloc_error.inc()
        Return MyBase.prepare()
    End Function

    Public Overrides Function finish() As Boolean
        suppress.alloc_error.dec()
        Return MyBase.finish()
    End Function
End Class

Public Class alloc_perf2
    Inherits performance_case_wrapper

    Private Class alloc_perf2_case
        Inherits [case]

        Private Shared Function alloc_vector_string() As Boolean
            alloc(Of vector(Of String))()
            Return True
        End Function

        Private Shared Function alloc_int() As Boolean
            alloc(Of Int32)()
            Return True
        End Function

        Private Shared Function alloc_object() As Boolean
            alloc(Of Object)()
            Return True
        End Function

        Private Class test_class
            Public Sub New()
            End Sub
        End Class

        Private Shared Function alloc_test_class() As Boolean
            alloc(Of test_class)()
            Return True
        End Function

        Private Class test_class2
        End Class

        Private Shared Function alloc_test_class2() As Boolean
            alloc(Of test_class2)()
            Return True
        End Function

        Private Class test_class3
            Private Sub New(ByVal a As Int32, ByVal b As Boolean, ByVal c As Object)
            End Sub
        End Class

        Private Shared Function alloc_test_class3() As Boolean
            alloc(Of test_class3)()
            Return True
        End Function

        Private Shared Function alloc_action() As Boolean
            alloc(Of Action)()
            Return True
        End Function

        Private Shared Function alloc_func() As Boolean
            alloc(Of Func(Of Boolean))()
            Return True
        End Function

        Private MustInherit Class test_class4
            Public Sub New(ByVal a As String, ByVal b As Int32)
            End Sub
        End Class

        Private Shared Function alloc_test_class4() As Boolean
            alloc(Of test_class4)()
            Return True
        End Function

        Private Class test_class5
            Private Sub New()
            End Sub
        End Class

        Private Shared Function alloc_test_class5() As Boolean
            alloc(Of test_class5)()
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return alloc_vector_string() AndAlso
                   alloc_int() AndAlso
                   alloc_object() AndAlso
                   alloc_test_class() AndAlso
                   alloc_test_class2() AndAlso
                   alloc_test_class3() AndAlso
                   alloc_action() AndAlso
                   alloc_func() AndAlso
                   alloc_test_class4() AndAlso
                   alloc_test_class5()
        End Function
    End Class

    Public Sub New()
        MyBase.New(repeat(New alloc_perf2_case(), 1024 * 1024))
    End Sub
End Class
