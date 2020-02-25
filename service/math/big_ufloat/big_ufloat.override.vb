
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class big_ufloat
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return Equals(cast(Of big_ufloat)().from(obj, False))
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return i.GetHashCode() Or d.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
