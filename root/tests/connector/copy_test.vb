
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class copy_test
    Inherits [case]

    Private Shared Function copy_clone_case() As Boolean
        Dim a() As Int32 = Nothing
        ReDim a(rnd_int(5, 10))
        For i As Int32 = 0 To array_size(a) - 1
            a(i) = rnd_int(min_int32, max_int32)
        Next
        assert_true(TypeOf a Is ICloneable)
        Dim b() As Int32 = Nothing
        For i As Int32 = 0 To 128 - 1
            assert_true(copy(b, a))
            assert_equal(array_size(a), array_size(b))
            assert_not_equal(object_compare(a, b), 0)
        Next
        For i As Int32 = 0 To array_size(a) - 1
            assert_equal(a(i), b(i))
            a(i) += 1
            assert_not_equal(a(i), b(i))
        Next
        Return True
    End Function

    Private Shared Function copy_no_clone_case() As Boolean
        Dim a As Int32 = 0
        a = rnd_int(min_int32, max_int32)
        Dim b As Int32 = 0
        assert_true(copy(b, a))
        assert_equal(a, b)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 32 - 1
            If Not copy_clone_case() OrElse
               Not copy_no_clone_case() Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
