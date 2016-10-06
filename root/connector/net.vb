
Imports System.Net
Imports System.Net.NetworkInformation

Public Module _net
    Private Function tcp_listening(ByVal p As UInt16) As Boolean
        Dim infos() As IPEndPoint = Nothing
        infos = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
        For i As Int32 = 0 To array_size(infos) - 1
            If infos(i).Port() = p Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function tcp_connected(ByVal p As UInt16) As Boolean
        Dim infos() As TcpConnectionInformation = Nothing
        infos = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections()
        For i As Int32 = 0 To array_size(infos) - 1
            If infos(i).LocalEndPoint().Port() = p Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function udp_listening(ByVal p As UInt16) As Boolean
        Dim infos() As IPEndPoint = Nothing
        infos = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners()
        For i As Int32 = 0 To array_size(infos) - 1
            If infos(i).Port() = p Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function local_port_available(ByVal p As UInt16) As Boolean
        Return local_tcp_port_available(p) AndAlso
               local_udp_port_available(p)
    End Function

    Public Function local_udp_port_available(ByVal p As UInt16) As Boolean
        Try
            Return Not udp_listening(p)
        Catch
            Return False
        End Try
    End Function

    Public Function local_tcp_port_available(ByVal p As UInt16) As Boolean
        Try
            Return Not tcp_listening(p) AndAlso
                   Not tcp_connected(p)
        Catch
            Return False
        End Try
    End Function

    Public Function local_tcp_port_listening(ByVal p As UInt16) As Boolean
        Try
            Return tcp_listening(p)
        Catch
            Return False
        End Try
    End Function

    Public Function local_tcp_port_connected(ByVal p As UInt16) As Boolean
        Try
            Return tcp_connected(p)
        Catch
            Return False
        End Try
    End Function

    Public Function local_udp_port_listening(ByVal p As UInt16) As Boolean
        Try
            Return udp_listening(p)
        Catch
            Return False
        End Try
    End Function
End Module
