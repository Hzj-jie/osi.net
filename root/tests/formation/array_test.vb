
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.utt

Public Class array_test
    Inherits [case]

    Private Shared Function fixed_size_array() As Boolean
        Dim a As array(Of Int32, _100) = Nothing
        a = New array(Of Int32, _100)()
        assert_equal(a.size(), CUInt(100))
        For i As Int32 = 0 To 100 - 1
            a(i) = i
        Next
        For i As Int32 = 0 To 100 - 1
            assert_equal(a(i), i)
        Next
        Return True
    End Function

    Private Shared Function dynamic_size_array() As Boolean
        Dim a As array(Of Int32) = Nothing
        a = New array(Of Int32)(100)
        assert_equal(a.size(), CUInt(100))
        For i As Int32 = 0 To 100 - 1
            a(i) = i
        Next
        For i As Int32 = 0 To 100 - 1
            assert_equal(a(i), i)
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return fixed_size_array() AndAlso
               dynamic_size_array()
    End Function
End Class
