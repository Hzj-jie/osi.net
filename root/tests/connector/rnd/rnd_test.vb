
Imports osi.root.connector
Imports osi.root.utt

Public Class rnd_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New rnd_test_case(), If(isreleasebuild(), 2000000, 100000))
    End Sub

    Private Class rnd_test_case
        Inherits [case]

        Private Shared Function rnd_case() As Boolean
            Const i As Double = 10
            Const j As Double = 1000
            assert_int(rnd(i, j, True))
            'there is a chance to return an integer, 
            'but seems totally impossible to return two integers
            assert_not_int(rnd(i, j, False) + rnd(-j, -i, False))
            assert_less(rnd(i, j, True), j)
            assert_more_or_equal(rnd(i, j, True), i)
            assert_less(rnd(i, j, False), j)
            assert_more_or_equal(rnd(i, j, False), i)
            Return True
        End Function

        Private Shared Function rnd_int_case() As Boolean
            Const i As Int32 = 10
            Const j As Int32 = 1000
            assert_less(rnd_int(i, j), j)
            assert_more_or_equal(rnd_int(i, j), i)
            Return True
        End Function

        Private Shared Function rnd_uint_case() As Boolean
            Const i As UInt32 = 10
            Const j As UInt32 = 1000
            assert_less(rnd_uint(i, j), j)
            assert_more_or_equal(rnd_uint(i, j), i)
            Return True
        End Function

        Private Shared Function rnd_double_case() As Boolean
            Const i As Double = 10
            Const j As Double = 1000
            assert_less(rnd_double(i, j), j)
            assert_more_or_equal(rnd_double(i, j), i)
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return rnd_case() AndAlso
                   rnd_int_case() AndAlso
                   rnd_uint_case() AndAlso
                   rnd_double_case()
        End Function
    End Class
End Class
