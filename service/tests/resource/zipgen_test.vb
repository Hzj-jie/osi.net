
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class zipgen_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        If assert_equal(array_size(data), CUInt(1349206)) AndAlso
           assert_equal(array_size(data), array_size(zipdata)) Then
            For i As UInt32 = 0 To array_size(data) - 1
                assert_equal(data(i), zipdata(i))
            Next
        End If
        Return True
    End Function
End Class
