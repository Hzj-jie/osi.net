
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class container_operator(Of T)
    Public Interface enumerator
        Function [end]() As Boolean
        Function current() As T
        Sub [next]()
    End Interface

    Public Shared Function map_enumerator(Of R)(ByVal i As enumerator,
                                                ByVal f As Func(Of T, R)) As container_operator(Of R).enumerator
        Return New enumerator_mapper(Of R)(i, f)
    End Function

    Private NotInheritable Class enumerator_mapper(Of R)
        Implements container_operator(Of R).enumerator

        Private ReadOnly i As enumerator
        Private ReadOnly f As Func(Of T, R)

        Public Sub New(ByVal i As enumerator, ByVal f As Func(Of T, R))
            assert(Not i Is Nothing)
            assert(Not f Is Nothing)
            Me.i = i
            Me.f = f
        End Sub

        Public Sub [next]() Implements container_operator(Of R).enumerator.next
            i.next()
        End Sub

        Public Function current() As R Implements container_operator(Of R).enumerator.current
            Return f(i.current())
        End Function

        Public Function [end]() As Boolean Implements container_operator(Of R).enumerator.end
            Return i.end()
        End Function
    End Class

    Private Sub New()
    End Sub
End Class

Partial Public Class container_operator(Of CONTAINER, T)
    Public Interface enumerator
        Inherits container_operator(Of T).enumerator
    End Interface
End Class
