
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class container_operator(Of CONTAINER, T)
    Public Function new_or_clear(ByRef i As CONTAINER) As CONTAINER
        If i Is Nothing Then
            i = alloc(Of CONTAINER)()
        Else
            clear(i)
        End If
        Return i
    End Function
End Class
