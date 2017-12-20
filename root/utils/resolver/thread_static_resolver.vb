
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class thread_static_resolver
    Public Shared Function resolve(Of T As Class)(ByRef o As T) As Boolean
        Return thread_static_resolver(Of T).resolve(o)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class thread_static_resolver(Of T As Class)
    <ThreadStatic()> Private Shared resolver As resolver(Of T)

    Private Shared Function create_resolver() As resolver(Of T)
        Dim r As resolver(Of T) = Nothing
        r = resolver
        If r Is Nothing Then
            r = New resolver(Of T)()
            resolver = r
        End If
        assert(Not r Is Nothing)
        Return r
    End Function

    Public Shared Sub register(ByVal i As T)
        create_resolver().register(i)
    End Sub

    ' thread_static_resolver.register(Of T)(ByVal i As Func(Of T))
    ' cannot work as expected, the compiler automatically infers to
    ' thread_static_resolver.register(Of T As Func(Of ?))(ByVal i As T).
    Public Shared Sub register(ByVal i As Func(Of T))
        create_resolver().register(i)
    End Sub

    Public Shared Function resolve(ByRef o As T) As Boolean
        Return create_resolver().resolve(o)
    End Function

    Public Shared Function resolve() As T
        Return create_resolver().resolve()
    End Function

    Public Shared Function resolve_or_null() As T
        Return create_resolver().resolve_or_null()
    End Function

    Private Sub New()
    End Sub
End Class
