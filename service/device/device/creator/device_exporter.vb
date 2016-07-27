
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports osi.service.selector

Public Class device_exporter(Of T)
    Implements idevice_exporter(Of T)

    Public Event new_device_exported(ByVal d As idevice(Of T), ByRef export_result As Boolean) _
                                    Implements idevice_exporter(Of T).new_device_exported
    Public Event after_start() Implements idevice_exporter(Of T).after_start
    Public Event after_stop() Implements idevice_exporter(Of T).after_stop
    Private ReadOnly exped As atomic_int
    Private ReadOnly EXPORTED_COUNT_COUNTER As Int64

    Shared Sub New()
        assert(Not GetType(T).is(GetType(async_getter(Of ))))
    End Sub

    Protected Sub New()
        Me.New(Nothing)
    End Sub

    Protected Sub New(ByVal id As String)
        If String.IsNullOrEmpty(id) Then
            id = type_info(Of T).name
        End If
        exped = New atomic_int()
        EXPORTED_COUNT_COUNTER = counter.register_average_and_last_average(
                                     strcat(strtoupper(unref(id)), "_DEVICE_INITED_COUNT"),
                                     last_average_length:=256,
                                     sample_rate:=1)
    End Sub

    Protected Function device_exported(ByVal d As idevice(Of T)) As Boolean
        assert(Not d Is Nothing)
        counter.increase(EXPORTED_COUNT_COUNTER, exped.increment())
        Dim export_result As Boolean = False
        RaiseEvent new_device_exported(d, export_result)
        If export_result Then
            Return True
        Else
            d.close()
            Return False
        End If
    End Function

    Public Function exported() As UInt32 Implements idevice_exporter(Of T).exported
        Return +exped
    End Function

    Public Overridable Function start() As Boolean Implements idevice_exporter(Of T).start
        RaiseEvent after_start()
        Return True
    End Function

    Public Overridable Function [stop]() As Boolean Implements idevice_exporter(Of T).stop
        RaiseEvent after_stop()
        Return True
    End Function

    Public Overridable Function started() As Boolean Implements idevice_exporter(Of T).started
        Return True
    End Function

    Public Overridable Function stopped() As Boolean Implements idevice_exporter(Of T).stopped
        Return True
    End Function
End Class
