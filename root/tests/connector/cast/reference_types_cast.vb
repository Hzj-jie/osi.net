
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Module _reference_types_cast
    Private Function cast_case_verify(Of T As {return_s, Class},
                                         T2 As {return_s, Class})(ByVal i As T, ByVal j As T2) As Boolean
        If assertion.is_not_null(j) Then
            assertion.equal(i.s, j.s)
        End If
        Return True
    End Function

    Private Function cast_case_T_T2(Of T As {return_s, Class},
                                       T2 As {return_s, Class})(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        assertion.is_true(cast(i, j))
        Return cast_case_verify(i, j)
    End Function

    Private Function cast_as_T(Of T As {return_s, Class},
                                  T2 As {return_s, Class})(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        assertion.is_true(i.cast_to(j))
        Return cast_case_verify(i, j)
    End Function

    Private Function cast_case_T_object(Of T As {return_s, Class}, T2 As {return_s, Class})(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        Using scoped.atomic_bool(suppress.on("cast(Of T)(Object v)"))
            assertion.is_true(cast(Of T2)(i, j))
        End Using
        Return cast_case_verify(i, j)
    End Function

    Private Function cast_as_object(Of T As {return_s, Class}, T2 As {return_s, Class})(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        assertion.is_true(i.cast_to(Of T2)(j))
        Return cast_case_verify(i, j)
    End Function

    Private Function cast_case(Of T As {return_s, Class}, T2 As {return_s, Class})(ByVal i As T) As Boolean
        Return cast_case_T_T2(Of T, T2)(i) AndAlso
               cast_as_T(Of T, T2)(i) AndAlso
               cast_case_T_object(Of T, T2)(i) AndAlso
               cast_as_object(Of T, T2)(i)
    End Function

    Private Function base_inherit() As Boolean
        Return cast_test.failed_case(Of base_class, inherit_class)(New base_class("abc"))
    End Function

    Private Function inherit_base() As Boolean
        Return cast_case(Of inherit_class, base_class)(New inherit_class("abc"))
    End Function

    Private Function inherit_other() As Boolean
        Return cast_test.failed_case(Of inherit_class, other_class)(New inherit_class("abc"))
    End Function

    Private Function ctyped_inherit() As Boolean
        'compile time casting
        Return cast_case(Of inherit_class, inherit_class)(CType(New ctyped_class("abc"), inherit_class)) AndAlso
               cast_case(Of ctyped_class, inherit_class)(New ctyped_class("abc")) 'runtime casting
    End Function

    Private Function ctyped_base() As Boolean
        'compile time casting
        Return cast_case(Of inherit_class, base_class)(CType(New ctyped_class("abc"), inherit_class)) AndAlso
               cast_case(Of ctyped_class, base_class)(New ctyped_class("abc")) 'runtime casting + inherit
    End Function

    Private Function ctyped_return_s() As Boolean
        'compile time casting
        Return cast_case(Of return_s, return_s)(CType(New ctyped_class("abc"), return_s)) AndAlso
               cast_case(Of ctyped_class, return_s)(New ctyped_class("abc")) 'runtime casting + implement
    End Function

    Private Function ctyped2_inherit() As Boolean
        'compile time casting
        Return cast_case(Of inherit_class, inherit_class)(CType(New ctyped2_class("abc"), inherit_class)) AndAlso
               cast_case(Of ctyped2_class, inherit_class)(New ctyped2_class("abc")) 'runtime casting
    End Function

    Private Function ctyped2_base() As Boolean
        'compile time casting
        Return cast_case(Of inherit_class, base_class)(CType(New ctyped2_class("abc"), inherit_class)) AndAlso
               cast_case(Of ctyped2_class, base_class)(New ctyped2_class("abc")) 'runtime casting + inherit
    End Function

    Private Function ctyped2_return_s() As Boolean
        'compile time casting
        Return cast_case(Of return_s, return_s)(CType(New ctyped2_class("abc"), return_s)) AndAlso
               cast_case(Of ctyped2_class, return_s)(New ctyped2_class("abc")) 'runtime casting + implement
    End Function

    Private Function inherit_ctyped() As Boolean
        Return cast_test.failed_case(Of inherit_class, ctyped_class)(New inherit_class("abc"))
    End Function

    Public Function reference_types() As Boolean
        Return base_inherit() AndAlso
               inherit_base() AndAlso
               inherit_other() AndAlso
               ctyped_inherit() AndAlso
               ctyped_base() AndAlso
               ctyped_return_s() AndAlso
               inherit_ctyped() AndAlso
               ctyped2_inherit() AndAlso
               ctyped2_base() AndAlso
               ctyped2_return_s()
    End Function
End Module
