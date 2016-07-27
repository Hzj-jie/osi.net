
Imports osi.root.connector
Imports osi.root.utt

Public Class cast_test
    Inherits [case]

    Public Shared Function failed_case(Of T, T2)(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        assert_false(cast(Of T2)(i, j))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return value_types() AndAlso
               reference_types()
    End Function
End Class
