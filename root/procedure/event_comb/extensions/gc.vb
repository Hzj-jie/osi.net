
#If DEAD_LOCK Then '??
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils

Public Module _gc
    Private ReadOnly ec As atom(Of event_comb)

    Sub New()
        ec = New atom(Of event_comb)()
    End Sub

    Public Function start_gc_trigger(ByVal interval_ms As Int64) As Boolean
        If interval_ms < 0 Then
            interval_ms = 10 * second_milli
        End If
        If ec.compare_exchange(New event_comb(Function() As Boolean
                                                  Return waitfor(interval_ms) AndAlso
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  waitfor_gc_collect()
                                                  Return goto_prev()
                                              End Function),
                               Nothing) Is Nothing Then
            assert_begin(+ec)
            application_lifetime.stopping_handle(AddressOf stop_gc_trigger)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function start_gc_trigger() As Boolean
        Return start_gc_trigger(npos)
    End Function

    Public Function stop_gc_trigger() As Boolean
        Dim e As event_comb = Nothing
        e = (+ec)
        If Not e Is Nothing Then
            e.cancel()
            ec.set(Nothing)
            Return True
        Else
            Return False
        End If
    End Function
End Module
#End If
