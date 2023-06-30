
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(cast(Of big_udec)().from(obj, False))
    End Function

    Public Overrides Function GetHashCode() As Int32
        If is_zero() Then
            Return 0
        End If
        Return n.GetHashCode() Or d.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
