
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class stream(Of T)
    Private ReadOnly e As container_operator(Of T).enumerator

    Public Sub New(ByVal e As container_operator(Of T).enumerator)
        assert(Not e Is Nothing)
        Me.e = e
    End Sub

    Public Function map(Of R)(ByVal f As Func(Of T, R)) As stream(Of R)
        Return New stream(Of R)(streams.enumerators.map(e, f))
    End Function

    ' A + A + ... => A
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

    ' A + A + ... => B
    Public Function collect(Of CT)(ByVal f As Action(Of CT, T), ByVal c As CT) As CT
        assert(Not f Is Nothing)
        While Not e.end()
            f(c, e.current())
            e.next()
        End While
        Return c
    End Function

    Public Function collect_by(Of CT)(ByVal f As Action(Of CT, T)) As CT
        Return collect(f, alloc(Of CT)())
    End Function

    Public Function collect_to(Of CT)(ByVal c As CT) As CT
        Return collect(Sub(ByVal i As CT, ByVal v As T)
                           container_operator.insert(i, v)
                       End Sub,
                       c)
    End Function

    Public Function collect(Of CT)() As CT
        Return collect_to(alloc(Of CT)())
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
        Return New stream(Of T)(streams.enumerators.limit_count(e, count))
    End Function

    Public Sub foreach(ByVal f As Action(Of T))
        assert(Not f Is Nothing)
        While Not e.end()
            Try
                f(e.current())
            Catch ex As break_lambda
                Return
            Finally
                e.next()
            End Try
        End While
    End Sub

    Public Function filter(ByVal f As Func(Of T, Boolean)) As stream(Of T)
        Return New stream(Of T)(streams.enumerators.filter(e, f))
    End Function

    Public Function to_array() As T()
        Return +collect(Of vector(Of T))()
    End Function

    Public Function flat_map(Of R)(ByVal f As Func(Of T, stream(Of R))) As stream(Of R)
        Return New stream(Of R)(streams.enumerators.concat(
                                    map(f).
                                    map(Function(ByVal s As stream(Of R)) As container_operator(Of R).enumerator
                                            Return s.e
                                        End Function).
                                    to_array()))
    End Function

    Public Function find_first() As [optional](Of T)
        If e.end() Then
            Return [optional].empty(Of T)()
        End If
        Dim r As [optional](Of T) = Nothing
        r = [optional].of(e.current())
        e.next()
        Return r
    End Function

    Public Function sort() As stream(Of T)
        Return collect_by(collectors.sorted_frequency()).
               stream().
               flat_map(Function(ByVal i As first_const_pair(Of T, UInt32)) As stream(Of T)
                            Return streams.repeat(i.first, i.second)
                        End Function)
    End Function

    Public Function sort(ByVal cmp As Func(Of T, T, Int32)) As stream(Of T)
        assert(Not cmp Is Nothing)
        Return map(Function(ByVal i As T) As compare_node
                       Return New compare_node(i, cmp)
                   End Function).
               sort().
               map(Function(ByVal i As compare_node) As T
                       Return i.v
                   End Function)
    End Function

    Public Function count() As stream(Of tuple(Of T, UInt32))
        Return collect_by(collectors.frequency()).
               stream().
               map(AddressOf tuple(Of T, UInt32).from_first_const_pair)
    End Function

    Public Function with_index() As stream(Of tuple(Of UInt32, T))
        Return collect_by(collectors.with_index()).
               stream()
    End Function

    Public Function concat(ByVal e As container_operator(Of T).enumerator) As stream(Of T)
        Return New stream(Of T)(streams.enumerators.concat(Me.e, e))
    End Function

    Public Function concat(ByVal s As stream(Of T)) As stream(Of T)
        assert(Not s Is Nothing)
        Return concat(s.e)
    End Function

    Public Function concat(ByVal ParamArray others() As T) As stream(Of T)
        Return concat(streams.enumerators.from_array(others))
    End Function
End Class
