
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports List = System.Collections.Generic.List(Of System.Net.Sockets.Socket)

Public NotInheritable Class poll_read_select
    Inherits event_comb_select(Of pair(Of TcpClient, Int64))

    Private Shared ReadOnly instance As poll_read_select = New poll_read_select()
    Private last_poll_ms As Int64

    Private Sub New()
        MyBase.New(max(timeslice_length_ms \ 2, 1))
    End Sub

    Public Shared Shadows Function queue(ByVal i As TcpClient,
                                         Optional ByVal timeout_ms As Int64 = npos) As event_comb
        assert(i IsNot Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If timeout_ms = 0 Then
                                      Return goto_end()
                                  Else
                                      If timeout_ms < 0 OrElse
                                         CDec(timeout_ms) + nowadays.milliseconds() > max_int64 Then
                                          timeout_ms = max_int64
                                      Else
                                          timeout_ms = nowadays.milliseconds() + timeout_ms
                                      End If
                                      ec = instance.queue_proxy(pair.emplace_of(i, timeout_ms))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function queue_proxy(ByVal i As pair(Of TcpClient, Int64)) As event_comb
        Return MyBase.queue(i)
    End Function

    Protected Overrides Function [select](ByVal i As pair(Of TcpClient, Int64)) As Boolean
        assert(i IsNot Nothing)
        Return Not free_poll_alive(i.first)
    End Function

    Protected Overrides Function [select]() As event_comb
        Return sync_async(Sub()
                              Dim wc As Int64 = constants.interval_ms.connection_check_interval
                              Dim mc As Int64 = 0
                              mc = nowadays.milliseconds()
                              If mc - last_poll_ms >= wc Then
                                  sync_select()
                                  last_poll_ms = mc
                              Else
                                  Dim v As New List()
                                  Dim m As New map(Of Int64, pair(Of pair(Of TcpClient, Int64), Action))()
                                  foreach(Function(p As pair(Of pair(Of TcpClient, Int64), Action)) As Boolean
                                              assert(p.first IsNot Nothing)
                                              assert(p.first.first IsNot Nothing)
                                              If nowadays.milliseconds() < p.first.second Then
                                                  v.Add(p.first.first.Client())
#If DEBUG Then
                                                  assert(m.find(p.first.first.handle_id()) = m.end())
#End If
                                                  m.emplace(p.first.first.handle_id(), p)
                                                  Return False
                                              Else
                                                  Return True
                                              End If
                                          End Function)
                                  If v.Count() > 0 Then
                                      'the following logic will throw null reference exception
                                      'if the TcpClient has been closed already.
                                      Socket.Select(v, Nothing, Nothing, 0)
                                      If v.Count() > 0 Then
                                          For Each i As Socket In v
                                              [select](m(i.handle_id()))
                                          Next
                                      End If
                                  End If
                              End If
                          End Sub)
    End Function
End Class
