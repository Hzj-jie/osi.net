
Imports osi.root.utt

Public Class nullable_test
    Inherits [case]

    Private Const value As Int32 = 100

    Private Sub return_null(ByRef i As Int32?)
        i = Nothing
    End Sub

    Private Sub return_value(ByRef i As Int32?)
        i = value
    End Sub

    Public Overrides Function run() As Boolean
        Dim i As Int32? = Nothing
        return_null(i)
        assert_true(i Is Nothing)
        assert_false(i.HasValue())
        Try
            Dim tmp As Int32 = 0
            tmp = i
            assert_true(False)
        Catch
        End Try

        return_value(i)
        assert_true(Not i Is Nothing)
        assert_true(i.HasValue())
        assert_equal(i, value)

        Dim j As Int32 = Nothing
        Try
            return_null(j)
            assert_true(False)
        Catch
        End Try

        return_value(j)
        assert_equal(j, value)

        Return True
    End Function
End Class
