
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public Class stream(Of T)
    Private ReadOnly e As container_operator(Of T).enumerator

    Public Sub New(ByVal e As container_operator(Of T).enumerator)
        assert(Not e Is Nothing)
        Me.e = e
    End Sub

    Public Function map(Of R)(ByVal f As Func(Of T, R)) As stream(Of R)
        Return New stream(Of R)(container_operator.enumerators.map(e, f))
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

    Public Function collect(Of CT)(ByVal f As Action(Of CT, T)) As CT
        Return collect(f, alloc(Of CT)())
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

    Public Function skip(ByVal count As UInt32) As stream(Of T)
        Dim i As UInt32 = 0
        While i < count AndAlso Not e.end()
            e.next()
            i += uint32_1
        End While
        Return Me
    End Function

    Public Function limit(ByVal count As UInt32) As stream(Of T)
        Return New stream(Of T)(container_operator.enumerators.limit_count(e, count))
    End Function

    Public Sub foreach(ByVal f As Action(Of T))
        assert(Not f Is Nothing)
        While Not e.end()
            f(e.current())
            e.next()
        End While
    End Sub

    Public Function filter(ByVal f As Func(Of T, Boolean)) As stream(Of T)
        Return New stream(Of T)(container_operator.enumerators.filter(e, f))
    End Function
End Class
