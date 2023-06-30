
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt

Public Class compare_predefined_test
    Inherits [case]

    Private Shared Function value_type_with_null(Of T, T2)() As Boolean
        Dim x As T = Nothing
        Dim y As T2 = Nothing
        Dim r As Int32 = 0
        assertion.is_true(compare(x, y, r))
        assertion.equal(r, 0)
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
        assertion.is_false(object_compare(1, 1, 0))
        assertion.is_false(object_compare(1.0, 1.0, 0))
        assertion.is_false(object_compare(True, True, 0))
        assertion.is_false(object_compare(New s(), New s(), 0))
        assertion.is_false(object_comparable("", 0))
        assertion.is_false(object_compare("", 0, 0))

        assertion.is_true(object_compare(0, Nothing, 0))
        assertion.more(object_compare(-1, Nothing), 0)
        assertion.is_true(object_compare(True, Nothing, 0))
        assertion.more(object_compare(False, Nothing), 0)
        assertion.is_true(object_compare("", Nothing, 0))
        assertion.more(object_compare("", Nothing), 0)
        Return True
    End Function

    Private Shared Function value_with_reference_compare() As Boolean
        Dim obj As Object = Nothing
        obj = 1
        assertion.is_false(object_compare(1, obj, 0))
        Return True
    End Function

    Private Shared Function nullable_compare() As Boolean
        Dim i As Int32? = Nothing
        i = 100
        assertion.equal(compare(i, 100), 0)
        assertion.is_false(compare(i, 100.0, 0))
        i = Nothing
        assertion.not_equal(compare(i, 100), 0)
        Return True
    End Function

    Private Shared Function int32_to_int64() As Boolean
        assertion.is_false(compare(max_int32, max_int64, 0))
        Return True
    End Function

    Private Shared Function array_compare() As Boolean
        Dim x() As Int32 = {1, 2, 3}
        Dim y() As Int32 = {4, 5, 6}
        Dim r As Int32 = 0
        ' TODO: Make this test passs, support array in compare().
        ' assertion.is_true(compare(x, y, r))
        ' assertion.less(r, 0)
        assertion.is_true(compare(x, x, r))
        assertion.equal(r, 0)
        assertion.is_true(compare(y, y, r))
        assertion.equal(r, 0)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Using scoped.atomic_bool(suppress.compare_error)
            Return value_type_with_null() AndAlso
                   value_type_object_compare() AndAlso
                   value_with_reference_compare() AndAlso
                   nullable_compare() AndAlso
                   array_compare()
        End Using
    End Function
End Class
