
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_int
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return equal(cast(Of big_int)(obj))
    End Function

    Public Overrides Function GetHashCode() As Int32
        If positive() Then
            Return d.GetHashCode()
        End If
        Return Not d.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
