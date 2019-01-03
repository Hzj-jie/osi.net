
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template

Public Class check(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Private Const default_assert_async_wait_time_ms As Int64 = 10 * second_milli
    Private Shared ReadOnly _is_true As __void(Of Boolean, Object())

    Shared Sub New()
        _is_true = alloc(Of IS_TRUE_FUNC)()
        assert(Not _is_true Is Nothing)
    End Sub

    Private Shared Function left_right_msg(Of T)(ByVal cmp As String,
                                                 ByVal i As T,
                                                 ByVal j As T,
                                                 ByVal msg() As Object) As Object()
        Return New Object() {"left ", i, ", right ", j, ", expected left is ", cmp, " right, ", msg}
    End Function

    Public Shared Function is_true(ByVal v As Boolean, ByVal ParamArray msg() As Object) As Boolean
        _is_true.invoke(v, msg)
        Return v
    End Function

    Public Shared Function is_false(ByVal v As Boolean, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(Not v, msg)
    End Function

    Public Shared Function is_null(Of T)(ByVal i As T, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(i.is_null(), msg)
    End Function

    Public Shared Function is_not_null(Of T)(ByVal i As T, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(Not i.is_null(), msg)
    End Function

    Public Shared Function reference_equal(Of T As Class)(ByVal i As T,
                                                          ByVal j As T,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return is_true(object_compare(i, j) = 0, msg)
    End Function

    Public Shared Function not_reference_equal(Of T As Class)(ByVal i As T,
                                                              ByVal j As T,
                                                              ByVal ParamArray msg() As Object) As Boolean
        Return is_true(object_compare(i, j) <> 0, msg)
    End Function

    Public Shared Function equal(Of T)(ByVal i As T,
                                       ByVal j As T,
                                       ByVal ParamArray msg() As Object) As Boolean
        Return is_true(compare(i, j) = 0, left_right_msg("equal to", i, j, msg))
    End Function

    Public Shared Function not_equal(Of T)(ByVal i As T,
                                           ByVal j As T,
                                           ByVal ParamArray msg() As Object) As Boolean
        Return is_true(compare(i, j) <> 0, left_right_msg("not equal to", i, j, msg))
    End Function

    Public Shared Function array_equal(Of T)(ByVal i() As T,
                                             ByVal j() As T,
                                             ByVal ParamArray msg() As Object) As Boolean
        If Not equal(array_size(i), array_size(j), msg) Then
            Return False
        End If
        For k As Int32 = 0 To array_size_i(i) - 1
            If Not equal(i(k), j(k), msg) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function array_not_equal(Of T)(ByVal i() As T,
                                                 ByVal j() As T,
                                                 ByVal ParamArray msg() As Object) As Boolean
        If array_size(i) <> array_size(j) Then
            Return True
        End If
        For k As Int32 = 0 To array_size_i(i) - 1
            If compare(i(k), j(k)) <> 0 Then
                Return True
            End If
        Next
        Return is_true(False, msg)
    End Function

    Public Shared Function vector_equal(Of T)(ByVal i As vector(Of T),
                                              ByVal j As vector(Of T),
                                              ByVal ParamArray msg() As Object) As Boolean
        Return array_equal(+i, +j, msg)
    End Function

    Public Shared Function vector_not_equal(Of T)(ByVal i As vector(Of T),
                                                  ByVal j As vector(Of T),
                                                  ByVal ParamArray msg() As Object) As Boolean
        Return array_not_equal(+i, +j, msg)
    End Function

    Public Shared Function less(Of T)(ByVal i As T, ByVal j As T, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(compare(i, j) < 0, left_right_msg("less than", i, j, msg))
    End Function

    Public Shared Function less_or_equal(Of T)(ByVal i As T, ByVal j As T, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(compare(i, j) <= 0, left_right_msg("less than or equal to", i, j, msg))
    End Function

    Public Shared Function more(Of T)(ByVal i As T, ByVal j As T, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(compare(i, j) > 0, left_right_msg("more than", i, j, msg))
    End Function

    Public Shared Function more_or_equal(Of T)(ByVal i As T, ByVal j As T, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(compare(i, j) >= 0, left_right_msg("more than or equal to", i, j, msg))
    End Function

    Public Shared Function more_and_less(Of T)(ByVal i As T,
                                               ByVal min As T,
                                               ByVal max As T,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return more(i, min, msg) AndAlso
               less(i, max, msg)
    End Function

    Public Shared Function more_or_equal_and_less(Of T)(ByVal i As T,
                                                        ByVal min As T,
                                                        ByVal max As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return more_or_equal(i, min, msg) AndAlso
               less(i, max, msg)
    End Function

    Public Shared Function more_and_less_or_equal(Of T)(ByVal i As T,
                                                        ByVal min As T,
                                                        ByVal max As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return more(i, min, msg) AndAlso
               less_or_equal(i, max, msg)
    End Function

    Public Shared Function more_or_equal_and_less_or_equal(Of T)(ByVal i As T,
                                                                 ByVal min As T,
                                                                 ByVal max As T,
                                                                 ByVal ParamArray msg() As Object) As Boolean
        Return more_or_equal(i, min, msg) AndAlso
               less_or_equal(i, max, msg)
    End Function

    Public Shared Function is_int(ByVal i As Double, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(i.is_int(), msg)
    End Function

    Public Shared Function is_not_int(ByVal i As Double, ByVal ParamArray msg() As Object) As Boolean
        Return is_true(i.is_not_int(), msg)
    End Function

    Public Shared Function equal_to_compare(Of T)(ByVal l As T,
                                                  ByVal r As T,
                                                  ByVal exp As Int32,
                                                  ByVal ParamArray msg() As Object) As Boolean
        If isemptyarray(msg) Then
            msg = {"left ", l, ", right ", r, ", exp ", exp}
        End If
        Return equal(compare(l, r) < 0, exp < 0, msg) And
               equal(compare(l, r) > 0, exp > 0, msg) And
               equal(compare(l, r) = 0, exp = 0, msg) And
               equal(compare(r, l) < 0, exp > 0, msg) And
               equal(compare(r, l) > 0, exp < 0, msg) And
               equal(compare(r, l) = 0, exp = 0, msg)
    End Function

    Public Shared Function consistently_compare(Of T)(ByVal l As T, ByVal r As T) As Boolean
        Return equal_to_compare(l, r, compare(l, r))
    End Function

    Public Shared Function not_reach(ByVal ParamArray msg() As Object) As Boolean
        is_true(False, msg)
        Return False
    End Function

    Public Shared Function happening_in(ByVal f As Func(Of Boolean),
                                        ByVal ms As Int64,
                                        ByVal ParamArray msg() As Object) As Boolean
        Return is_true(lazy_sleep_wait_until(f, ms), msg)
    End Function

    Public Shared Function happening(ByVal f As Func(Of Boolean),
                                     ByVal ParamArray msg() As Object) As Boolean
        Return happening_in(f, default_assert_async_wait_time_ms, msg)
    End Function

    Public Shared Function not_happening_in(ByVal f As Func(Of Boolean),
                                            ByVal ms As Int64,
                                            ByVal ParamArray msg() As Object) As Boolean
        Return is_false(lazy_sleep_wait_until(f, ms), msg)
    End Function

    Public Shared Function not_happening(ByVal f As Func(Of Boolean),
                                         ByVal ParamArray msg() As Object) As Boolean
        Return not_happening_in(f, default_assert_async_wait_time_ms, msg)
    End Function

    Public Shared Function thrown(Of EXCEPTION_TYPE)(ByVal d As Action,
                                                     ByVal ParamArray msg() As Object) As Boolean
        If Not is_not_null(d) Then
            Return False
        End If
        Try
            d()
            Return not_reach(msg)
        Catch ex As Exception
            Return is_true(ex.GetType().is(GetType(EXCEPTION_TYPE)), msg)
        End Try
    End Function

    Public Shared Function thrown(ByVal d As Action, ByVal ParamArray msg() As Object) As Boolean
        Return thrown(Of Exception)(d, msg)
    End Function

    Protected Sub New()
    End Sub
End Class
