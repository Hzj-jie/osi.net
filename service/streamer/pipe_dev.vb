
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.device

Public Class pipe_dev(Of T)
    Implements dev_T(Of T)

    Private Class pipe_indicator
        Inherits never_fail_sync_indicator

        Public ReadOnly pipe As pipe(Of T)

        Public Sub New(ByVal pipe As pipe(Of T))
            assert(Not pipe Is Nothing)
            Me.pipe = pipe
        End Sub

        Protected Overrides Function indicate() As Boolean
            Return Not pipe.empty()
        End Function
    End Class

    Public ReadOnly pipe As pipe(Of T)
    Private ReadOnly sensor As sensor

    Public Sub New(ByVal pipe As pipe(Of T))
        assert(Not pipe Is Nothing)
        Me.pipe = pipe
        Me.sensor = New indicator_sensor_adapter(New pipe_indicator(pipe))
    End Sub

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements dev_T(Of T).sense
        Return sensor.sense(pending, timeout_ms)
    End Function

    Public Function send(ByVal i As T) As event_comb Implements dev_T(Of T).send
        Return pipe.push(i)
    End Function

    Public Function receive(ByVal o As pointer(Of T)) As event_comb Implements dev_T(Of T).receive
        Return pipe.pop(o)
    End Function

    Public Function sync_send(ByVal i As T) As Boolean
        Return pipe.sync_push(i)
    End Function

    Public Function sync_receive(ByRef o As T) As Boolean
        Return pipe.sync_pop(o)
    End Function
End Class
