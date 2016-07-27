
Imports System.Runtime.CompilerServices
Imports osi.root.lock.atomic
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.lock.slimlock
Imports osi.root.delegates
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public Module _event_comb_lock
    <Extension()> Public Sub release(ByVal i As ref(Of event_comb_lock))
        assert(Not i Is Nothing)
        i.p.release()
    End Sub

    <Extension()> Public Sub wait(ByVal i As ref(Of event_comb_lock))
        assert(Not i Is Nothing)
        i.p.wait()
    End Sub

    <Extension()> Public Function locked(ByVal i As ref(Of event_comb_lock),
                                         ByVal d As Func(Of event_comb)) As event_comb
        assert(Not i Is Nothing)
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Return waitfor(i) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  ec = d()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  i.release()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function locked(ByVal i As ref(Of event_comb_lock),
                                         ByVal d As Func(Of Boolean)) As event_comb
        assert(Not i Is Nothing)
        assert(Not d Is Nothing)
        Return New event_comb(Function() As Boolean
                                  Return waitfor(i) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim r As Boolean = False
                                  Try
                                      r = d()
                                  Finally
                                      i.release()
                                  End Try
                                  Return r AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function locked(ByVal i As ref(Of event_comb_lock),
                                         ByVal d As Action) As event_comb
        assert(Not d Is Nothing)
        Return locked(i, Function() As Boolean
                             d()
                             Return True
                         End Function)
    End Function
End Module

Public Structure event_comb_lock
    Implements islimlock
    Private Const NOT_IN_USE As Boolean = False
    Private Const IN_USE As Boolean = Not NOT_IN_USE
    Private l As lock_t
    Private inuse As Boolean
    Private q As fixed_queue(Of Action, _max_uint16)

    Shared Sub New()
        assert(DirectCast(Nothing, Boolean) = NOT_IN_USE)
    End Sub

    Public Sub wait() Implements islimlock.wait
        Dim v As Action = Nothing
        v = event_comb.wait()
        assert(Not v Is Nothing)
        l.wait()
        If inuse = IN_USE Then
            q.emplace(v)
        Else
            inuse = IN_USE
            v()
        End If
        l.release()
    End Sub

    Public Sub release() Implements islimlock.release
        Dim v As Action = Nothing
        l.wait()
        assert(inuse = IN_USE)
        If q.empty() Then
            inuse = NOT_IN_USE
        Else
            assert(q.pop(v))
            assert(Not v Is Nothing)
        End If
        l.release()
        If Not v Is Nothing Then
            v()
        End If
    End Sub

    Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
        Return True
    End Function

    Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
        Return True
    End Function
End Structure

Public Structure event_comb_lock(Of SIZE As _int64)
    Implements islimlock
    Private Const NOT_IN_USE As Boolean = False
    Private Const IN_USE As Boolean = Not NOT_IN_USE
    Private l As lock_t
    Private inuse As Boolean
    Private q As fixed_queue(Of Action, SIZE)

    Shared Sub New()
        assert(DirectCast(Nothing, Boolean) = NOT_IN_USE)
    End Sub

    Public Sub wait() Implements islimlock.wait
        Dim v As Action = Nothing
        v = event_comb.wait()
        assert(Not v Is Nothing)
        l.wait()
        If inuse = IN_USE Then
            q.emplace(v)
        Else
            inuse = IN_USE
            v()
        End If
        l.release()
    End Sub

    Public Sub release() Implements islimlock.release
        Dim v As Action = Nothing
        l.wait()
        assert(inuse = IN_USE)
        If q.empty() Then
            inuse = NOT_IN_USE
        Else
            v = q.pop()
            assert(Not v Is Nothing)
        End If
        l.release()
        If Not v Is Nothing Then
            v()
        End If
    End Sub

    Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
        Return True
    End Function

    Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
        Return True
    End Function
End Structure

Public Structure unlimited_event_comb_lock
    Implements islimlock
    Private Const NOT_IN_USE As Boolean = False
    Private Const IN_USE As Boolean = Not NOT_IN_USE
    Private l As lock_t
    Private inuse As Boolean
    Private q As queue(Of pointer(Of Action))

    Shared Sub New()
        assert(DirectCast(Nothing, Boolean) = NOT_IN_USE)
        assert(DirectCast(Nothing, queue(Of pointer(Of Action))) Is Nothing)
    End Sub

    Public Sub wait() Implements islimlock.wait
        create_if_nothing(q)
        Dim v As Action = Nothing
        v = event_comb.wait()
        assert(Not v Is Nothing)
        l.wait()
        If inuse = IN_USE Then
            assert(q.push(make_pointer(v)))
        Else
            inuse = IN_USE
            v()
        End If
        l.release()
    End Sub

    Public Sub release() Implements islimlock.release
        Dim v As Action = Nothing
        l.wait()
        assert(inuse = IN_USE)
        If q.empty() Then
            inuse = NOT_IN_USE
        Else
            v = +(q.front())
            assert(q.pop())
            assert(Not v Is Nothing)
        End If
        l.release()
        If Not v Is Nothing Then
            v()
        End If
    End Sub

    Public Function can_thread_owned() As Boolean Implements islimlock.can_thread_owned
        Return True
    End Function

    Public Function can_cross_thread() As Boolean Implements islimlock.can_cross_thread
        Return True
    End Function
End Structure
