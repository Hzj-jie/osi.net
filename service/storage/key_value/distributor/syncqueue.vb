
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.lock

Public NotInheritable Class syncqueue
    Private Const sync_wait_ms As Int64 = second_milli
    Private Const push_wait_ms As Int64 = second_milli
    Private ReadOnly prepare_sync_queue As slimqless2(Of String)
    Private ReadOnly sync_queue As unique_queue(Of String)
    Private ReadOnly sync_queue_lock As ref(Of event_comb_lock)
    Private ReadOnly impl As iredundance_distributor

    Public Sub New(ByVal impl As iredundance_distributor)
        Me.impl = impl
        assert(Not Me.impl Is Nothing)
        prepare_sync_queue = New slimqless2(Of String)()
        sync_queue = New unique_queue(Of String)()
        sync_queue_lock = New ref(Of event_comb_lock)()
        start_push_to_sync_queue()
        start_sync()
    End Sub

    Private Function sync(ByVal key As String) As event_comb
        Dim ec As event_comb = Nothing
        Dim result As ref(Of Byte()) = Nothing
        Dim ts As ref(Of Int64) = Nothing
        Dim nr As ref(Of Boolean()) = Nothing
        Return New event_comb(Function() As Boolean
                                  result = New ref(Of Byte())()
                                  ts = New ref(Of Int64)()
                                  nr = New ref(Of Boolean())()
                                  ec = impl.read(key, result, ts, nr)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+result) Is Nothing AndAlso
                                         (+ts) = npos Then
                                          'the key is not existing anymore
                                          Return goto_end()
                                      Else
                                          ec = impl.modify(key, +result, +ts, +nr)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Sub start_sync()
        Dim key As String = Nothing
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        If impl.expired().in_use() Then
                                            Return goto_end()
                                        Else
                                            Return waitfor(sync_queue_lock) AndAlso
                                                   goto_next()
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        If sync_queue.empty() Then
                                            sync_queue_lock.release()
                                            Return waitfor(sync_wait_ms) AndAlso
                                                   goto_begin()
                                        Else
                                            key = sync_queue.front()
                                            sync_queue.pop()
                                            sync_queue_lock.release()
                                            Return impl.wrappered().lock(key) AndAlso
                                                   goto_next()
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        ec = sync(key)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        impl.wrappered().release(key)
                                        If Not ec.end_result() Then
                                            push_to_prepare_sync_queue(key)
                                        End If
                                        Return goto_begin()
                                    End Function))
    End Sub

    Private Sub start_push_to_sync_queue()
        Dim key As String = Nothing
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        If impl.expired().in_use() Then
                                            Return goto_end()
                                        ElseIf prepare_sync_queue.pop(key) Then
                                            Return waitfor(sync_queue_lock) AndAlso
                                                   goto_next()
                                        Else
                                            Return waitfor(push_wait_ms)
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        sync_queue.push(key)
                                        sync_queue_lock.release()
                                        Return goto_begin()
                                    End Function))
    End Sub

    Public Sub push_to_prepare_sync_queue(ByVal key As String)
        prepare_sync_queue.push(key)
    End Sub

    Public Function clear() As event_comb
        Return New event_comb(Function() As Boolean
                                  prepare_sync_queue.clear()
                                  Return waitfor(sync_queue_lock) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  sync_queue.clear()
                                  sync_queue_lock.release()
                                  Return goto_end()
                              End Function)
    End Function
End Class
