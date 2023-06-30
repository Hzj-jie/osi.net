
Imports osi.root.connector

Public NotInheritable Class device_exporter_adapter
    Public Shared Function [New](Of IT, OT)(ByVal i As idevice_exporter(Of IT),
                                            ByVal c As Func(Of IT, OT)) As device_exporter_adapter(Of IT, OT)
        Return New device_exporter_adapter(Of IT, OT)(i, c)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class device_exporter_adapter(Of IT, OT)
    Implements idevice_exporter(Of OT)

    Public Event after_start() Implements idevice_exporter(Of OT).after_start
    Public Event after_stop() Implements idevice_exporter(Of OT).after_stop
    Public Event new_device_exported(ByVal d As idevice(Of OT), ByRef export_result As Boolean) _
                                    Implements idevice_exporter(Of OT).new_device_exported

    Private ReadOnly i As idevice_exporter(Of IT)
    Private ReadOnly c As Func(Of IT, OT)

    Public Sub New(ByVal i As idevice_exporter(Of IT), ByVal c As Func(Of IT, OT))
        assert(Not i Is Nothing)
        assert(Not c Is Nothing)
        Me.i = i
        Me.c = c
        AddHandler i.after_start, Sub()
                                      RaiseEvent after_start()
                                  End Sub
        AddHandler i.after_stop, Sub()
                                     RaiseEvent after_stop()
                                 End Sub
        AddHandler i.new_device_exported, Sub(d As idevice(Of IT), ByRef export_result As Boolean)
                                              ' VS 2010
                                              RaiseEvent new_device_exported(device_adapter.[New](Of IT, OT)(d, c),
                                                                             export_result)
                                          End Sub
    End Sub

    Public Function exported() As UInt32 Implements idevice_exporter(Of OT).exported
        Return i.exported()
    End Function

    Public Function start() As Boolean Implements idevice_exporter(Of OT).start
        Return i.start()
    End Function

    Public Function started() As Boolean Implements idevice_exporter(Of OT).started
        Return i.started()
    End Function

    Public Function [stop]() As Boolean Implements idevice_exporter(Of OT).stop
        Return i.stop()
    End Function

    Public Function stopped() As Boolean Implements idevice_exporter(Of OT).stopped
        Return i.stopped()
    End Function
End Class
