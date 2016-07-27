
Imports osi.root.connector

Partial Public Class big_int
    Public NotOverridable Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(cast(Of big_int)(obj))
    End Function

    Public NotOverridable Overrides Function GetHashCode() As Int32
        If positive() Then
            Return d.GetHashCode()
        Else
            Return Not d.GetHashCode()
        End If
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return str()
    End Function
End Class
