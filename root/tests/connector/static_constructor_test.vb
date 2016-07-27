
Imports osi.root.connector
Imports osi.root.utt

Public Class static_constructor_test
    Inherits [case]

    Private Shared ReadOnly def As Int32
    Private Shared ReadOnly exp As Int32
    Private Shared v As Int32

    Shared Sub New()
        def = rnd_int()
        While exp = def
            exp = rnd_int()
        End While
        v = def
    End Sub

    Private Class C
        Shared Sub New()
            If v = def Then
                v = exp
            Else
                v = def
            End If
        End Sub
    End Class

    Private Class D
    End Class

    Public Overrides Function run() As Boolean
        Dim c As C = Nothing
        assert_equal(v, def)
        static_constructor(Of C).execute()
        assert_equal(v, exp)
        c = New C()
        assert_equal(v, exp)
        assert_not_nothing(static_constructor(Of C).retrieve())
        static_constructor(Of C).as_action()()
        assert_equal(v, def)

        assert_nothing(static_constructor(Of D).retrieve())
        assert_nothing(static_constructor(Of D).as_action())
        static_constructor(Of D).execute()
        Return True
    End Function
End Class
