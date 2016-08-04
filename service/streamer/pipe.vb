
Imports osi.root.template
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.device

Public Class pipe(Of T)
    Private ReadOnly max_size As UInt32
    Private ReadOnly retries As Int32
    Private ReadOnly q As qless2(Of T)
    Private ReadOnly input_pending As pending_io_punishment(Of _true)
    Private ReadOnly output_pending As pending_io_punishment(Of _true)

    Public Sub New(ByVal max_size As UInt32,
                   ByVal retries As Int32,
                   ByVal enable_io_pending_punishment As Boolean)
        Me.max_size = max_size
        If retries = max_int32 Then
            retries -= 1
        End If
        Me.retries = retries
        q = New qless2(Of T)()
        If enable_io_pending_punishment Then
            _new(input_pending)
            _new(output_pending)
        End If
    End Sub

    Public Sub New(ByVal pipe As pipe(Of T))
        Me.New(assert_return(Not pipe Is Nothing, pipe.max_size),
               assert_return(Not pipe Is Nothing, pipe.retries),
               assert_return(Not pipe Is Nothing, pipe.enable_io_pending_punishment()))
    End Sub

    Public Sub New()
        Me.New(uint32_0, npos, True)
    End Sub

    Public Function limited_size() As Boolean
        Return max_size > uint32_0
    End Function

    Public Function limited_retries() As Boolean
        Return retries >= 0
    End Function

    Public Function enable_io_pending_punishment() As Boolean
        Return Not input_pending Is Nothing AndAlso
               assert(Not output_pending Is Nothing)
    End Function

    Public Sub clear()
        q.clear()
    End Sub

    Public Function size() As UInt32
        Return q.size()
    End Function

    Public Function empty() As Boolean
        Return q.empty()
    End Function

    Private Function unlimited(ByVal v As Func(Of Boolean),
                               ByVal using_pending As pending_io_punishment(Of _true)) As event_comb
        assert(retries < 0)
        assert(Not v Is Nothing)
        Return New event_comb(Function() As Boolean
                                  If v() Then
                                      If enable_io_pending_punishment() Then
                                          assert(Not using_pending Is Nothing)
                                          using_pending.reset()
                                      End If
                                      Return goto_end()
                                  Else
                                      If enable_io_pending_punishment() Then
                                          assert(Not using_pending Is Nothing)
                                          Dim pending_time_ms As Int64 = 0
                                          assert(using_pending.record(False, pending_time_ms))
                                          Return waitfor(pending_time_ms)
                                      Else
                                          Return True
                                      End If
                                  End If
                              End Function)
    End Function

    Private Function limited(ByVal v As Func(Of Boolean),
                             ByVal using_pending As pending_io_punishment(Of _true)) As event_comb
        assert(retries >= 0)
        assert(Not v Is Nothing)
        Dim c As Int32 = 0
        Return New event_comb(Function() As Boolean
                                  c = retries + 1
                                  assert(c > 0)
                                  Return goto_next()
                              End Function,
                              Function() As Boolean
                                  c -= 1
                                  If v() Then
                                      If enable_io_pending_punishment() Then
                                          assert(Not using_pending Is Nothing)
                                          using_pending.reset()
                                      End If
                                      Return goto_end()
                                  Else
                                      If c = 0 Then
                                          Return False
                                      ElseIf enable_io_pending_punishment() Then
                                          assert(Not using_pending Is Nothing)
                                          Dim pending_time_ms As Int64 = 0
                                          assert(using_pending.record(False, pending_time_ms))
                                          Return waitfor(pending_time_ms)
                                      Else
                                          Return True
                                      End If
                                  End If
                              End Function)
    End Function

    Public Function push(ByVal c As T) As event_comb
        Return emplace(copy_no_error(c))
    End Function

    Public Function sync_push(ByVal c As T) As Boolean
        Return sync_emplace(copy_no_error(c))
    End Function

    Public Function sync_emplace(ByVal c As T) As Boolean
        If limited_size() Then
            Return q.emplace(c, max_size)
        Else
            q.emplace(c)
            Return True
        End If
    End Function

    Private Function unlimited_emplace(ByVal c As T) As event_comb
        assert(max_size > 0)
        Return unlimited(Function() sync_emplace(c), input_pending)
    End Function

    Private Function limited_emplace(ByVal c As T) As event_comb
        assert(max_size > 0)
        Return limited(Function() sync_emplace(c), input_pending)
    End Function

    Public Function emplace(ByVal c As T) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If limited_size() Then
                                      ec = If(limited_retries(), limited_emplace(c), unlimited_emplace(c))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      q.emplace(c)
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sync_pop(ByRef r As T) As Boolean
        Return q.pop(r)
    End Function

    Private Function sync_pop(ByVal r As pointer(Of T)) As Boolean
        Dim o As T = Nothing
        Return sync_pop(o) AndAlso
               eva(r, o)
    End Function

    Private Function unlimited_pop(ByVal r As pointer(Of T)) As event_comb
        Return unlimited(Function() sync_pop(r), output_pending)
    End Function

    Private Function limited_pop(ByVal r As pointer(Of T)) As event_comb
        Return limited(Function() sync_pop(r), output_pending)
    End Function

    Public Function pop(ByVal r As pointer(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = If(limited_retries(), limited_pop(r), unlimited_pop(r))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
