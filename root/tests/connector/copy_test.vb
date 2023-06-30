
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class copy_test
    Inherits [case]

    Private Shared Function copy_clone_case() As Boolean
        Dim a() As Int32 = Nothing
        ReDim a(rnd_int(5, 10))
        For i As Int32 = 0 To array_size_i(a) - 1
            a(i) = rnd_int(min_int32, max_int32)
        Next
        assertion.is_true(TypeOf a Is ICloneable)
        Dim b() As Int32 = Nothing
        For i As Int32 = 0 To 128 - 1
            assertion.is_true(copy(b, a))
            assertion.equal(array_size(a), array_size(b))
            assertion.not_equal(object_compare(a, b), 0)
        Next
        For i As Int32 = 0 To array_size_i(a) - 1
            assertion.equal(a(i), b(i))
            a(i) += 1
            assertion.not_equal(a(i), b(i))
        Next
        Return True
    End Function

    Private Shared Function copy_no_clone_case() As Boolean
        Dim a As Int32 = 0
        a = rnd_int(min_int32, max_int32)
        Dim b As Int32 = 0
        assertion.is_true(copy(b, a))
        assertion.equal(a, b)
        Return True
    End Function

    Private Shared Function copy_object_case() As Boolean
        Dim a() As Int32 = Nothing
        ReDim a(rnd_int(5, 10))
        For i As Int32 = 0 To array_size_i(a) - 1
            a(i) = rnd_int(min_int32, max_int32)
        Next
        Dim b() As Int32 = Nothing
        For i As Int32 = 0 To 128 - 1
            assertion.is_true(implicit_conversions.copy_test_copy_object_case(b, a))
            assertion.equal(array_size(a), array_size(b))
            assertion.not_equal(object_compare(a, b), 0)
        Next
        For i As Int32 = 0 To array_size_i(a) - 1
            assertion.equal(a(i), b(i))
            a(i) += 1
            assertion.not_equal(a(i), b(i))
        Next
        Return True
    End Function

    Private Shared Function copy_non_clone_object_case() As Boolean
        Dim a As Int32 = 0
        a = rnd_int(min_int32, max_int32)
        Dim b As Int32 = 0
        Dim c As Int32 = 0
        Do
            c = rnd_int(min_int32, max_int32)
        Loop Until c <> a
        b = c
        assertion.is_false(implicit_conversions.copy_test_copy_non_clone_object_case(b, a))
        assertion.not_equal(a, b)
        assertion.equal(b, c)
        Return True
    End Function

    Private Shared Function copy_array_case() As Boolean
        Const size As Int32 = 10
        Dim a() As Object = Nothing
        ReDim a(size - 1)
        For i As Int32 = 0 To size - 1
            a(i) = New Object()
        Next

        Dim b() As Object = Nothing
        assertion.is_true(copy(b, a))
        assertion.equal(array_size_i(b), size)
        For i As Int32 = 0 To size - 1
            assertion.reference_equal(a(i), b(i))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 32 - 1
            If Not copy_clone_case() OrElse
               Not copy_no_clone_case() OrElse
               Not copy_object_case() OrElse
               Not copy_non_clone_object_case() Then
                Return False
            End If
        Next
        Return copy_array_case()
    End Function
End Class
