
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class unique_queue(Of T)
    Implements ICloneable, ICloneable(Of unique_queue(Of T))

    Private ReadOnly q As queue(Of T)
    Private ReadOnly s As [set](Of T)

    Private Sub New(ByVal q As queue(Of T), ByVal s As [set](Of T))
        assert(Not q Is Nothing)
        assert(Not s Is Nothing)
        Me.q = q.CloneT()
        Me.s = s.CloneT()
    End Sub

    Public Sub New()
        q = New queue(Of T)()
        s = New [set](Of T)()
    End Sub

    <copy_constructor>
    Public Sub New(ByVal other As unique_queue(Of T))
        Me.New(assert_which.of(other).is_not_null().q, other.s)
    End Sub

    Public Sub clear()
        q.clear()
        s.clear()
    End Sub

    Public Function size() As Int64
        assert(q.size() = s.size(), "queue does not coincide with set.")
        Return q.size()
    End Function

    Public Function empty() As Boolean
        assert(q.empty() = s.empty(), "queue does not coincide with set.")
        Return q.empty()
    End Function

    Public Sub push(ByVal v As T)
        If s.find(v) = s.end() Then
            q.push(v)
            s.insert(v)
        End If
    End Sub

    Public Sub pop()
        Dim v As T = q.front()
        q.pop()
        assert(s.erase(v), "queue does not coincide with set.")
    End Sub

    Public Function front() As T
        Return q.front()
    End Function

    Public Function back() As T
        Return q.back()
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As unique_queue(Of T) Implements ICloneable(Of unique_queue(Of T)).Clone
        Return New unique_queue(Of T)(q, s)
    End Function
End Class
