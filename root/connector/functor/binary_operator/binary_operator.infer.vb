
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class binary_operator
    Public Shared Sub register_add(Of T, T2, RT)(ByVal f As Func(Of T, T2, RT))
        binary_operator(Of T, T2, RT).register_add(f)
    End Sub

    Public Shared Sub register_minus(Of T, T2, RT)(ByVal f As Func(Of T, T2, RT))
        binary_operator(Of T, T2, RT).register_minus(f)
    End Sub

    Public Shared Sub register_multiply(Of T, T2, RT)(ByVal f As Func(Of T, T2, RT))
        binary_operator(Of T, T2, RT).register_multiply(f)
    End Sub

    Public Shared Function add(Of T)(ByVal i As T, ByVal j As T) As T
        Return binary_operator(Of T).r.add(i, j)
    End Function

    Public Shared Function minus(Of T)(ByVal i As T, ByVal j As T) As T
        Return binary_operator(Of T).r.minus(i, j)
    End Function

    Public Shared Function multiply(Of T)(ByVal i As T, ByVal j As T) As T
        Return binary_operator(Of T).r.multiply(i, j)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class binary_operator(Of T)
    Inherits binary_operator(Of T, T, T)

    Protected Sub New()
    End Sub
End Class
