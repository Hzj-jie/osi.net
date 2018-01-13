
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class container_operator
    Public Shared Sub register(Of CONTAINER, T)(ByVal f As Func(Of CONTAINER, T, Boolean))
        container_operator(Of CONTAINER, T).register(f)
    End Sub

    Public Shared Sub register(Of CONTAINER, T) _
                              (ByVal f As Func(Of CONTAINER, container_operator(Of CONTAINER, T).enumerator))
        container_operator(Of CONTAINER, T).register(f)
    End Sub

    Public Shared Function emplace(Of CONTAINER, T)(ByVal i As CONTAINER, ByVal j As T) As Boolean
        Return container_operator(Of CONTAINER, T).default.emplace(i, j)
    End Function

    Public Shared Function insert(Of CONTAINER, T)(ByVal i As CONTAINER, ByVal j As T) As Boolean
        Return container_operator(Of CONTAINER, T).default.insert(i, j)
    End Function

    Private Sub New()
    End Sub
End Class
