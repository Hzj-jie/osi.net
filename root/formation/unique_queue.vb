
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class unique_queue(Of T As IComparable(Of T))
    Implements ICloneable

    Private queue As queue(Of T) = Nothing
    Private [set] As [set](Of T) = Nothing

    Public Sub clear()
        queue.clear()
        [set].clear()
    End Sub

    Public Function size() As Int64
        assert(queue.size() = [set].size(), "queue does not coincide with set.")
        Return queue.size()
    End Function

    Public Function empty() As Boolean
        assert(queue.empty() = [set].empty(), "queue does not coincide with set.")
        Return queue.empty()
    End Function

    Public Sub push(ByVal v As T)
        If [set].find(v) = [set].end() Then
            queue.push(v)
            [set].insert(v)
        End If
    End Sub

    Public Sub pop()
        Dim v As T = queue.front()
        queue.pop()
        assert([set].erase(v), "queue does not coincide with set.")
    End Sub

    Public Function front() As T
        Return queue.front()
    End Function

    Public Function back() As T
        Return queue.back()
    End Function

    Public Sub New()
        queue = New queue(Of T)()
        [set] = New [set](Of T)()
    End Sub

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim that As unique_queue(Of T) = Nothing
        that = New unique_queue(Of T)()
        copy(that.queue, queue)
        copy(that.set, [set])

        Return that
    End Function
End Class
