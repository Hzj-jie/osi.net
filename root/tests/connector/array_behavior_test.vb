
Imports osi.root.connector
Imports osi.root.utt

Public Class array_behavior_test
    Inherits [case]

    Private Shared Function array_clone_case1() As Boolean
        Dim a() As Int32 = Nothing
        ReDim a(4)
        For i As Int32 = 0 To 4
            a(i) = i
        Next
        Dim b() As Int32 = Nothing
        copy(b, a)
        For i As Int32 = 0 To 4
            assert_equal(b(i), i)
            b(i) = i + 1
        Next
        For i As Int32 = 0 To 4
            assert_equal(a(i), i)
            assert_equal(b(i), i + 1)
        Next

        b = DirectCast(a.Clone(), Int32())
        For i As Int32 = 0 To 4
            assert_equal(b(i), i)
            b(i) = i + 1
        Next
        For i As Int32 = 0 To 4
            assert_equal(a(i), i)
            assert_equal(b(i), i + 1)
        Next
        Return True
    End Function

    Private Shared Function array_clone_case2() As Boolean
        Dim base() As Object = Nothing
        ReDim base(4)
        For i As Int32 = 0 To 4
            base(i) = New Object()
        Next
        Dim a() As Object = Nothing
        ReDim a(4)
        For i As Int32 = 0 To 4
            a(i) = base(i)
        Next
        Dim b() As Object = Nothing
        copy(b, a)
        For i As Int32 = 0 To 4
            assert_reference_equal(a(i), base(i))
            assert_reference_equal(a(i), b(i))
            b(i) = New Object()
            assert_reference_equal(a(i), base(i))
            assert_not_reference_equal(a(i), b(i))
            assert_not_reference_equal(b(i), base(i))
        Next

        b = DirectCast(a.Clone(), Object())
        For i As Int32 = 0 To 4
            assert_reference_equal(a(i), base(i))
            assert_reference_equal(a(i), b(i))
            b(i) = New Object()
            assert_reference_equal(a(i), base(i))
            assert_not_reference_equal(a(i), b(i))
            assert_not_reference_equal(b(i), base(i))
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return array_clone_case1() AndAlso
               array_clone_case2()
    End Function
End Class
