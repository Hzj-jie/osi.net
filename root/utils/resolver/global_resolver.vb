
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class global_resolver
    Public Shared Function resolve(Of T As Class)(ByRef o As T) As Boolean
        Return global_resolver(Of T).resolve(o)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class global_resolver(Of T As Class)
    Private Shared ReadOnly r As thread_safe_resolver(Of T)

    Shared Sub New()
        r = New thread_safe_resolver(Of T)()
    End Sub

    Public Shared Sub register(ByVal i As T)
        r.register(i)
    End Sub

    Public Shared Sub register(ByVal i As Func(Of T))
        r.register(i)
    End Sub

    Public Shared Function resolve(ByRef o As T) As Boolean
        Return r.resolve(o)
    End Function

    Public Shared Function resolve() As T
        Return r.resolve()
    End Function

    Public Shared Function resolve_or_null() As T
        Return r.resolve_or_null()
    End Function

    Private Sub New()
    End Sub
End Class
