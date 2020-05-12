
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class container_operator
    Public NotInheritable Class enumerators
        Public Shared Function map(Of T, R)(ByVal i As container_operator(Of T).enumerator,
                                            ByVal f As Func(Of T, R)) As container_operator(Of R).enumerator
            Return New mapper(Of T, R)(i, f)
        End Function

        Public Shared Function limit_count(Of T)(ByVal i As container_operator(Of T).enumerator,
                                                 ByVal c As UInt32) As container_operator(Of T).enumerator
            Return New limiter(Of T)(c, i)
        End Function

        Public Shared Function filter(Of T)(ByVal i As container_operator(Of T).enumerator,
                                            ByVal f As Func(Of T, Boolean)) As container_operator(Of T).enumerator
            Return New filterer(Of T)(i, f)
        End Function

        Private NotInheritable Class filterer(Of T)
            Implements container_operator(Of T).enumerator

            Private ReadOnly i As container_operator(Of T).enumerator
            Private ReadOnly f As Func(Of T, Boolean)

            Public Sub New(ByVal i As container_operator(Of T).enumerator, ByVal f As Func(Of T, Boolean))
                assert(Not i Is Nothing)
                assert(Not f Is Nothing)
                Me.i = i
                Me.f = f
            End Sub

            Private Sub ignore_filtered()
                While Not i.end()
                    If f(i.current()) Then
                        Return
                    End If
                    i.next()
                End While
            End Sub

            Public Sub [next]() Implements container_operator(Of T).enumerator.next
                i.next()
                ignore_filtered()
            End Sub

            Public Function current() As T Implements container_operator(Of T).enumerator.current
                ignore_filtered()
                Return i.current()
            End Function

            Public Function [end]() As Boolean Implements container_operator(Of T).enumerator.end
                ignore_filtered()
                Return i.end()
            End Function
        End Class

        Private NotInheritable Class mapper(Of T, R)
            Implements container_operator(Of R).enumerator

            Private ReadOnly i As container_operator(Of T).enumerator
            Private ReadOnly f As Func(Of T, R)

            Public Sub New(ByVal i As container_operator(Of T).enumerator, ByVal f As Func(Of T, R))
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

        Private NotInheritable Class limiter(Of T)
            Implements container_operator(Of T).enumerator

            Private ReadOnly c As UInt32
            Private ReadOnly e As container_operator(Of T).enumerator
            Private v As UInt32

            Public Sub New(ByVal c As UInt32, ByVal e As container_operator(Of T).enumerator)
                assert(Not e Is Nothing)
                Me.c = c
                Me.e = e
                Me.v = uint32_0
            End Sub

            Public Sub [next]() Implements container_operator(Of T).enumerator.next
                assert(v < c)
                v += uint32_1
                e.next()
            End Sub

            Public Function current() As T Implements container_operator(Of T).enumerator.current
                Return e.current()
            End Function

            Public Function [end]() As Boolean Implements container_operator(Of T).enumerator.end
                Return e.end() OrElse v = c
            End Function
        End Class

        Private Sub New()
        End Sub
    End Class
End Class

Public NotInheritable Class container_operator(Of T)
    Public Interface enumerator
        Function [end]() As Boolean
        Function current() As T
        Sub [next]()
    End Interface

    Private Sub New()
    End Sub
End Class

Partial Public Class container_operator(Of CONTAINER, T)
    Public Interface enumerator
        Inherits container_operator(Of T).enumerator
    End Interface
End Class
