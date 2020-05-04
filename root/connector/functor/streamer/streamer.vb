
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class streamer(Of T)
    Private ReadOnly e As container_operator(Of T).enumerator

    Public Sub New(ByVal e As container_operator(Of T).enumerator)
        assert(Not e Is Nothing)
        Me.e = e
    End Sub

    Public Function map(Of R)(ByVal f As Func(Of T, R)) As streamer(Of R)
        Return New streamer(Of R)(container_operator(Of T).map_enumerator(e, f))
    End Function

    Public Function aggregate(ByVal f As Func(Of T, T, T), ByVal r As T) As T
        assert(Not f Is Nothing)
        While Not e.end()
            r = f(r, e.current())
            e.next()
        End While
        Return r
    End Function

    Public Function aggregate(ByVal f As Func(Of T, T, T)) As T
        Return aggregate(f, alloc(Of T)())
    End Function

    Public Function collect(Of CT)(ByVal f As Action(Of CT, T), ByVal c As CT) As CT
        assert(Not f Is Nothing)
        While Not e.end()
            f(c, e.current())
            e.next()
        End While
        Return c
    End Function

    Public Function collect(Of CT)(ByVal c As CT) As CT
        Return collect(Sub(ByVal i As CT, ByVal v As T)
                           container_operator.insert(i, v)
                       End Sub,
                       c)
    End Function

    Public Function collect(Of CT)() As CT
        Return collect(alloc(Of CT)())
    End Function
End Class
