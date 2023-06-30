
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.constants
Imports osi.service.streamer

Public Class iosys(Of T As Class)
    Private ReadOnly f As flower(Of T)
    Private ReadOnly agents As agents(Of T)
    Private ReadOnly receivers As receivers(Of T)

    Public Sub New(ByVal max_size As UInt32)
        agents = New agents(Of T)(New pipe(Of T)(max_size, uint32_0, True))
        receivers = New receivers(Of T)()
        f = New direct_flower(Of T)(agents, receivers)
        assert_begin(+f)
    End Sub

    Public Function [stop]() As Boolean
        Return f.stop()
    End Function

    Public Function listen(ByVal a As iagent(Of T)) As Boolean
        Return agents.listen(a)
    End Function

    Public Function ignore(ByVal a As iagent(Of T)) As Boolean
        Return agents.ignore(a)
    End Function

    Public Function notice(ByVal r As ireceiver(Of T)) As Boolean
        Return receivers.notice(r)
    End Function

    Public Function ignore(ByVal r As ireceiver(Of T)) As Boolean
        Return receivers.ignore(r)
    End Function
End Class
