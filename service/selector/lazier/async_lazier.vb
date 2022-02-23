
Imports System.Threading
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.template
Imports osi.root.utils

Public Class async_lazier(Of T, THREAD_SAFE As _boolean)
    Implements async_getter(Of T)
    Private Shared ReadOnly threadsafe As Boolean

    Shared Sub New()
        threadsafe = +alloc(Of THREAD_SAFE)()
    End Sub

    Private ReadOnly ts As async_thread_safe_lazier(Of T)
    Private ReadOnly tus As async_thread_unsafe_lazier(Of T)

    Public Sub New(ByVal d As Func(Of ref(Of T), event_comb))
        If threadsafe Then
            ts = New async_thread_safe_lazier(Of T)(d)
        Else
            tus = New async_thread_unsafe_lazier(Of T)(d)
        End If
    End Sub

    Public Function initialized_wait_handle() As WaitHandle Implements async_getter(Of T).initialized_wait_handle
        If threadsafe Then
            assert(ts IsNot Nothing)
            Return ts.initialized_wait_handle()
        Else
            assert(tus IsNot Nothing)
            Return tus.initialized_wait_handle()
        End If
    End Function

    Public Function alive() As ternary Implements async_getter(Of T).alive
        If threadsafe Then
            assert(ts IsNot Nothing)
            Return ts.alive()
        Else
            assert(tus IsNot Nothing)
            Return tus.alive()
        End If
    End Function

    Public Function [get](ByRef r As T) As Boolean Implements async_getter(Of T).get
        If threadsafe Then
            assert(ts IsNot Nothing)
            Return ts.get(r)
        Else
            assert(tus IsNot Nothing)
            Return tus.get(r)
        End If
    End Function

    Public Function [get](ByVal r As ref(Of T)) As event_comb Implements async_getter(Of T).get
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If threadsafe Then
                                      assert(ts IsNot Nothing)
                                      ec = ts.get(r)
                                  Else
                                      assert(tus IsNot Nothing)
                                      ec = tus.get(r)
                                  End If
                                  assert(ec IsNot Nothing)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class

Public NotInheritable Class async_thread_unsafe_lazier
    Public Shared Function [New](Of T)(ByVal d As Func(Of ref(Of T), event_comb)) _
                                      As async_thread_unsafe_lazier(Of T)
        Return New async_thread_unsafe_lazier(Of T)(d)
    End Function

    Public Shared Function new_sync(Of T)(ByVal d As T) As async_thread_unsafe_lazier(Of T)
        Return New async_thread_unsafe_lazier(Of T)(Function(p As ref(Of T)) As event_comb
                                                        Return sync_async(Sub() eva(p, d))
                                                    End Function)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class async_thread_unsafe_lazier(Of T)
    Implements async_getter(Of T)
    Private ReadOnly d As Func(Of ref(Of T), event_comb)
    Private ReadOnly inited As ManualResetEvent
    Private state As ternary
    Private v As T

    Public Sub New(ByVal d As Func(Of ref(Of T), event_comb))
        assert(d IsNot Nothing)
        Me.d = d
        Me.inited = New ManualResetEvent(False)
        Me.state = ternary.unknown
        Me.v = Nothing
    End Sub

    Private Sub start_initialization()
        If Not initialized() Then
            assert_begin([get](DirectCast(Nothing, ref(Of T))))
        End If
    End Sub

    Public Function initialized_wait_handle() As WaitHandle Implements async_getter(Of T).initialized_wait_handle
        start_initialization()
        Return inited
    End Function

    Public Function alive() As ternary Implements async_getter(Of T).alive
        Return state
    End Function

    Public Function [get](ByRef r As T) As Boolean Implements async_getter(Of T).get
        If initialized() Then
            If initialization_succeeded() Then
                r = v
                Return True
            Else
                Return False
            End If
        Else
            start_initialization()
            Return False
        End If
    End Function

    Protected Overridable Function initialize(ByVal r As ref(Of T)) As event_comb
        assert(d IsNot Nothing)
        Return d(r)
    End Function

    Public Function [get](ByVal r As ref(Of T)) As event_comb Implements async_getter(Of T).get
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If not_initialized() Then
                                      assert(v Is Nothing OrElse type_info(Of T).is_valuetype)
                                      r.renew()
                                      ec = initialize(r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return initialization_succeeded() AndAlso
                                             eva(r, v) AndAlso
                                             goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      v = (+r)
                                  End If
                                  state = ec.end_result()
                                  assert(inited.Set())
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Overrides Sub Finalize()
        inited.Close()
        MyBase.Finalize()
    End Sub
End Class

Public NotInheritable Class async_thread_safe_lazier
    Public Shared Function [New](Of T)(ByVal d As Func(Of ref(Of T), event_comb)) _
                                      As async_thread_safe_lazier(Of T)
        Return New async_thread_safe_lazier(Of T)(d)
    End Function

    Public Shared Function new_sync(Of T)(ByVal d As T) As async_thread_safe_lazier(Of T)
        Return New async_thread_safe_lazier(Of T)(Function(p As ref(Of T)) As event_comb
                                                      Return sync_async(Sub() eva(p, d))
                                                  End Function)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class async_thread_safe_lazier(Of T)
    Implements async_getter(Of T)
    Private ReadOnly finish_get As signal_event
    Private ReadOnly d As Func(Of ref(Of T), event_comb)
    Private ReadOnly inited As ManualResetEvent
    Private state As ternary
    Private begin_get As singleentry
    Private v As T

    Public Sub New(ByVal d As Func(Of ref(Of T), event_comb))
        assert(d IsNot Nothing)
        Me.finish_get = New signal_event()
        Me.d = d
        Me.inited = New ManualResetEvent(False)
        Me.state = ternary.unknown
        Me.v = Nothing
    End Sub

    Private Sub start_initialization()
        If Not initialized() Then
            assert_begin([get](DirectCast(Nothing, ref(Of T))))
        End If
    End Sub

    Public Function initialized_wait_handle() As WaitHandle Implements async_getter(Of T).initialized_wait_handle
        start_initialization()
        Return inited
    End Function

    Public Function alive() As ternary Implements async_getter(Of T).alive
        Return state
    End Function

    Public Function [get](ByRef r As T) As Boolean Implements async_getter(Of T).get
        If initialized() Then
            If initialization_succeeded() Then
                r = v
                Return True
            Else
                Return False
            End If
        Else
            start_initialization()
            Return False
        End If
    End Function

    Protected Overridable Function prepare(ByVal r As ref(Of T)) As event_comb
        assert(d IsNot Nothing)
        Return d(r)
    End Function

    Public Function [get](ByVal r As ref(Of T)) As event_comb Implements async_getter(Of T).get
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not initialized() Then
                                      If begin_get.mark_in_use() Then
                                          assert(v Is Nothing OrElse type_info(Of T).is_valuetype)
                                          r.renew()
                                          ec = d(r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return waitfor(finish_get) AndAlso
                                                 goto_next()
                                      End If
                                  Else
                                      Return eva(r, v) AndAlso
                                             goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec Is Nothing Then
                                      assert(initialized)
                                      Return eva(r, v) AndAlso
                                             goto_end()
                                  Else
                                      If ec.end_result() Then
                                          v = (+r)
                                      End If
                                      state = ec.end_result()
                                      finish_get.mark()
                                      assert(inited.Set())
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End If
                              End Function)
    End Function

    Protected Overrides Sub Finalize()
        inited.Close()
        MyBase.Finalize()
    End Sub
End Class
