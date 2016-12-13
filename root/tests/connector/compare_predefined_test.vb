
Imports osi.root.connector
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

    Public Overrides Function run() As Boolean
        Return value_type_with_null()
    End Function
End Class
