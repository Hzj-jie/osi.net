
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.convertor
Imports osi.service.argument
Imports osi.service.http.constants

Public Module _link_status
    <Extension()> Public Function this_or_unlimited(ByVal ls As link_status) As link_status
        Return If(ls Is Nothing, link_status.unlimited, ls)
    End Function
End Module

Public Class link_status
    Public ReadOnly timeout_ms As Int64
    Public ReadOnly buff_size As UInt32
    Public ReadOnly rate_sec As UInt32
    Public ReadOnly max_content_length As UInt64

    Public Sub New(ByVal timeout_ms As Int64,
                   Optional ByVal buff_size As UInt32 = default_value.buff_size,
                   Optional ByVal rate_sec As UInt32 = default_value.rate_sec,
                   Optional ByVal max_content_length As UInt64 = default_value.max_content_length)
        Me.timeout_ms = timeout_ms
        Me.buff_size = buff_size
        Me.rate_sec = rate_sec
        Me.max_content_length = max_content_length
    End Sub

    Public Sub New(ByVal timeout_ms As String,
                   ByVal buff_size As String,
                   ByVal rate_sec As String,
                   ByVal max_content_length As String)
        Me.New(timeout_ms.to_int64(default_value.connect_timeout_ms),
               buff_size.to_uint32(default_value.buff_size),
               rate_sec.to_uint32(default_value.rate_sec),
               max_content_length.to_uint64(default_value.max_content_length))
    End Sub

    Public Shared ReadOnly request As link_status
    Public Shared ReadOnly response As link_status
    Public Shared ReadOnly server As link_status
    Public Shared ReadOnly unlimited As link_status
    Public Shared ReadOnly null As link_status

    Shared Sub New()
        request = New link_status(default_value.connect_timeout_ms)
        response = New link_status(default_value.response_timeout_ms)
        server = New link_status(0)
        unlimited = New link_status(npos, npos, npos, npos)
        null = Nothing
    End Sub

    Public Shared Function create(ByVal v As var,
                                  ByRef o As link_status,
                                  ByVal server_ls As Boolean,
                                  ByVal request_ls As Boolean) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Const p_connect_timeout_ms As String = "connect-timeout-ms"
            Const p_response_timeout_ms As String = "response-timeout-ms"
            Const p_buff_size As String = "buff-size"
            Const p_rate_sec As String = "rate-sec"
            Const p_max_content_length As String = "max-content-length"
            If Not server_ls Then
                If request_ls Then
                    v.bind(p_connect_timeout_ms)
                Else
                    v.bind(p_response_timeout_ms)
                End If
            End If
            v.bind(p_buff_size, p_rate_sec, p_max_content_length)
            Dim connect_timeout_ms As String = Nothing
            Dim response_timeout_ms As String = Nothing
            Dim buff_size As String = Nothing
            Dim rate_sec As String = Nothing
            Dim max_content_length As String = Nothing
            If Not server_ls Then
                If request_ls Then
                    connect_timeout_ms = v(p_connect_timeout_ms)
                Else
                    response_timeout_ms = v(p_response_timeout_ms)
                End If
            End If
            buff_size = v(p_buff_size)
            rate_sec = v(rate_sec)
            max_content_length = v(p_max_content_length)
            If server_ls Then
                o = New link_status("0", buff_size, rate_sec, max_content_length)
            ElseIf request_ls Then
                o = New link_status(connect_timeout_ms, buff_size, rate_sec, max_content_length)
            Else
                o = New link_status(response_timeout_ms, buff_size, rate_sec, max_content_length)
            End If
            Return True
        End If
    End Function

    Public Shared Function create(ByVal v As var,
                                  ByVal server_ls As Boolean,
                                  ByVal request_ls As Boolean) As link_status
        Dim o As link_status = Nothing
        assert(create(v, o, server_ls, request_ls))
        Return o
    End Function

    Public Shared Function create_server_link_status(ByVal v As var) As link_status
        Return create(v, True, False)
    End Function

    Public Shared Function create_client_link_status(ByVal v As var,
                                                     Optional ByVal request_ls As Boolean = True) As link_status
        Return create(v, False, request_ls)
    End Function

    Public Shared Function create_request_link_status(ByVal v As var) As link_status
        Return create_client_link_status(v, True)
    End Function

    Public Shared Function create_response_link_status(ByVal v As var) As link_status
        Return create_client_link_status(v, False)
    End Function
End Class
