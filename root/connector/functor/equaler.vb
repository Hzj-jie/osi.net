
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class equaler
    Public Shared Sub register(Of T, T2)(ByVal f As Func(Of T, T2, Boolean))
        equaler(Of T, T2).register(f)
    End Sub

    Public Shared Function equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return equaler(Of T, T2).equal(this, that)
    End Function

    Private Sub New()
    End Sub
End Class

' Implementations can assume i and j are not null. Users should not use this class directly except for the registration.
Public NotInheritable Class equaler(Of T, T2)
    Public Shared Sub register(ByVal f As Func(Of T, T2, Boolean))
        assert(Not f Is Nothing)
        register(Of T, T2)(f)
        If Not type_info(Of T, type_info_operators.is, T2).v Then
            register(Of T2, T)(Function(ByVal i As T2, ByVal j As T) As Boolean
                                   Return f(j, i)
                               End Function)
        End If
    End Sub

    Private Shared Sub register(Of IT, IT2)(ByVal f As Func(Of IT, IT2, Boolean))
        global_resolver(Of Func(Of IT, IT2, Boolean), equaler(Of IT, IT2)).assert_first_register(f)
    End Sub

    Public Shared Sub unregister()
        global_resolver(Of Func(Of T, T2, Boolean), equaler(Of T, T2)).assert_unregister()
    End Sub

    Public Shared Function defined() As Boolean
        Return global_resolver(Of Func(Of T, T2, Boolean), equaler(Of T, T2)).registered()
    End Function

    Public Shared Function ref() As Func(Of T, T2, Boolean)
        Return global_resolver(Of Func(Of T, T2, Boolean), equaler(Of T, T2)).resolve_or_null()
    End Function

    Public Shared Function equal(ByVal i As T, ByVal j As T2) As Boolean
        Return global_resolver(Of Func(Of T, T2, Boolean), equaler(Of T, T2)).resolve()(i, j)
    End Function

    Private Sub New()
    End Sub
End Class
