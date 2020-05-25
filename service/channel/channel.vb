
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.service.commander
Imports osi.service.transmitter

Partial Public Class channel
    'Inherits pre_generated_device_pool(Of block)

    Public ReadOnly lifetime As signal_event
    Private ReadOnly h As herald
    Private ReadOnly m() As channel_block
    Private ReadOnly pending_io As pending_io_punishment
    Private ReadOnly send_event As signal_event
    Private ReadOnly send_queue As slimqless2(Of command)

    Shared Sub New()
        bytes_serializer(Of constants.remote.action).forward_registration.from(Of SByte)()
        bytes_serializer(Of constants.remote.parameter).forward_registration.from(Of SByte)()
    End Sub

    Public Sub New(ByVal h As herald,
                   ByVal stopping As pointer(Of singleentry),
                   Optional ByVal idle_timeout_ms As Int64 = npos,
                   Optional ByVal max_channel_count As UInt32 = constants.default_max_channel_count)
        Me.New(h, AddressOf stopping.not_null_and_in_use, idle_timeout_ms, max_channel_count)
    End Sub

    Public Sub New(ByVal h As herald,
                   Optional ByVal stopping As Func(Of Boolean) = Nothing,
                   Optional ByVal idle_timeout_ms As Int64 = npos,
                   Optional ByVal max_channel_count As UInt32 = constants.default_max_channel_count)
        assert(Not h Is Nothing)
        assert(max_channel_count > 0)
        Me.h = h
        ReDim Me.m(CInt(max_channel_count - uint32_1))
        Me.pending_io = New pending_io_punishment()
        Me.send_event = New signal_event()
        Me.send_queue = New slimqless2(Of command)()
        Me.lifetime = New signal_event()
        send()
        receive()
        kick_idle_blocks(idle_timeout_ms)
    End Sub

    Private Sub send()
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        ec = Nothing
                                        Return waitfor(send_event) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        If lifetime.marked() Then
                                            Return False
                                        Else
                                            If ec.end_result_or_null() Then
                                                Dim c As command = Nothing
                                                If send_queue.pop(c) Then
                                                    ec = h.send(c)
                                                    Return waitfor(ec)
                                                Else
                                                    send_event.unmark()
                                                    Return goto_begin()
                                                End If
                                            Else
                                                lifetime.mark()
                                                Return False
                                            End If
                                        End If
                                    End Function))
    End Sub

    Private Sub receive()
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of command) = Nothing
        Dim o As command = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        If lifetime.marked() Then
                                            p.renew()
                                            ec = h.receive(p)
                                            Return waitfor(ec) AndAlso
                                                   goto_next()
                                        Else
                                            Return False
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        If ec.end_result() Then
                                            If Not (+p) Is Nothing Then
                                                Dim remote_id As UInt32 = 0
                                                If (+p).parameter(constants.remote.parameter.local_id, remote_id) Then
                                                    If (+p).action_is(constants.remote.action.connect) Then
                                                        o.renew()
                                                        ec = execute_connect(+p, o)
                                                    ElseIf (+p).action_is(constants.remote.action.send) Then
                                                        o.renew()
                                                        ec = execute_send(+p, o)
                                                    ElseIf (+p).action_is(constants.remote.action.close) Then
                                                        o.renew()
                                                        ec = execute_close(+p, o)
                                                    End If
                                                End If
                                            End If
                                            Return goto_begin()
                                        Else
                                            lifetime.mark()
                                            Return False
                                        End If
                                    End Function))
    End Sub

    Private Sub send(ByVal c As command)
        assert(Not c Is Nothing)
        send_queue.emplace(c)
        send_event.mark()
    End Sub

    Private Sub kick_idle_blocks(ByVal idle_timeout_ms As Int64)
        If idle_timeout_ms >= 0 Then
            assert_begin(New event_comb(Function() As Boolean
                                            Return waitfor(idle_timeout_ms) AndAlso
                                                   goto_next()
                                        End Function,
                                        Function() As Boolean
                                            Dim n As Int64 = 0
                                            n = nowadays.milliseconds()
                                            For i As UInt32 = 0 To array_size(m) - uint32_1
                                                If Not m(CInt(i)) Is Nothing AndAlso
                                                   n - m(CInt(i)).last_active_ms() >= idle_timeout_ms Then
                                                    ' TODO
                                                End If
                                            Next
                                        End Function))
        End If
    End Sub

    Private Function execute_connect(ByVal i As command, ByVal o As command) As event_comb

    End Function

    Private Function execute_close(ByVal i As command, ByVal o As command) As event_comb

    End Function

    Private Function execute_send(ByVal i As command, ByVal o As command) As event_comb

    End Function
End Class
