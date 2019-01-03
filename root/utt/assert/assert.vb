
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

Public Module _assert
    Public Function assert_true(ByVal v As Boolean, ByVal ParamArray msg() As Object) As Boolean
        Return assertion.is_true(v, msg)
    End Function

    Public Function assert_false(ByVal v As Boolean, ByVal ParamArray msg() As Object) As Boolean
        Return assertion.is_false(v, msg)
    End Function

    Public Function assert_nothing(Of T)(ByVal i As T, ByVal ParamArray msg() As Object) As Boolean
        Return assertion.is_null(i, msg)
    End Function

    Public Function assert_not_nothing(Of T)(ByVal i As T,
                                             ByVal ParamArray msg() As Object) As Boolean
        Return assertion.is_not_null(i, msg)
    End Function

    Public Function assert_reference_equal(Of T As Class)(ByVal i As T,
                                                          ByVal j As T,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return assertion.reference_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_reference_equal_to(Of T As Class)(ByVal i As T,
                                                                           ByVal j As T,
                                                                           ByVal ParamArray msg() As Object) As Boolean
        Return assert_reference_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_not_reference_equal(Of T As Class)(ByVal i As T,
                                                              ByVal j As T,
                                                              ByVal ParamArray msg() As Object) As Boolean
        Return assertion.not_reference_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_not_reference_equal_to(Of T As Class) _
                                                               (ByVal i As T,
                                                                ByVal j As T,
                                                                ByVal ParamArray msg() As Object) As Boolean
        Return assert_not_reference_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_equal(Of T)(ByVal i As T,
                                       ByVal j As T,
                                       ByVal ParamArray msg() As Object) As Boolean
        Return assertion.equal(i, j, msg)
    End Function

    Public Function assert_not_equal(Of T)(ByVal i As T,
                                           ByVal j As T,
                                           ByVal ParamArray msg() As Object) As Boolean
        Return assertion.not_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_not_equal_to(Of T)(ByVal i As T,
                                                            ByVal j As T,
                                                            ByVal ParamArray msg() As Object) As Boolean
        Return assert_not_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_array_equal(Of T)(ByVal i() As T,
                                             ByVal j() As T,
                                             ByVal ParamArray msg() As Object) As Boolean
        Return assertion.array_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_array_equal_to(Of T)(ByVal i() As T,
                                                              ByVal j() As T,
                                                              ByVal ParamArray msg() As Object) As Boolean
        Return assert_array_equal(Of T)(i, j, msg)
    End Function

    Public Function assert_array_not_equal(Of T)(ByVal i() As T,
                                                 ByVal j() As T,
                                                 ByVal ParamArray msg() As Object) As Boolean
        Return assertion.array_not_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_array_not_equal_to(Of T)(ByVal i() As T,
                                                                  ByVal j() As T,
                                                                  ByVal ParamArray msg() As Object) As Boolean
        Return assert_array_equal(i, j, msg)
    End Function

    Public Function assert_vector_equal(Of T)(ByVal i As vector(Of T),
                                              ByVal j As vector(Of T),
                                              ByVal ParamArray msg() As Object) As Boolean
        Return assertion.vector_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_vector_equal_to(Of T)(ByVal i As vector(Of T),
                                                               ByVal j As vector(Of T),
                                                               ByVal ParamArray msg() As Object) As Boolean
        Return assert_vector_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_equal_to(Of T)(ByVal i As T,
                                                        ByVal j As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return assert_equal(i, j, msg)
    End Function

    Public Function assert_less(Of T)(ByVal i As T,
                                      ByVal j As T,
                                      ByVal ParamArray msg() As Object) As Boolean
        Return assertion.less(i, j, msg)
    End Function

    <Extension()> Public Function assert_less_than(Of T)(ByVal i As T,
                                                         ByVal j As T,
                                                         ByVal ParamArray msg() As Object) As Boolean
        Return assert_less(i, j, msg)
    End Function

    Public Function assert_less_or_equal(Of T)(ByVal i As T,
                                               ByVal j As T,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assertion.less_or_equal(i, j, msg)
    End Function

    <Extension()> Public Function assert_less_than_or_equal_to(Of T)(ByVal i As T,
                                                                     ByVal j As T,
                                                                     ByVal ParamArray msg() As Object) As Boolean
        Return assert_less_or_equal(i, j, msg)
    End Function

    Public Function assert_more(Of T)(ByVal i As T,
                                      ByVal j As T,
                                      ByVal ParamArray msg() As Object) As Boolean
        Return assertion.more(i, j, msg)
    End Function

    <Extension()> Public Function assert_more_than(Of T)(ByVal i As T,
                                                         ByVal j As T,
                                                         ByVal ParamArray msg() As Object) As Boolean
        Return assert_more(i, j, msg)
    End Function

    Public Function assert_more_or_equal(Of T)(ByVal i As T,
                                               ByVal j As T,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assertion.more_or_equal(i, j, msg)
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
        Return assertion.more_and_less(i, min, max, msg)
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
        Return assert_more_and_less(i, min, max, msg)
    End Function

    Public Function assert_more_or_equal_and_less(Of T)(ByVal i As T,
                                                        ByVal min As T,
                                                        ByVal max As T,
                                                        ByVal ParamArray msg() As Object) As Boolean
        Return assertion.more_or_equal_and_less(i, min, max, msg)
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
        Return assert_more_and_less_or_equal(i, min, max, msg)
    End Function

    Public Function assert_more_or_equal_and_less_or_equal(Of T)(ByVal i As T,
                                                                 ByVal min As T,
                                                                 ByVal max As T,
                                                                 ByVal ParamArray msg() As Object) As Boolean
        Return assertion.more_or_equal_and_less_or_equal(i, min, max, msg)
    End Function

    <Extension()> Public Function assert_more_than_or_equal_to_and_less_than_or_equal_to _
                          (Of T) _
                          (ByVal i As T,
                           ByVal min As T,
                           ByVal max As T,
                           ByVal ParamArray msg() As Object) As Boolean
        Return assert_more_or_equal_and_less_or_equal(i, min, max, msg)
    End Function

    <Extension()> Public Function assert_int(ByVal i As Double, ByVal ParamArray msg() As Object) As Boolean
        Return assertion.is_int(i, msg)
    End Function

    <Extension()> Public Function assert_not_int(ByVal i As Double, ByVal ParamArray msg() As Object) As Boolean
        Return assertion.is_not_int(i, msg)
    End Function

    Public Function assert_compare(Of T)(ByVal l As T,
                                         ByVal r As T,
                                         ByVal exp As Int32,
                                         ByVal ParamArray msg() As Object) As Boolean
        Return assertion.equal_to_compare(l, r, exp, msg)
    End Function

    <Extension()> Public Function assert_compare_to(Of T)(ByVal l As T,
                                                          ByVal r As T,
                                                          ByVal exp As Int32,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return assert_compare(l, r, exp, msg)
    End Function

    Public Function assert_compare(Of T)(ByVal l As T, ByVal r As T) As Boolean
        Return assertion.consistently_compare(l, r)
    End Function

    Public Function assert_not_reach(ByVal ParamArray msg() As Object) As Boolean
        Return assertion.not_reach(msg)
    End Function

    <Extension()> Public Function assert_compare_to(Of T)(ByVal l As T, ByVal r As T) As Boolean
        Return assert_compare(l, r)
    End Function

    <Extension()> Public Function assert_happening_in(ByVal f As Func(Of Boolean),
                                                      ByVal ms As Int64,
                                                      ByVal ParamArray msg() As Object) As Boolean
        Return assertion.happening_in(f, ms, msg)
    End Function

    <Extension()> Public Function assert_happening(ByVal f As Func(Of Boolean),
                                                   ByVal ParamArray msg() As Object) As Boolean
        Return assertion.happening(f, msg)
    End Function

    <Extension()> Public Function assert_not_happening_in(ByVal f As Func(Of Boolean),
                                                          ByVal ms As Int64,
                                                          ByVal ParamArray msg() As Object) As Boolean
        Return assertion.not_happening_in(f, ms, msg)
    End Function

    <Extension()> Public Function assert_not_happening(ByVal f As Func(Of Boolean),
                                                       ByVal ParamArray msg() As Object) As Boolean
        Return assertion.not_happening(f, msg)
    End Function

    <Extension()> Public Function assert_throw(Of EXCEPTION_TYPE)(ByVal d As Action,
                                                                  ByVal ParamArray msg() As Object) As Boolean
        Return assertion.thrown(Of EXCEPTION_TYPE)(d, msg)
    End Function

    <Extension()> Public Function assert_throw(ByVal d As Action,
                                               ByVal ParamArray msg() As Object) As Boolean
        Return assertion.thrown(d, msg)
    End Function

    Public Function failure_count() As Int64
        Return assertion.failure_count()
    End Function

    Public Sub clear_failure()
        assertion.clear_failure()
    End Sub
End Module
