
Imports osi.root.connector

Partial Public Class big_uint
    Public NotOverridable Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(cast(Of big_uint)(obj))
    End Function

    Public NotOverridable Overrides Function GetHashCode() As Int32
        If is_zero() Then
            Return 0
        ElseIf is_one() Then
            Return 1
        Else
            Dim h As Int32 = 0
            For i As Int32 = 0 To v.size() - 1
                h = h Xor v(i).GetHashCode()
            Next
            Return h
        End If
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return str()
    End Function
End Class
