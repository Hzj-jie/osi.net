
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class throws
    Public Shared Function not_null(Of T)(ByVal i As T) As T
        If i Is Nothing Then
            Throw New NullReferenceException()
        End If
        Return i
    End Function

    Private Sub New()
    End Sub
End Class
