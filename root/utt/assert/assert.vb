
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

Public Module _assert
    Private Const default_assert_async_wait_time_ms As Int64 = 10 * second_milli
    Private failure As Int64 = 0

    Public Function assert_true(ByVal v As Boolean, ByVal ParamArray msg() As Object) As Boolean
        If Not v Then
            utt_raise_error("assert failed, ", msg, " @ ", callingcode("_assert"), ", stacktrace ", callstack())
            If Not envs.utt_no_assert Then
                Interlocked.Increment(failure)
                assert(atomic.read(failure) < If(envs.mono, 10000, 1000), "too many assert failures")
            End If
        End If
        Return v
    End Function

    Public Function assert_nothing(Of T)(ByVal i As T,
                                         ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(i.is_null(), msg)
    End Function

    Public Function assert_not_nothing(Of T)(ByVal i As T,
                                             ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(Not i.is_null(), msg)
    End Function

    Public Function assert_false(ByVal v As Boolean, ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(Not v, msg)
    End Function

    Public Function assert_reference_equal(Of T As Class)(ByVal i As T,
                                                          ByVal j As T,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(object_compare(i, j) = 0, msg)
    End Function

    <Extension()> Public Function assert_reference_equal_to(Of T As Class)(ByVal i As T,
                                                                           ByVal j As T,
                                                                           ByVal ParamArray msg() As Object) As Boolean
        Return assert_reference_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_not_reference_equal(Of T As Class)(ByVal i As T,
                                                              ByVal j As T,
                                                              ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(object_compare(i, j) <> 0, msg)
    End Function

    <Extension()> Public Function assert_not_reference_equal_to(Of T As Class) _
                                                               (ByVal i As T,
                                                                ByVal j As T,
                                                                ByVal ParamArray msg() As Object) As Boolean
        Return assert_not_reference_equal(Of T)(i, j, msg)
    End Function

    Private Function assert_left_right_msg(Of T)(ByVal cmp As String,
                                                 ByVal i As T,
                                                 ByVal j As T,
                                                 ByVal msg() As Object) As Object()
        Return New Object() {"left ", i, ", right ", j, ", expected left is ", cmp, " right, ", msg}
    End Function

    Public Function assert_not_equal(Of T)(ByVal i As T,
                                           ByVal j As T,
                                           ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(compare(i, j) <> 0, assert_left_right_msg("not equal to", i, j, msg))
    End Function

    <Extension()> Public Function assert_not_equal_to(Of T)(ByVal i As T,
                                                            ByVal j As T,
                                                            ByVal ParamArray msg() As Object) As Boolean
        Return assert_not_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_array_equal(Of T)(ByVal i() As T,
                                             ByVal j() As T,
                                             ByVal ParamArray msg() As Object) As Boolean
        If assert_equal(array_size(i), array_size(j), msg) Then
            For k As Int32 = 0 To array_size(i) - 1
                If Not assert_equal(i(k), j(k), msg) Then
                    Return False
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function assert_array_equal_to(Of T)(ByVal i() As T,
                                                              ByVal j() As T,
                                                              ByVal ParamArray msg() As Object) As Boolean
        Return assert_array_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_array_not_equal(Of T)(ByVal i() As T,
                                                 ByVal j() As T,
                                                 ByVal ParamArray msg() As Object) As Boolean
        If array_size(i) <> array_size(j) Then
            Return True
        Else
            For k As Int32 = 0 To array_size(i) - 1
                If compare(i(k), j(k)) <> 0 Then
                    Return True
                End If
            Next
            Return assert_true(False, msg)
        End If
    End Function

    <Extension()> Public Function assert_array_not_equal_to(Of T)(ByVal i() As T,
                                                                  ByVal j() As T,
                                                                  ByVal ParamArray msg() As Object) As Boolean
        Return assert_array_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_vector_equal(Of T)(ByVal i As vector(Of T),
                                              ByVal j As vector(Of T),
                                              ByVal ParamArray msg() As Object) As Boolean
        Return assert_array_equal(+i, +j, msg)
    End Function

    <Extension()> Public Function assert_vector_equal_to(Of T)(ByVal i As vector(Of T),
                                                               ByVal j As vector(Of T),
                                                               ByVal ParamArray msg() As Object) As Boolean
        Return assert_vector_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_equal(Of T)(ByVal i As T,
                                       ByVal j As T,
                                       ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(compare(i, j) = 0, assert_left_right_msg("equal to", i, j, msg))
    End Function

    <Extension()> Public Function assert_equal_to(Of T)(ByVal i As T,
                                                        ByVal j As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return assert_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_less(Of T)(ByVal i As T,
                                      ByVal j As T,
                                      ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(compare(i, j) < 0, assert_left_right_msg("less than", i, j, msg))
    End Function

    <Extension()> Public Function assert_less_than(Of T)(ByVal i As T,
                                                         ByVal j As T,
                                                         ByVal ParamArray msg() As Object) As Boolean
        Return assert_less(Of T)(i, j, msg)
    End Function

    Public Function assert_less_or_equal(Of T)(ByVal i As T,
                                               ByVal j As T,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(compare(i, j) <= 0, assert_left_right_msg("less than or equal to", i, j, msg))
    End Function

    <Extension()> Public Function assert_less_than_or_equal_to(Of T)(ByVal i As T,
                                                                     ByVal j As T,
                                                                     ByVal ParamArray msg() As Object) As Boolean
        Return assert_less_or_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_more(Of T)(ByVal i As T,
                                      ByVal j As T,
                                      ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(compare(i, j) > 0, assert_left_right_msg("more than", i, j, msg))
    End Function

    <Extension()> Public Function assert_more_than(Of T)(ByVal i As T,
                                                         ByVal j As T,
                                                         ByVal ParamArray msg() As Object) As Boolean
        Return assert_more(Of T)(i, j, msg)
    End Function

    Public Function assert_more_or_equal(Of T)(ByVal i As T,
                                               ByVal j As T,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(compare(i, j) >= 0, assert_left_right_msg("more than or equal to", i, j, msg))
    End Function

    <Extension()> Public Function assert_more_than_or_equal_to(Of T)(ByVal i As T,
                                                                     ByVal j As T,
                                                                     ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_or_equal(i, j, msg)
    End Function

    Public Function assert_more_and_less(Of T)(ByVal i As T,
                                               ByVal min As T,
                                               ByVal max As T,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assert_more(i, min, msg) AndAlso
               assert_less(i, max, msg)
    End Function

    <Extension()> Public Function assert_more_than_and_less_than(Of T)(ByVal i As T,
                                                                       ByVal min As T,
                                                                       ByVal max As T,
                                                                       ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_and_less(i, min, max, msg)
    End Function

    <Extension()> Public Function assert_in_range(Of T)(ByVal i As T,
                                                        ByVal min As T,
                                                        ByVal max As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_and_less(Of T)(i, min, max, msg)
    End Function

    Public Function assert_more_or_equal_and_less(Of T)(ByVal i As T,
                                                        ByVal min As T,
                                                        ByVal max As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_or_equal(i, min, msg) AndAlso
               assert_less(i, max, msg)
    End Function

    <Extension()> Public Function assert_more_than_or_equal_to_and_less_than(Of T) _
                                                                            (ByVal i As T,
                                                                             ByVal min As T,
                                                                             ByVal max As T,
                                                                             ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_or_equal_and_less(Of T)(i, min, max, msg)
    End Function

    Public Function assert_more_and_less_or_equal(Of T)(ByVal i As T,
                                                        ByVal min As T,
                                                        ByVal max As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return assert_more(i, min, msg) AndAlso
               assert_less_or_equal(i, max, msg)
    End Function

    <Extension()> Public Function assert_more_than_and_less_than_or_equal_to(Of T) _
                                                                            (ByVal i As T,
                                                                             ByVal min As T,
                                                                             ByVal max As T,
                                                                             ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_and_less_or_equal(Of T)(i, min, max, msg)
    End Function

    Public Function assert_more_or_equal_and_less_or_equal(Of T)(ByVal i As T,
                                                                 ByVal min As T,
                                                                 ByVal max As T,
                                                                 ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_or_equal(i, min, msg) AndAlso
               assert_less_or_equal(i, max, msg)
    End Function

    <Extension()> Public Function assert_more_than_or_equal_to_and_less_than_or_equal_to _
                          (Of T) _
                          (ByVal i As T,
                           ByVal min As T,
                           ByVal max As T,
                           ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_or_equal(i, min, msg) AndAlso
               assert_less_or_equal(i, max, msg)
    End Function

    <Extension()> Public Function assert_int(ByVal i As Double, ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(i.is_int(), msg)
    End Function

    <Extension()> Public Function assert_not_int(ByVal i As Double, ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(i.is_not_int(), msg)
    End Function

    Public Function assert_compare(Of T)(ByVal l As T,
                                         ByVal r As T,
                                         ByVal exp As Int32,
                                         ByVal ParamArray msg() As Object) As Boolean
        If isemptyarray(msg) Then
            msg = {"left ", l, ", right ", r, ", exp ", exp}
        End If
        Return assert_equal(compare(l, r) < 0, exp < 0, msg) And
               assert_equal(compare(l, r) > 0, exp > 0, msg) And
               assert_equal(compare(l, r) = 0, exp = 0, msg) And
               assert_equal(compare(r, l) < 0, exp > 0, msg) And
               assert_equal(compare(r, l) > 0, exp < 0, msg) And
               assert_equal(compare(r, l) = 0, exp = 0, msg)
    End Function

    <Extension()> Public Function assert_compare_to(Of T)(ByVal l As T,
                                                          ByVal r As T,
                                                          ByVal exp As Int32,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return assert_compare(Of T)(l, r, exp, msg)
    End Function

    Public Function assert_compare(Of T)(ByVal l As T, ByVal r As T) As Boolean
        Return assert_compare(l, r, compare(l, r))
    End Function

    Public Function assert_not_reach(ByVal ParamArray msg() As Object) As Boolean
        assert_true(False, msg)
        Return False
    End Function

    <Extension()> Public Function assert_compare_to(Of T)(ByVal l As T, ByVal r As T) As Boolean
        Return assert_compare(Of T)(l, r)
    End Function

    <Extension()> Public Function assert_happening_in(ByVal f As Func(Of Boolean),
                                                      ByVal ms As Int64,
                                                      ByVal ParamArray msg() As Object) As Boolean
        Return assert_true(lazy_sleep_wait_until(f, ms), msg)
    End Function

    <Extension()> Public Function assert_happening(ByVal f As Func(Of Boolean),
                                                   ByVal ParamArray msg() As Object) As Boolean
        Return assert_happening_in(f, default_assert_async_wait_time_ms, msg)
    End Function

    <Extension()> Public Function assert_not_happening_in(ByVal f As Func(Of Boolean),
                                                          ByVal ms As Int64,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return assert_false(lazy_sleep_wait_until(f, ms), msg)
    End Function

    <Extension()> Public Function assert_not_happening(ByVal f As Func(Of Boolean),
                                                       ByVal ParamArray msg() As Object) As Boolean
        Return assert_not_happening_in(f, default_assert_async_wait_time_ms, msg)
    End Function

    <Extension()> Public Function assert_throw(Of EXCEPTION_TYPE)(ByVal d As Action,
                                                                  ByVal ParamArray msg() As Object) As Boolean
        If Not assert_not_nothing(d) Then
            Return False
        End If
        Try
            d()
            Return assert_not_reach(msg)
        Catch ex As Exception
            Return assert_true(ex.GetType().is(GetType(EXCEPTION_TYPE)), msg)
        End Try
    End Function

    <Extension()> Public Function assert_throw(ByVal d As Action,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assert_throw(Of Exception)(d, msg)
    End Function

    Public Function failure_count() As Int64
        Return atomic.read(failure)
    End Function

    Public Sub clear_failure()
        atomic.eva(failure, 0)
    End Sub
End Module
