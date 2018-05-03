
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.utils

#Const POLL_READ_SELECT = False

Public Module _extension
    Public ReadOnly donot_poll_status As Boolean
    Private ReadOnly poll_status As Boolean
    Private ReadOnly empty_buff() As Byte
    Private ReadOnly empty_buff_size As Int32
    Private ReadOnly act As invoker(Of Func(Of Boolean))

    Sub New()
        ReDim empty_buff(0)
        empty_buff_size = array_size_i(empty_buff)
        assert(npos < 0)
        donot_poll_status = env_bool(env_keys("do", "not", "poll", "status")) OrElse
                            env_bool(env_keys("donot", "poll", "status"))
        poll_status = Not donot_poll_status
        assert(invoker.of(act).
                   with_type(GetType(TcpListener)).
                   with_binding_flags(binding_flags.instance_private_method).
                   with_name("get_Active").
                   build(act))
        assert(act.valid())
        assert(act.post_binding())
    End Sub

    Public Function use_socket_send() As Boolean
        Return constants.use_socket
    End Function

    Public Function use_socket_receive() As Boolean
        Return constants.use_socket
    End Function

    <Extension()> Public Function poll_write(ByVal c As TcpClient) As Boolean
        Return c.alive() AndAlso
               c.Client().poll_write()
    End Function

    <Extension()> Public Sub shutdown(ByVal client As TcpClient)
        On Error Resume Next
        If Not client Is Nothing Then
            client.GetStream().Close()
            client.GetStream().Dispose()
            client.Client().Shutdown(SocketShutdown.Both)
            'client.Client().SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.DontLinger, False)
            'client.Client().Close(0)
            client.Client().Close()
            client.Close()
        End If
    End Sub

    <Extension()> Public Function send_buff_size(ByVal c As TcpClient) As UInt32
        If c Is Nothing Then
            Return failure_send_buff_size
        Else
            Return c.Client().send_buff_size()
        End If
    End Function

    <Extension()> Public Function receive_buff_size(ByVal c As TcpClient) As UInt32
        If c Is Nothing Then
            Return failure_receive_buff_size
        Else
            Return c.Client().receive_buff_size()
        End If
    End Function

    <Extension()> Public Function no_delay(ByVal c As TcpClient) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return c.Client().no_delay()
        End If
    End Function

    <Extension()> Public Function set_no_delay(ByVal c As TcpClient, ByVal v As Boolean) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return c.Client().set_no_delay(v)
        End If
    End Function

    <Extension()> Public Function free_poll_alive(ByVal client As TcpClient,
                                                  Optional ByVal p As powerpoint = Nothing) As Boolean
        If poll_alive(client) Then
            Return True
        Else
            If tcp_trace Then
                raise_error("shutdown a connection ",
                            client.identity(),
                            If(p Is Nothing,
                               Nothing,
                               strcat(" on powerpoint ", p.identity)),
                            " because of connectivity")
            End If
            client.shutdown()
            Return False
        End If
    End Function

#If 0 Then
    Private Function acceptable_socket_exception(ByVal ex As SocketException) As Boolean
        Return assert(Not ex Is Nothing) AndAlso
               ex.SocketErrorCode() = SocketError.WouldBlock
        Return assert(Not ex Is Nothing) AndAlso
               (ex.SocketErrorCode() = SocketError.WouldBlock OrElse
                ex.SocketErrorCode() = SocketError.TimedOut)
    End Function
#End If

    <Extension()> Public Function poll_alive(ByVal client As TcpClient) As Boolean
        Try
            If Not client.alive() Then
                If tcp_trace Then
                    raise_error(error_type.warning,
                                "client ",
                                client.identity(),
                                " is not connected, treat as dead")
                End If
                Return False
            Else
                If client.Client().poll_error() Then
                    If tcp_trace Then
                        raise_error(error_type.warning,
                                    "polled error from client ",
                                    client.identity(),
                                    ", treat as dead")
                    End If
                    Return False
                End If
                If poll_status Then
                    If client.Client().poll_read() Then
                        Dim r As Boolean = False
                        Try
                            Dim rst As Int32 = 0
                            Dim buffered_bytes As Int32 = 0
                            buffered_bytes = client.Available()
                            rst = client.Client().Receive(empty_buff, empty_buff_size, SocketFlags.Peek)
                            If buffered_bytes = 0 Then
                                'the data is coming right after client.Available() called
                                r = (rst = 0 OrElse rst = empty_buff_size)
                            Else
                                r = (rst = empty_buff_size)
                            End If
                        Catch ex As SocketException
                            If tcp_trace Then
                                raise_error(error_type.warning,
                                            "caught socket exception on client ",
                                            client.identity(),
                                            ", socket error code ",
                                            ex.SocketErrorCode(),
                                            ", native error code ",
                                            ex.NativeErrorCode(),
                                            ", ex ",
                                            ex.Message())
                            End If
                            r = False
                        End Try
                        If Not r OrElse Not client.alive() Then
                            If tcp_trace AndAlso Not r Then
                                raise_error(error_type.warning,
                                            "polled read, but cannot receive from client ",
                                            client.identity(),
                                            ", treat as dead")
                            End If
                            Return False
                        End If
                    End If
                    If client.Client().poll_write() Then
                        Dim r As Boolean = False
                        Dim ob As Boolean = False
                        Dim c As Socket = Nothing
                        c = client.Client()
                        ob = c.Blocking()
                        Try
                            c.Blocking() = False
                            r = (c.Send(empty_buff, 0, SocketFlags.None) = 0)
                        Catch ex As SocketException
                            If tcp_trace Then
                                raise_error(error_type.warning,
                                            "caught socket exception on client ",
                                            client.identity(),
                                            ", socket error code ",
                                            ex.SocketErrorCode(),
                                            ", native error code ",
                                            ex.NativeErrorCode(),
                                            ", ex ",
                                            ex.Message())
                            End If
                            r = False
                        Finally
                            c.Blocking() = ob
                        End Try
                        If Not r OrElse Not client.alive() Then
                            If tcp_trace Then
                                raise_error(error_type.warning,
                                            "failed to send unblock data to the client ",
                                            client.identity(),
                                            ", treat as dead")
                            End If
                            Return False
                        End If
                    End If
                End If
                If client.alive() Then
                    Dim state As TcpState = Nothing
                    state = connection_state.query(client.Client())
                    If state = TcpState.Closed OrElse
                       state = TcpState.CloseWait OrElse
                       state = TcpState.DeleteTcb Then
                        If tcp_trace Then
                            raise_error(error_type.warning,
                                        "the connection state of client ",
                                        client.identity(),
                                        " is ",
                                        state,
                                        ", ",
                                        "treat as dead")
                        End If
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
                'Return client.GetStream().can_read() AndAlso
                '       client.GetStream().can_write() AndAlso
                '       client.GetStream().data_available() <> ternary.unknown
                'Return client.Client().can_send() AndAlso
                '       client.Client().can_receive() AndAlso
                '       client.Client().data_buffered() <> ternary.unknown
            End If
            'Return operate(client, maxInt64, Function(x) x.can_write())
        Catch ex As Exception
            raise_error(error_type.warning,
                        "caught unhandled exception when poll client ",
                        client.identity(),
                        " status, treat as dead, ex ",
                        ex.Message,
                        ", callstack ",
                        ex.StackTrace())
            Return False
        End Try
    End Function

    <Extension()> Public Function alive(ByVal client As TcpClient) As Boolean
        If client Is Nothing Then
            Return False
        Else
            Try
                Return client.Connected()
            Catch
                Return False
            End Try
        End If
    End Function

    '=0 no data to read
    '<0 client is not connected
    '>0 has data to read
    <Extension()> Public Function buffered_bytes(ByVal client As TcpClient) As Int32
        ' Since Socket.buffered_bytes() does not care about broken connection, we should check whether the client is
        ' alive before calling Socket.buffered_bytes().
        If alive(client) Then
            Return client.Client().buffered_bytes()
        Else
            Return npos
        End If
    End Function

    <Extension()> Public Function identity(ByVal c As TcpClient) As String
        If c Is Nothing Then
            Return "null-tcp-client"
        Else
            Return c.Client().identity()
        End If
    End Function

    <Extension()> Public Function handle_id(ByVal this As TcpClient) As Int64
        If this Is Nothing Then
            Return npos
        Else
            Return this.Client().handle_id()
        End If
    End Function

    <Extension()> Public Function stream(ByVal this As TcpClient) As Stream
        If this Is Nothing Then
            Return Nothing
        Else
            Try
                Return this.GetStream()
            Catch
                Return Nothing
            End Try
        End If
    End Function

    <Extension()> Public Function set_keepalive(ByVal c As TcpClient,
                                                Optional ByVal enable As Boolean = enable_socket_keepalive,
                                                Optional ByVal first_keepalive_ms As UInt32 = socket_first_keepalive_ms,
                                                Optional ByVal interval_ms As UInt32 = socket_keepalive_interval_ms) _
                                               As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return c.Client().set_keepalive(enable, first_keepalive_ms, interval_ms)
        End If
    End Function

    <Extension()> Public Function enable_keepalive(ByVal c As TcpClient,
                                                   Optional ByVal first_keepalive_ms As UInt32 = _
                                                       socket_first_keepalive_ms,
                                                   Optional ByVal interval_ms As UInt32 = _
                                                       socket_keepalive_interval_ms) _
                                                  As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return c.Client().enable_keepalive(first_keepalive_ms, interval_ms)
        End If
    End Function

    <Extension()> Public Function active(ByVal i As TcpListener) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return act(i)()
        End If
    End Function
End Module
