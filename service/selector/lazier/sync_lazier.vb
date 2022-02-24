
Imports System.Threading
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils

Public Class sync_lazier(Of T, THREAD_SAFE As _boolean)
    Private Shared ReadOnly threadsafe As Boolean

    Shared Sub New()
        threadsafe = +alloc(Of THREAD_SAFE)()
    End Sub

    Private ReadOnly ts As sync_thread_safe_lazier(Of T)
    Private ReadOnly tus As sync_thread_unsafe_lazier(Of T)

    Public Sub New(ByVal d As Func(Of T))
        If threadsafe Then
            ts = New sync_thread_safe_lazier(Of T)(d)
        Else
            tus = New sync_thread_unsafe_lazier(Of T)(d)
        End If
    End Sub

    Public Function alive() As ternary
        If threadsafe Then
            assert(Not ts Is Nothing)
            Return ts.alive()
        Else
            assert(Not tus Is Nothing)
            Return tus.alive()
        End If
    End Function

    Public Function [get]() As T
        If threadsafe Then
            assert(Not ts Is Nothing)
            Return ts.get()
        Else
            assert(Not tus Is Nothing)
            Return tus.get()
        End If
    End Function
End Class

Public Class sync_thread_unsafe_lazier(Of T)
    Private ReadOnly d As Func(Of T)
    Private state As ternary
    Private v As T

    Public Sub New(ByVal d As Func(Of T))
        assert(Not d Is Nothing)
        Me.d = d
        Me.state = ternary.unknown
        Me.v = Nothing
    End Sub

    Protected Overridable Function prepare() As T
        assert(Not d Is Nothing)
        Return d()
    End Function

    Public Function alive() As ternary
        Return state
    End Function

    Public Function [get]() As T
        If state.unknown_() Then
            assert(v Is Nothing OrElse type_info(Of T).is_valuetype)
            Try
                v = prepare()
            Catch ex As Exception
                assert(False, ex)
            End Try
            state = Not v Is Nothing
        End If
        Return v
    End Function
End Class

Public Class sync_thread_safe_lazier(Of T)
    Private ReadOnly finish_get As ManualResetEvent
    Private ReadOnly d As Func(Of T)
    Private state As ternary
    Private begin_get As singleentry
    Private v As T

    Public Sub New(ByVal d As Func(Of T))
        Me.finish_get = New ManualResetEvent(False)
        assert(Not d Is Nothing)
        Me.d = d
        Me.state = ternary.unknown
        Me.v = Nothing
    End Sub

    Protected Overridable Function prepare() As T
        assert(Not d Is Nothing)
        Return d()
    End Function

    Public Function alive() As ternary
        Return state
    End Function

    Public Function [get]() As T
        If state.unknown_() Then
            If begin_get.mark_in_use() Then
                assert(v Is Nothing OrElse type_info(Of T).is_valuetype)
                Try
                    v = prepare()
                Catch ex As Exception
                    assert(False, ex)
                End Try
                assert(finish_get.Set())
                state = Not v Is Nothing
            Else
                assert(finish_get.WaitOne())
            End If
        End If
        Return v
    End Function

    Protected Overrides Sub Finalize()
        finish_get.Close()
        MyBase.Finalize()
    End Sub
End Class
