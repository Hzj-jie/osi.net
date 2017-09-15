﻿
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class compare_predefined_test
    Inherits [case]

    Private Shared Function value_type_with_null(Of T, T2)() As Boolean
        Dim x As T = Nothing
        Dim y As T2 = Nothing
        Dim r As Int32 = 0
        assert_true(compare(x, y, r))
        assert_equal(r, 0)
        Return True
    End Function

    Private Shared Function value_type_with_null() As Boolean
        Return value_type_with_null(Of Int32, Int32)() AndAlso
               value_type_with_null(Of Int32, Int16)() AndAlso
               value_type_with_null(Of Int64, Int16)() AndAlso
               value_type_with_null(Of Boolean, Boolean)()
    End Function

    Private Structure s
    End Structure

    Private Shared Function value_type_object_compare() As Boolean
        assert_false(object_compare(1, 1, 0))
        assert_false(object_compare(1.0, 1.0, 0))
        assert_false(object_compare(True, True, 0))
        assert_false(object_compare(New s(), New s(), 0))
        assert_false(object_comparable("", 0))
        assert_false(object_compare("", 0, 0))
        Return True
    End Function

    Private Shared Function value_with_reference_compare() As Boolean
        Dim obj As Object = Nothing
        obj = 1
        assert_false(object_compare(1, obj, 0))
        Return True
    End Function

    Private Shared Function nullable_compare() As Boolean
        Dim i As Int32? = Nothing
        i = 100
        assert_equal(compare(i, 100), 0)
        assert_false(compare(i, 100.0, 0))
        i = Nothing
        assert_not_equal(compare(i, 100), 0)
        Return True
    End Function


    Public Overrides Function run() As Boolean
        Using regional_atomic_bool(suppress.compare_error)
            Return value_type_with_null() AndAlso
                   value_type_object_compare() AndAlso
                   value_with_reference_compare() AndAlso
                   nullable_compare()
        End Using
    End Function
End Class
