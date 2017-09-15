
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class equaler
    Public Shared Sub register(Of T, T2)(ByVal f As Func(Of T, T2, Boolean))
        equaler(Of T, T2).register(f)
        If Not type_info(Of T, type_info_operators.is, T2).v Then
            equaler(Of T2, T).register(Function(ByVal i As T2, ByVal j As T) As Boolean
                                           Return f(j, i)
                                       End Function)
        End If
    End Sub

    Public Shared Function equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return equaler(Of T, T2).equal(this, that)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class equaler(Of T, T2)
    Private Shared f As Func(Of T, T2, Boolean)

    Public Shared Sub register(ByVal f As Func(Of T, T2, Boolean))
        assert(Not f Is Nothing)
        assert(equaler(Of T, T2).f Is Nothing OrElse object_compare(equaler(Of T, T2).f, f) = 0)
        equaler(Of T, T2).f = f
    End Sub

    Public Shared Sub unregister()
        assert(Not f Is Nothing)
        f = Nothing
    End Sub

    Public Shared Function defined() As Boolean
        Return Not f Is Nothing
    End Function

    Public Shared Function ref() As Func(Of T, T2, Boolean)
        Return f
    End Function

    Public Shared Function equal(ByVal i As T, ByVal j As T2) As Boolean
        assert(Not f Is Nothing)
        Return f(i, j)
    End Function

    Private Sub New()
    End Sub
End Class
