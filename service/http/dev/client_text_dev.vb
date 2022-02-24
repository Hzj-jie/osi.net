
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.transmitter

Public MustInherit Class client_text_dev
    Inherits client_dev
    Implements text

    Protected Sub New(ByVal host As String,
                      ByVal port As UInt16,
                      Optional ByVal send_link_status As link_status = Nothing,
                      Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(host, port, send_link_status, receive_link_status)
    End Sub

    Protected Sub New(ByVal host As IPAddress,
                      ByVal port As UInt16,
                      Optional ByVal send_link_status As link_status = Nothing,
                      Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(host, port, send_link_status, receive_link_status)
    End Sub

    Protected Sub New(ByVal remote As IPEndPoint,
                      Optional ByVal send_link_status As link_status = Nothing,
                      Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(remote, send_link_status, receive_link_status)
    End Sub

    'for configuration or arguments
    Protected Sub New(ByVal host As String,
                      ByVal port As String,
                      ByVal connect_timeout_ms As String,
                      ByVal response_timeout_ms As String,
                      ByVal buff_size As String,
                      ByVal rate_sec As String,
                      ByVal max_content_length As String)
        MyBase.New(host, port, connect_timeout_ms, response_timeout_ms, buff_size, rate_sec, max_content_length)
    End Sub

    Public Shadows Function sense(ByVal pending As ref(Of Boolean),
                                  ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return sync_async(Sub()
                              eva(pending, True)
                          End Sub)
    End Function

    Protected MustOverride Function store_request(ByVal s As String,
                                                  ByVal offset As UInt32,
                                                  ByVal len As UInt32) As Boolean

    Public Function send(ByVal s As String,
                         ByVal offset As UInt32,
                         ByVal len As UInt32) As event_comb Implements text_injector.send
        Return sync_async(Function() As Boolean
                              Return strlen(s) >= offset + len AndAlso store_request(s, offset, len)
                          End Function)
    End Function

    Protected MustOverride Function issue_request(ByVal r As client.string_response) As event_comb

    Public Function receive(ByVal result As ref(Of String)) As event_comb Implements text_pump.receive
        Return question(Function(ByVal r As client.string_response) As event_comb
                            assert(Not r Is Nothing)
                            Dim ec As event_comb = Nothing
                            Return New event_comb(Function() As Boolean
                                                      ec = issue_request(r)
                                                      Return waitfor(ec) AndAlso
                                                             goto_next()
                                                  End Function,
                                                  Function() As Boolean
                                                      Return ec.end_result() AndAlso
                                                             eva(result, r.result()) AndAlso
                                                             goto_end()
                                                  End Function)
                        End Function)
    End Function

    Public Function as_device() As idevice(Of text)
        Return delegate_device(Of text).[New](Me, AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public MustInherit Class creator
        Public MustOverride Function N(ByVal host As String,
                                       ByVal port As UInt16,
                                       Optional ByVal send_link_status As link_status = Nothing,
                                       Optional ByVal receive_link_status As link_status = Nothing) As client_text_dev

        Public MustOverride Function N(ByVal host As IPAddress,
                                       ByVal port As UInt16,
                                       Optional ByVal send_link_status As link_status = Nothing,
                                       Optional ByVal receive_link_status As link_status = Nothing) As client_text_dev

        Public MustOverride Function N(ByVal remote As IPEndPoint,
                                       Optional ByVal send_link_status As link_status = Nothing,
                                       Optional ByVal receive_link_status As link_status = Nothing) As client_text_dev

        'for configuration or arguments
        Public MustOverride Function N(ByVal host As String,
                                       ByVal port As String,
                                       ByVal connect_timeout_ms As String,
                                       ByVal response_timeout_ms As String,
                                       ByVal buff_size As String,
                                       ByVal rate_sec As String,
                                       ByVal max_content_length As String) As client_text_dev

        Public MustOverride Function method_str() As String
    End Class
End Class

<type_attribute()>
Partial Public NotInheritable Class client_get_dev
    Inherits client_text_dev(Of creator)

    Shared Sub New()
        type_attribute.of(Of client_get_dev)().set(attribute)
    End Sub

    Private path As String

    Public Sub New(ByVal host As String,
                   ByVal port As UInt16,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(host, port, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal host As IPAddress,
                   ByVal port As UInt16,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(host, port, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal remote As IPEndPoint,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(remote, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal host As String,
                   ByVal port As String,
                   ByVal connect_timeout_ms As String,
                   ByVal response_timeout_ms As String,
                   ByVal buff_size As String,
                   ByVal rate_sec As String,
                   ByVal max_content_length As String)
        MyBase.New(host,
                   port,
                   connect_timeout_ms,
                   response_timeout_ms,
                   buff_size,
                   rate_sec,
                   max_content_length)
    End Sub

    Protected Overrides Function issue_request(ByVal r As client.string_response) As event_comb
        Return request_builder.
                       [New]().
                       with_url(generate_url(host, port, path)).
                       with_request_link_status(send_link_status).
                       with_response_link_status(receive_link_status).
                       request(r)
    End Function

    Protected Overrides Function store_request(ByVal s As String,
                                               ByVal offset As UInt32,
                                               ByVal len As UInt32) As Boolean
        assert(strlen(s) >= offset + len)
        If len = 0 Then
            path = constants.uri.path_separator
        Else
            path = strcat(constants.uri.path_separator,
                          constants.uri.query_mark,
                          Convert.ToBase64String(constants.dev_enc.GetBytes(s, CInt(offset), CInt(len))))
        End If
        Return True
    End Function
End Class

<type_attribute()>
Partial Public NotInheritable Class client_post_dev
    Inherits client_text_dev(Of creator)

    Shared Sub New()
        type_attribute.of(Of client_post_dev)().set(attribute)
    End Sub

    Private s As String
    Private offset As UInt32
    Private len As UInt32

    Public Sub New(ByVal host As String,
                   ByVal port As UInt16,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(host, port, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal host As IPAddress,
                   ByVal port As UInt16,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(host, port, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal remote As IPEndPoint,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        MyBase.New(remote, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal host As String,
                   ByVal port As String,
                   ByVal connect_timeout_ms As String,
                   ByVal response_timeout_ms As String,
                   ByVal buff_size As String,
                   ByVal rate_sec As String,
                   ByVal max_content_length As String)
        MyBase.New(host,
                   port,
                   connect_timeout_ms,
                   response_timeout_ms,
                   buff_size,
                   rate_sec,
                   max_content_length)
    End Sub

    Protected Overrides Function issue_request(ByVal r As client.string_response) As event_comb
        Return request_builder.
                       [New]().
                       with_url(generate_url(host, port)).
                       with_body(s, offset, len, constants.dev_enc).
                       with_request_link_status(send_link_status).
                       with_response_link_status(receive_link_status).
                       request(r)
    End Function

    Protected Overrides Function store_request(ByVal s As String,
                                               ByVal offset As UInt32,
                                               ByVal len As UInt32) As Boolean
        assert(strlen(s) >= offset + len)
        Me.s = s
        Me.offset = offset
        Me.len = len
        Return True
    End Function
End Class
