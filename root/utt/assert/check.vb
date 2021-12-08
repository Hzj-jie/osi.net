
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.template

Partial Public Class check(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Private Const default_assert_async_wait_time_ms As UInt32 = 10 * second_milli
    Private Shared ReadOnly _is_true As __void(Of Boolean, Object()) =
        assert_which.of(alloc(Of IS_TRUE_FUNC)()).is_not_null()

    Private Shared Function left_right_msg(Of T)(ByVal cmp As String,
                                                 ByVal i As T,
                                                 ByVal j As T,
                                                 ByVal msg() As Object) As Object()
        Return New Object() {"expected left ", i, " is ", cmp, " right ", j, ", ", msg}
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
        Return is_true(object_compare(i, j) = 0, left_right_msg("refernece equal to ", i, j, msg))
    End Function

    Public Shared Function not_reference_equal(Of T As Class)(ByVal i As T,
                                                              ByVal j As T,
                                                              ByVal ParamArray msg() As Object) As Boolean
        Return is_true(object_compare(i, j) <> 0, left_right_msg("not reference equal to ", i, j, msg))
    End Function

    Public Shared Function equal(Of T)(ByVal i As T,
                                       ByVal j As T,
                                       ByVal ParamArray msg() As Object) As Boolean
        Return is_true(connector.equal(i, j), left_right_msg("equal to", i, j, msg))
    End Function

    Public Shared Function not_equal(Of T)(ByVal i As T,
                                           ByVal j As T,
                                           ByVal ParamArray msg() As Object) As Boolean
        Return is_true(Not connector.equal(i, j), left_right_msg("not equal to", i, j, msg))
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
                                        ByVal ms As UInt32,
                                        ByVal ParamArray msg() As Object) As Boolean
        Return is_true(timeslice_sleep_wait_until(f, ms), msg)
    End Function

    Public Shared Function happening(ByVal f As Func(Of Boolean),
                                     ByVal ParamArray msg() As Object) As Boolean
        Return happening_in(f, default_assert_async_wait_time_ms, msg)
    End Function

    Public Shared Function buzy_happening_in(ByVal f As Func(Of Boolean),
                                             ByVal ms As UInt32,
                                             ByVal ParamArray msg() As Object) As Boolean
        Return is_true(lazy_sleep_wait_until(f, ms), msg)
    End Function

    Public Shared Function buzy_happening(ByVal f As Func(Of Boolean),
                                          ByVal ParamArray msg() As Object) As Boolean
        Return buzy_happening_in(f, default_assert_async_wait_time_ms, msg)
    End Function

    Public Shared Function not_happening_in(ByVal f As Func(Of Boolean),
                                            ByVal ms As UInt32,
                                            ByVal ParamArray msg() As Object) As Boolean
        Return is_false(timeslice_sleep_wait_until(f, ms), msg)
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

    Public Shared Function catch_thrown(Of EXCEPTION_TYPE)(ByVal d As Action,
                                                           ByVal ParamArray msg() As Object) As EXCEPTION_TYPE
        If Not is_not_null(d) Then
            Return Nothing
        End If
        Try
            d()
            not_reach(msg)
            Return Nothing
        Catch ex As Exception
            is_true(ex.GetType().is(GetType(EXCEPTION_TYPE)), msg)
            Return direct_cast(Of EXCEPTION_TYPE)(ex)
        End Try
    End Function

    Public Shared Function thrown(ByVal d As Action, ByVal ParamArray msg() As Object) As Boolean
        Return thrown(Of Exception)(d, msg)
    End Function

    Public Shared Function now_in_time_range(ByVal l As Int64,
                                             ByVal u As Int64,
                                             ByVal ParamArray msg() As Object) As Boolean
        assert(l <= u)
        Dim n As Int64 = 0
        n = DateTime.Now().milliseconds()
        Return more_or_equal_and_less_or_equal(n, l, u, msg)
    End Function

    Public Shared Sub set_time_range(ByRef exp_l As Int64,
                                     ByRef exp_h As Int64,
                                     ByVal low As Int64,
                                     ByVal high As Int64)
        assert(low <= high)
        Dim n As Int64 = 0
        n = DateTime.Now().milliseconds()
        exp_l = n + low
        exp_h = n + high
    End Sub

    Public Shared Function timelimited_operation(ByVal low As Int64,
                                                 ByVal high As Int64,
                                                 ByVal ParamArray msg() As Object) As IDisposable
        Dim exp_l As Int64
        Dim exp_h As Int64
        set_time_range(exp_l, exp_h, low, high)
        Return defer.to(Sub()
                            now_in_time_range(exp_l, exp_h, msg)
                        End Sub)
    End Function

    Public Shared Function equal_after(Of T)(ByVal i As T,
                                             ByVal j As T,
                                             ByVal ParamArray msg() As Object) As IDisposable
        Return defer.to(Sub()
                            equal(i, j, msg)
                        End Sub)
    End Function

    Public Shared Function more_after(Of T)(ByVal i As T,
                                            ByVal j As T,
                                            ByVal ParamArray msg() As Object) As IDisposable
        Return defer.to(Sub()
                            more(i, j, msg)
                        End Sub)
    End Function

    Public Shared Function less_after(Of T)(ByVal i As T,
                                            ByVal j As T,
                                            ByVal ParamArray msg() As Object) As IDisposable
        Return defer.to(Sub()
                            less(i, j, msg)
                        End Sub)
    End Function

    Public Shared Function near_match(ByVal v As Double,
                                      ByVal exp As Double,
                                      ByVal diff As Double,
                                      ByVal ParamArray msg() As Object) As Boolean
        Return less(Math.Abs(v - exp), diff, left_right_msg("near match", v, exp, msg))
    End Function

    Public Shared Function near_match(ByVal v As Double,
                                      ByVal exp As Double,
                                      ByVal ParamArray msg() As Object) As Boolean
        Return near_match(v, exp, 0.001, msg)
    End Function

    Public Shared Function death(ByVal d As Action, ByVal check_exception As Action(Of String)) As Boolean
        assert(Not check_exception Is Nothing)
        Dim r As Boolean = True
        expect_assertion_failure(d,
                                 Sub()
                                     not_reach()
                                     r = False
                                 End Sub,
                                 Sub(ByVal msg As String)
                                     check_exception(msg)
                                     r = False
                                 End Sub)
        Return r
    End Function

    Public Shared Function death(ByVal d As Action) As Boolean
        Return death(d,
                     Sub(ByVal msg As String)
                     End Sub)
    End Function

    Public Shared Function str_contains(ByVal origin As String,
                                        ByVal exp As String,
                                        ByVal ParamArray msg() As Object) As Boolean
        Return is_not_null(origin, "origin", msg) AndAlso
               is_not_null(exp, "exp", msg) AndAlso
               is_true(origin.Contains(exp), left_right_msg("containing", origin, exp, msg))
    End Function

    Public Shared Function str_not_contains(ByVal origin As String,
                                            ByVal not_exp As String,
                                            ByVal ParamArray msg() As Object) As Boolean
        Return is_not_null(origin, "origin", msg) AndAlso
               is_not_null(not_exp, "not_exp", msg) AndAlso
               is_true(Not origin.Contains(not_exp), left_right_msg("not containing", origin, not_exp, msg))
    End Function

    Public Shared Function str_contains(ByVal origin As String,
                                        ByVal exps() As String,
                                        ByVal ParamArray msg() As Object) As Boolean
        Return is_not_null(exps, "exps", msg) AndAlso
               streams.of(exps).
                       map(Function(ByVal exp As String) As Boolean
                               Return str_contains(origin, exp, msg)
                           End Function).
                       aggregate(bool_stream.aggregators.all_true)
    End Function

    Public Shared Function str_not_contains(ByVal origin As String,
                                            ByVal not_exps() As String,
                                            ByVal ParamArray msg() As Object) As Boolean
        Return is_not_null(not_exps, "not_exps", msg) AndAlso
               streams.of(not_exps).
                       map(Function(ByVal not_exp As String) As Boolean
                               Return str_not_contains(origin, not_exp, msg)
                           End Function).
                       aggregate(bool_stream.aggregators.all_true)
    End Function

    Protected Sub New()
    End Sub
End Class
