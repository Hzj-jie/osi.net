
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class container_operator
    Public Shared Sub emplace(Of CONTAINER, T)(ByVal f As Func(Of CONTAINER, T, Boolean))
        container_operator(Of CONTAINER, T).emplace(f)
    End Sub

    Public Shared Sub enumerate(Of CONTAINER, T) _
                               (ByVal f As Func(Of CONTAINER, container_operator(Of CONTAINER, T).enumerator))
        container_operator(Of CONTAINER, T).enumerate(f)
    End Sub

    Public Shared Function emplace(Of CONTAINER, T)(ByVal i As CONTAINER, ByVal j As T) As Boolean
        Return container_operator(Of CONTAINER, T).default.emplace(i, j)
    End Function

    Public Shared Function insert(Of CONTAINER, T)(ByVal i As CONTAINER, ByVal j As T) As Boolean
        Return container_operator(Of CONTAINER, T).default.insert(i, j)
    End Function

    Public Shared Function size(Of CONTAINER, T)(ByVal i As CONTAINER) As UInt32
        Return container_operator(Of CONTAINER, T).default.size(i)
    End Function

    Public Shared Sub clear(Of CONTAINER, T)(ByVal i As CONTAINER)
        container_operator(Of CONTAINER, T).default.clear(i)
    End Sub

    Private Sub New()
    End Sub
End Class
