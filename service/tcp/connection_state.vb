
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.constants
Imports envs = osi.root.envs
Imports _socket = osi.root.utils._socket

Public Class connection_state
    Private Shared ReadOnly properties As IPGlobalProperties
    Private Shared ReadOnly rec As reference_count_event_comb_1
    Private Shared m As map(Of String, TcpState)

    Shared Sub New()
        If Not envs.disable_tcp_socket_state Then
            properties = IPGlobalProperties.GetIPGlobalProperties()
            rec = reference_count_event_comb_1.ctor(
                      Nothing,
                      Function() As event_comb
                          Const ms As Int64 = tcp_socket_state_update_interval_ms
                          Return New event_comb(Function() As Boolean
                                                    m = states()
                                                    Return waitfor(ms)
                                                End Function)
                      End Function,
                      Function() As event_comb
                          Return sync_async(Sub()
                                                m = Nothing
                                            End Sub)
                      End Function)
        End If
    End Sub

    Public Shared Function active_connections(ByVal p As IPGlobalProperties,
                                              ByRef a() As TcpConnectionInformation) As Boolean
        If p Is Nothing Then
            Return False
        Else
            Try
                a = p.GetActiveTcpConnections()
                Return True
            Catch ex As Exception
                raise_error(error_type.system,
                            "failed to get active tcp connections, ex ",
                            ex.Message())
                Return False
            End Try
        End If
    End Function

    Public Shared Function active_connections(ByRef a() As TcpConnectionInformation) As Boolean
        Return active_connections(properties, a)
    End Function

    Public Shared Function active_connections(ByVal p As IPGlobalProperties) As TcpConnectionInformation()
        Dim o() As TcpConnectionInformation = Nothing
        If active_connections(p, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function active_connections() As TcpConnectionInformation()
        Return active_connections(properties)
    End Function

    Public Shared Function states(ByVal a() As TcpConnectionInformation,
                                  ByRef o As map(Of String, TcpState)) As Boolean
        If a Is Nothing Then
            Return Nothing
        Else
            o.renew()
            For i As Int32 = 0 To array_size(a) - 1
                If Not a(i) Is Nothing Then
                    o(_socket.identity(a(i).LocalEndPoint(), a(i).RemoteEndPoint())) = a(i).State()
                End If
            Next
            Return True
        End If
    End Function

    Public Shared Function states(ByVal a() As TcpConnectionInformation) As map(Of String, TcpState)
        Dim o As map(Of String, TcpState) = Nothing
        If states(a, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function states(ByVal p As IPGlobalProperties, ByRef o As map(Of String, TcpState)) As Boolean
        Dim a() As TcpConnectionInformation = Nothing
        Return active_connections(p, a) AndAlso
               states(a, o)
    End Function

    Public Shared Function states(ByVal p As IPGlobalProperties) As map(Of String, TcpState)
        Dim o As map(Of String, TcpState) = Nothing
        If states(p, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function states(ByRef o As map(Of String, TcpState)) As Boolean
        Return states(properties, o)
    End Function

    Public Shared Function states() As map(Of String, TcpState)
        Dim o As map(Of String, TcpState) = Nothing
        If states(o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function query(ByVal m As map(Of String, TcpState),
                                 ByVal local As IPEndPoint,
                                 ByVal remote As IPEndPoint) As TcpState
        If m Is Nothing Then
            Return TcpState.Unknown
        ElseIf local Is Nothing OrElse remote Is Nothing Then
            Return TcpState.Closed
        Else
            Dim id As String = Nothing
            id = _socket.identity(local, remote)
            assert(Not String.IsNullOrEmpty(id))
            Dim it As map(Of String, TcpState).iterator = Nothing
            it = m.find(id)
            If it = m.end() Then
                Return TcpState.Unknown
            Else
                Return (+it).second
            End If
        End If
    End Function

    Public Shared Function query(ByVal m As map(Of String, TcpState),
                                 ByVal s As Socket) As TcpState
        If s Is Nothing Then
            Return TcpState.Closed
        Else
            Return query(m, s.LocalEndPoint(), s.RemoteEndPoint())
        End If
    End Function

    Public Shared Function query(ByVal local As IPEndPoint,
                                 ByVal remote As IPEndPoint) As TcpState
        Return query(m, local, remote)
    End Function

    Public Shared Function query(ByVal s As Socket) As TcpState
        Return query(m, s)
    End Function

    Public Shared Function bind() As Boolean
        If rec Is Nothing Then
            Return False
        Else
            Return rec.bind()
        End If
    End Function

    Public Shared Function release() As Boolean
        If rec Is Nothing Then
            Return False
        Else
            Return rec.release()
        End If
    End Function

    Public Shared Function valid() As Boolean
        Return Not m Is Nothing
    End Function
End Class
