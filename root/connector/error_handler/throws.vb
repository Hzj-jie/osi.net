
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

    Public Shared Sub not_implemented()
        Throw New NotImplementedException()
    End Sub

    Public Shared Sub out_of_memory(ParamArray ByVal msg() As Object)
        Throw New OutOfMemoryException(strcat(msg))
    End Sub

    Private Sub New()
    End Sub
End Class
