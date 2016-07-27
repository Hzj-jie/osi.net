
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.streamer
Imports osi.service.device
Imports utils = osi.root.utils

Public Class agents(Of T)
    Inherits pipe_dev(Of T)

    Private ReadOnly s As object_unique_set(Of iagent(Of T))

    Public Sub New(ByVal pipe As pipe(Of T))
        MyBase.New(pipe)
        s = New object_unique_set(Of iagent(Of T))()
    End Sub

    Private Sub agent_handler(ByVal c As T, ByRef receive_failed As Boolean)
        receive_failed = Not sync_send(c)
    End Sub

    Public Function listen(ByVal a As iagent(Of T)) As Boolean
        If s.insert(a) Then
            AddHandler a.deliver, AddressOf agent_handler
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ignore(ByVal a As iagent(Of T)) As Boolean
        If s.erase(a) Then
            RemoveHandler a.deliver, AddressOf agent_handler
            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class receivers(Of T As Class)
    Implements T_sender(Of T)

    Private ReadOnly s As object_unique_set(Of ireceiver(Of T))

    Public Sub New()
        s = New object_unique_set(Of ireceiver(Of T))()
    End Sub

    Public Function notice(ByVal r As ireceiver(Of T)) As Boolean
        Return s.insert(r)
    End Function

    Public Function ignore(ByVal r As ireceiver(Of T)) As Boolean
        Return s.erase(r)
    End Function

    Public Function send(ByVal c As T) As event_comb Implements T_sender(Of T).send
        Dim ecs As vector(Of event_comb) = Nothing
        Return New event_comb(Function() As Boolean
                                  ecs = New vector(Of event_comb)()
                                  Return utils.foreach(AddressOf s.foreach,
                                                       Sub(ByRef r As ireceiver(Of T))
                                                           Dim ec As event_comb = Nothing
                                                           ec = r.receive(c)
                                                           If Not ec Is Nothing Then
                                                               ecs.emplace_back(ec)
                                                           End If
                                                       End Sub) AndAlso
                                         If(Not ecs.empty(),
                                            waitfor(+ecs) AndAlso goto_next(),
                                            goto_end())
                              End Function,
                              Function() As Boolean
                                  If Not (+ecs).any_end_result() Then
                                      raise_error(error_type.warning,
                                                  "failed to deliver iosys case ",
                                                  c)
                                  End If
                                  'return false will stop flow procedure, which is not expected
                                  Return goto_end()
                              End Function)
    End Function
End Class
