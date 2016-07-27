
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class expression_test
        Inherits [case]

        Private Shared Function location_case() As Boolean
            Dim d As domain = Nothing
            d = New domain(New variables())
            Dim v As variable = Nothing
            v = rnd_variable()
            d.define(v)
            Dim e As expression = Nothing
            e = New expression(New location(0, 0))
            Dim v2 As variable = Nothing
            v2 = e.execute(d)
            assert_reference_equal(v, v2)
            assert_equal(v, v2)
            Return True
        End Function

        Private Shared Function function_case() As Boolean
            Dim d As domain = Nothing
            d = New domain(New variables())
            Dim f As rnd_int_function = Nothing
            f = New rnd_int_function(0)
            Dim e As expression = Nothing
            e = New expression(f, Nothing)
            Dim v As variable = Nothing
            v = e.execute(d)
            assert_not_nothing(v)
            assert_true(v.is_int())
            Dim i As Int32 = 0
            If assert_true(v.int(i)) Then
                assert_more_and_less(i, min_int32, max_int32)
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return location_case() AndAlso
                   function_case()
        End Function
    End Class
End Namespace
