
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.envs

Public Class constructor_detect_test
    Inherits [case]

    Private Class test_class_1
        Public Sub New(ByVal s As String)
        End Sub
    End Class

    Private Class test_class_2
        Public Sub New()
        End Sub
    End Class

    Private Class test_class_3
        Private Sub New()
        End Sub
    End Class

    Private Class test_class_4
        Protected Sub New()
        End Sub
    End Class

    Private Class test_class_5
        Inherits test_class_4
        Protected Sub New()
            MyBase.New()
        End Sub
    End Class

    Private Class test_class_6
        Inherits test_class_4
        Private Sub New()
            MyBase.New()
        End Sub
    End Class

    Private Class test_class_7
        Inherits test_class_2
        Private Sub New()
            MyBase.New()
        End Sub
    End Class

    Private Class test_class_8
        Inherits test_class_2
        Public Sub New()
            MyBase.New()
        End Sub
    End Class

    Private Class test_class_9
        Inherits test_class_1
        Public Sub New()
            MyBase.New("ABC")
        End Sub
    End Class

    Private Class test_class_10
        Inherits test_class_4
        Public Sub New()
            MyBase.New()
        End Sub
    End Class

    Private Shared Function run_case(Of T)(ByVal has_pl_c As Boolean, ByVal has_pl_p_c As Boolean)
        assert_equal(GetType(T).has_parameterless_constructor(), has_pl_c)
        assert_equal(GetType(T).has_parameterless_public_constructor(), has_pl_p_c)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(Of test_class_1)(False, False) AndAlso
               run_case(Of test_class_2)(True, True) AndAlso
               run_case(Of test_class_3)(True, False) AndAlso
               run_case(Of test_class_4)(True, False) AndAlso
               run_case(Of test_class_5)(True, False) AndAlso
               run_case(Of test_class_6)(True, False) AndAlso
               run_case(Of test_class_7)(True, False) AndAlso
               run_case(Of test_class_8)(True, True) AndAlso
               run_case(Of test_class_9)(True, True) AndAlso
               run_case(Of test_class_10)(True, True)
    End Function
End Class
