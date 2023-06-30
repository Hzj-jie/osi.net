
Option Strict On

Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.transmitter
Imports osi.service.commander

Partial Public Class powerpoint
    Public Function as_herald(ByVal c As TcpClient, ByRef o As herald) As Boolean
        Dim b As block = Nothing
        If as_block(c, b) Then
            o = New block_herald_adapter(b)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function as_block(ByVal c As TcpClient, ByRef o As block) As Boolean
        Dim f As flow = Nothing
        If as_flow(c, f) Then
            o = New flow_block_adapter(f)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function as_flow(ByVal c As TcpClient, ByRef o As flow) As Boolean
        If c Is Nothing Then
            Return False
        Else
            o = New client_flow_adapter(c, Me)
            Return True
        End If
    End Function

    Public Function as_herald(ByVal c As TcpClient) As herald
        Dim h As herald = Nothing
        assert(as_herald(c, h))
        Return h
    End Function

    Public Function as_block(ByVal c As TcpClient) As block
        Dim b As block = Nothing
        assert(as_block(c, b))
        Return b
    End Function

    Public Function as_flow(ByVal c As TcpClient) As flow
        Dim f As flow = Nothing
        assert(as_flow(c, f))
        Return f
    End Function
End Class
