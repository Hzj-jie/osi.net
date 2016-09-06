
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

Public Module _socket
    Private ReadOnly true_() As Byte
    Private ReadOnly false_() As Byte

    Sub New()
        true_ = bool_bytes(True)
        false_ = bool_bytes(False)
    End Sub

    Public Function io_control_code_to_str(ByVal control_code As Int32) As String
        Dim c As IOControlCode = Nothing
        Dim s As String = Nothing
        If enum_cast(Of IOControlCode, Int32)(control_code, c) Then
            s = Convert.ToString(c)
        Else
            s = Convert.ToString(control_code)
        End If
        Return s
    End Function

    Public Function identity(ByVal local As IPEndPoint, ByVal remote As IPEndPoint) As String
        If local Is Nothing OrElse remote Is Nothing Then
            Return Nothing
        Else
            Return strcat(Convert.ToString(local),
                          character.minus_sign,
                          Convert.ToString(remote))
        End If
    End Function

    <Extension()> Public Function handle_id(ByVal s As Socket) As Int64
        If s Is Nothing Then
            Return npos
        Else
            Return s.Handle().ToInt64()
        End If
    End Function

    <Extension()> Public Function identity(ByVal s As Socket) As String
        If s Is Nothing Then
            Return "null-socket"
        Else
            Try
                Return strcat(s.SocketType(),
                              character.left_mid_bracket,
                              s.AddressFamily(),
                              character.colon,
                              s.ProtocolType(),
                              character.right_mid_bracket,
                              identity(s.LocalEndPoint(),
                                       s.RemoteEndPoint()),
                              character.at,
                              s.handle_id())
            Catch
                Return "broken-socket"
            End Try
        End If
    End Function

    Private Function poll(ByVal s As Socket, ByVal timeout_ms As Int64, ByVal mode As SelectMode) As Boolean
        If s Is Nothing Then
            Return False
        Else
            Try
                Return s.Poll(milliseconds_to_microseconds(timeout_ms),
                              mode)
            Catch ex As Exception
                If network_trace Then
                    raise_error(error_type.warning,
                                "failed to poll ",
                                If(mode = SelectMode.SelectError,
                                   "error",
                                   If(mode = SelectMode.SelectRead,
                                      "read",
                                      "write")),
                                " status of the socket, ex ",
                                ex.Message())
                End If
                Return mode = SelectMode.SelectError OrElse
                       mode = SelectMode.SelectRead
            End Try
        End If
    End Function

    <Extension()> Public Function poll_write(ByVal s As Socket, Optional ByVal timeout_ms As Int64 = 0) As Boolean
        Return poll(s, timeout_ms, SelectMode.SelectWrite)
    End Function

    <Extension()> Public Function poll_read(ByVal s As Socket, Optional ByVal timeout_ms As Int64 = 0) As Boolean
        Return poll(s, timeout_ms, SelectMode.SelectRead)
    End Function

    <Extension()> Public Function poll_error(ByVal s As Socket, Optional ByVal timeout_ms As Int64 = 0) As Boolean
        Return poll(s, timeout_ms, SelectMode.SelectError)
    End Function

    '=0 no data to read
    '<0 client is not connected
    '>0 has data to read
    <Extension()> Public Function buffered_bytes(ByVal s As Socket) As Int32
        ' For connectionless Socket, checking Connected is not properly.
        If s Is Nothing Then
            Return npos
        Else
            Try
                Return s.Available()
            Catch
                Return npos
            End Try
        End If
    End Function

    <Extension()> Public Function send_buff_size(ByVal s As Socket) As UInt32
        If s Is Nothing Then
            Return failure_send_buff_size
        Else
            Try
                Return CUInt(s.SendBufferSize())
            Catch ex As Exception
                If network_trace Then
                    raise_error(error_type.warning,
                                "failed to get socket send buffer size, ex ",
                                ex.Message())
                End If
                Return failure_send_buff_size
            End Try
        End If
    End Function

    <Extension()> Public Function receive_buff_size(ByVal s As Socket) As UInt32
        If s Is Nothing Then
            Return failure_receive_buff_size
        Else
            Try
                Return CUInt(s.ReceiveBufferSize())
            Catch ex As Exception
                If network_trace Then
                    raise_error(error_type.warning,
                                "failed to get socket receive buffer size, ex ",
                                ex.Message())
                End If
                Return failure_receive_buff_size
            End Try
        End If
    End Function

    <Extension()> Public Function no_delay(ByVal s As Socket) As Boolean
        If s Is Nothing Then
            Return False
        Else
            Try
                Return s.NoDelay()
            Catch ex As Exception
                If network_trace Then
                    raise_error(error_type.warning,
                                "failed to get no-delay setting of socket, ex ",
                                ex.Message())
                End If
                Return False
            End Try
        End If
    End Function

    <Extension()> Public Function set_no_delay(ByVal s As Socket, ByVal v As Boolean) As Boolean
        If s Is Nothing Then
            Return False
        Else
            Try
                s.NoDelay() = v
                Return True
            Catch ex As Exception
                If network_trace Then
                    raise_error(error_type.warning,
                                "failed to set no-delay setting of socket, ex ",
                                ex.Message())
                End If
                Return False
            End Try
        End If
    End Function

    Private Function set_iocontrol(ByVal this As Socket,
                                   ByVal control_code As Int64,
                                   ByVal control_code_str As Func(Of String),
                                   ByVal in_value() As Byte,
                                   ByVal out_value() As Byte) As Int32
        If this Is Nothing Then
            Return npos
        End If
        Dim r As Int32 = 0
        Try
            r = this.IOControl(CType(control_code, IOControlCode), in_value, out_value)
            assert(r >= 0)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to set io control [",
                        control_code_str(),
                        "] value on client ",
                        this.identity(),
                        ", ex ",
                        ex.Message())
            Return npos
        End Try
        If network_trace Then
            raise_error("return value of io control [",
                        control_code_str(),
                        "] value on client ",
                        this.identity(),
                        " is ",
                        r)
        End If
        Return r
    End Function

    <Extension()> Public Function set_iocontrol(ByVal this As Socket,
                                                ByVal control_code As Int64,
                                                ByVal in_value() As Byte,
                                                Optional ByVal out_value() As Byte = Nothing) As Int32
        Return set_iocontrol(this, control_code, Function() io_control_code_to_str(control_code), in_value, out_value)
    End Function

    <Extension()> Public Function set_iocontrol(ByVal this As Socket,
                                                ByVal control_code As IOControlCode,
                                                ByVal in_value() As Byte,
                                                Optional ByVal out_value() As Byte = Nothing) As Int32
        Return set_iocontrol(this, CLng(control_code), Function() control_code.ToString(), in_value, out_value)
    End Function

    <Extension()> Public Function set_iocontrol(ByVal this As Socket,
                                                ByVal control_code As Int64,
                                                ByVal in_value As Boolean,
                                                Optional ByVal out_value() As Byte = Nothing) As Int32
        Return set_iocontrol(this, control_code, If(in_value, true_, false_), out_value)
    End Function

    <Extension()> Public Function set_iocontrol(ByVal this As Socket,
                                                ByVal control_code As IOControlCode,
                                                ByVal in_value As Boolean,
                                                Optional ByVal out_value() As Byte = Nothing) As Int32
        Return set_iocontrol(this, control_code, If(in_value, true_, false_), out_value)
    End Function

    <Extension()> Public Function set_keepalive(ByVal c As Socket,
                                                Optional ByVal enable As Boolean = enable_socket_keepalive,
                                                Optional ByVal first_keepalive_ms As UInt32 = socket_first_keepalive_ms,
                                                Optional ByVal interval_ms As UInt32 = socket_keepalive_interval_ms) _
                                               As Boolean
        If c Is Nothing Then
            Return False
        Else
            Try
                c.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, True)
            Catch ex As Exception
                If network_trace Then
                    raise_error(error_type.warning,
                                "failed to set socket option keepalive on client ",
                                c.identity(),
                                ", ex ",
                                ex.Message())
                End If
                Return False
            End Try
            Dim v() As Byte = Nothing
            ReDim v(sizeof_uint32 * 3 - 1)
            Dim offset As UInt32 = 0
            If enable Then
                assert(uint32_bytes(uint32_1, v, offset))
            Else
                assert(uint32_bytes(uint32_0, v, offset))
            End If
            assert(uint32_bytes(first_keepalive_ms, v, offset))
            assert(uint32_bytes(interval_ms, v, offset))
            assert(offset = array_size(v))
            Return c.set_iocontrol(IOControlCode.KeepAliveValues, v, Nothing) <> npos
        End If
    End Function

    <Extension()> Public Function enable_keepalive(ByVal c As Socket,
                                                   Optional ByVal first_keepalive_ms As UInt32 = 
                                                       socket_first_keepalive_ms,
                                                   Optional ByVal interval_ms As UInt32 = _
                                                       socket_keepalive_interval_ms) As Boolean
        Return set_keepalive(c, first_keepalive_ms, interval_ms)
    End Function

    <Extension()> Public Function set_receive_buffer_size(ByVal u As Socket, ByVal size As UInt32) As Boolean
        If u Is Nothing Then
            Return False
        Else
            u.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, CInt(size))
            Return True
        End If
    End Function

    <Extension()> Public Function set_send_buffer_size(ByVal u As Socket, ByVal size As UInt32) As Boolean
        If u Is Nothing Then
            Return False
        Else
            u.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, CInt(size))
            Return True
        End If
    End Function

    <Extension()> Public Function ipv4(ByVal u As Socket) As Boolean
        If u Is Nothing Then
            Return False
        Else
            Return u.AddressFamily() = AddressFamily.InterNetwork
        End If
    End Function

    <Extension()> Public Function ipv6(ByVal u As Socket) As Boolean
        If u Is Nothing Then
            Return False
        Else
            Return u.AddressFamily() = AddressFamily.InterNetworkV6
        End If
    End Function
End Module
