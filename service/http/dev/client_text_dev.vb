
Imports System.IO
Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.convertor
Imports osi.service.commander
Imports osi.service.device
Imports default_value = osi.service.http.constants.default_value

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

    Public Shadows Function sense(ByVal pending As pointer(Of Boolean),
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

    Protected MustOverride Function issue_request(ByVal hs As pointer(Of HttpStatusCode),
                                                  ByVal hc As pointer(Of WebHeaderCollection),
                                                  ByVal r As pointer(Of String)) As event_comb

    Public Function receive(ByVal r As pointer(Of String)) As event_comb Implements text_pump.receive
        Return question(Function(hs As pointer(Of HttpStatusCode),
                                 hc As pointer(Of WebHeaderCollection)) As event_comb
                            Return issue_request(hs, hc, r)
                        End Function)
    End Function

    Public Function as_device() As idevice(Of text)
        Return New delegate_device(Of text)(Me, AddressOf validate, AddressOf close, AddressOf identity)
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

    Protected Overrides Function issue_request(ByVal hs As pointer(Of HttpStatusCode),
                                               ByVal hc As pointer(Of WebHeaderCollection),
                                               ByVal r As pointer(Of String)) As event_comb
        Return client.request(generate_url(host, port, path),
                              hs,
                              hc,
                              r,
                              send_link_status,
                              receive_link_status)
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
                          Convert.ToBase64String(constants.dev_enc.GetBytes(strmid(s, offset, len))))
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

    Protected Overrides Function issue_request(ByVal hs As pointer(Of HttpStatusCode),
                                               ByVal hc As pointer(Of WebHeaderCollection),
                                               ByVal r As pointer(Of String)) As event_comb
        Dim ec As event_comb = Nothing
        Dim ms As MemoryStream = Nothing
        Return New event_comb(Function() As Boolean
                                  assert(memory_stream.create(s,
                                                              offset,
                                                              len,
                                                              constants.dev_enc,
                                                              ms),
                                         "s = ", s, ", offset = ", offset, ", len = ", len, ", strlen(s) = ", strlen(s))
                                  ec = client.request(generate_url(host, port),
                                                      ms,
                                                      ms.Length(),
                                                      hs,
                                                      hc,
                                                      r,
                                                      send_link_status,
                                                      receive_link_status)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert(Not ms Is Nothing)
                                  ms.Dispose()
                                  ms.Close()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
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
