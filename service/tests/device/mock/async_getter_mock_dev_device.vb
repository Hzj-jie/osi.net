
Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.event
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.selector

Public Class async_getter_mock_dev_device(Of PROTECTOR)
    Inherits async_getter_delegate_device(Of mock_dev(Of PROTECTOR))

    ' I do not want this instance to be initialized automatically to break the assumption in async_getter_device_test.
    ' And all the implementations related to async_getter in device should not expect the async_getter is implemented
    ' by async_preparer / async_thread_unsafe_lazier / async_thread_safe_lazier.
    Private Sub New(ByVal f As Func(Of ref(Of mock_dev(Of PROTECTOR)), event_comb))
        MyBase.New(async_thread_unsafe_lazier.[New](f),
                   AddressOf mock_dev(Of PROTECTOR).validate,
                   AddressOf mock_dev(Of PROTECTOR).close,
                   AddressOf mock_dev(Of PROTECTOR).identity,
                   AddressOf mock_dev(Of PROTECTOR).check)
    End Sub

    Public Sub New()
        Me.New(New mock_dev(Of PROTECTOR)())
    End Sub

    Public Sub New(ByVal v As mock_dev(Of PROTECTOR))
        Me.New(Function(p As ref(Of mock_dev(Of PROTECTOR))) As event_comb
                   Return New event_comb(Function() As Boolean
                                             Return If(rnd_bool(), waitfor(rnd_int64(0, 100)), True) AndAlso
                                                    goto_next()
                                         End Function,
                                         Function() As Boolean
                                             Return eva(p, v) AndAlso
                                                    goto_end()
                                         End Function)
               End Function)
    End Sub

    Public Sub New(ByVal v As mock_dev(Of PROTECTOR), ByVal trigger As ManualResetEvent)
        Me.New(Function(p As ref(Of mock_dev(Of PROTECTOR))) As event_comb
                   Return New event_comb(Function() As Boolean
                                             Return waitfor(trigger) AndAlso
                                                    goto_next()
                                         End Function,
                                         Function() As Boolean
                                             Return eva(p, v) AndAlso
                                                    goto_end()
                                         End Function)
               End Function)
    End Sub

    Public Sub New(ByVal trigger As ManualResetEvent)
        Me.New(New mock_dev(Of PROTECTOR)(), trigger)
    End Sub

    Public Sub New(ByVal v As mock_dev(Of PROTECTOR), ByVal trigger As signal_event)
        Me.New(Function(p As ref(Of mock_dev(Of PROTECTOR))) As event_comb
                   Return New event_comb(Function() As Boolean
                                             Return waitfor(trigger) AndAlso
                                                    goto_next()
                                         End Function,
                                         Function() As Boolean
                                             Return eva(p, v) AndAlso
                                                    goto_end()
                                         End Function)
               End Function)
    End Sub

    Public Sub New(ByVal trigger As signal_event)
        Me.New(New mock_dev(Of PROTECTOR)(), trigger)
    End Sub
End Class
