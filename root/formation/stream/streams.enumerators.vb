
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class streams
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

        Public Shared Function concat(Of T)(ByVal ParamArray e() As container_operator(Of T).enumerator) _
                                           As container_operator(Of T).enumerator
            Return New concator(Of T)(e)
        End Function

        Public Shared Function from_array(Of T)(ByVal e() As T) As container_operator(Of T).enumerator
            Return New array_wrapper(Of T)(e)
        End Function

        Public Shared Function repeat(Of T)(ByVal e As T, ByVal count As UInt32) As container_operator(Of T).enumerator
            Return New repeater(Of T)(e, count)
        End Function

        Public Shared Function range(ByVal start As Int32,
                                     ByVal [end] As Int32) As container_operator(Of Int32).enumerator
            Return New ranger(start, [end])
        End Function

        Public Shared Function range(ByVal start As UInt32,
                                     ByVal [end] As UInt32) As container_operator(Of Int32).enumerator
            assert(start <= max_int32)
            assert([end] <= max_int32)
            Return New ranger(CInt(start), CInt([end]))
        End Function

        Private Structure filterer(Of T)
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
        End Structure

        Private Structure mapper(Of T, R)
            Implements container_operator(Of R).enumerator

            Private ReadOnly i As container_operator(Of T).enumerator
            Private ReadOnly f As Func(Of T, R)

            Public Sub New(ByVal i As container_operator(Of T).enumerator, ByVal f As Func(Of T, R))
                assert(Not GetType(R).generic_type_is(GetType(container_operator(Of ))))

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
        End Structure

        Private Structure limiter(Of T)
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
        End Structure

        Private Structure concator(Of T)
            Implements container_operator(Of T).enumerator

            Private ReadOnly e() As container_operator(Of T).enumerator
            Private i As Int32

            Public Sub New(ByVal e() As container_operator(Of T).enumerator)
                Me.e = e
            End Sub

            Private Sub next_available()
                While i < array_size_i(e) AndAlso e(i).end()
                    i += 1
                End While
            End Sub

            Public Sub [next]() Implements container_operator(Of T).enumerator.next
                e(i).next()
                next_available()
            End Sub

            Public Function current() As T Implements container_operator(Of T).enumerator.current
                next_available()
                Return e(i).current()
            End Function

            Public Function [end]() As Boolean Implements container_operator(Of T).enumerator.end
                next_available()
                Return i = array_size_i(e)
            End Function
        End Structure

        Private Structure array_wrapper(Of T)
            Implements container_operator(Of T).enumerator

            Private ReadOnly e() As T
            Private i As Int32

            Public Sub New(ByVal e() As T)
                Me.e = e
                Me.i = 0
            End Sub

            Public Sub [next]() Implements container_operator(Of T).enumerator.next
                i += 1
            End Sub

            Public Function current() As T Implements container_operator(Of T).enumerator.current
                Return e(i)
            End Function

            Public Function [end]() As Boolean Implements container_operator(Of T).enumerator.end
                Return i >= array_size_i(e)
            End Function
        End Structure

        Private Structure repeater(Of T)
            Implements container_operator(Of T).enumerator

            Private ReadOnly e As T
            Private count As UInt32

            Public Sub New(ByVal e As T, ByVal count As UInt32)
                Me.e = e
                Me.count = count
            End Sub

            Public Sub [next]() Implements container_operator(Of T).enumerator.next
                If Not [end]() Then
                    count -= uint32_1
                End If
            End Sub

            Public Function current() As T Implements container_operator(Of T).enumerator.current
                assert(Not [end]())
                Return e
            End Function

            Public Function [end]() As Boolean Implements container_operator(Of T).enumerator.end
                Return count = 0
            End Function
        End Structure

        Private Structure ranger
            Implements container_operator(Of Int32).enumerator

            Private ReadOnly e As Int32
            Private i As Int32

            Public Sub New(ByVal start As Int32, ByVal [end] As Int32)
                assert(start <= [end])
                e = [end]
                i = start
            End Sub

            Public Sub [next]() Implements container_operator(Of Int32).enumerator.next
                i += 1
            End Sub

            Public Function current() As Int32 Implements container_operator(Of Int32).enumerator.current
                Return i
            End Function

            Public Function [end]() As Boolean Implements container_operator(Of Int32).enumerator.end
                Return i = e
            End Function
        End Structure

        Private Sub New()
        End Sub
    End Class
End Class
