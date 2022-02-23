
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.service.convertor
Imports osi.service.transmitter

Public Class client_dev
    Protected Shared ReadOnly attribute As trait
    Protected ReadOnly send_link_status As link_status
    Protected ReadOnly receive_link_status As link_status
    Protected ReadOnly host As String
    Protected ReadOnly port As UInt16
    Private ReadOnly id As String

    Shared Sub New()
        attribute = trait.[New]().
                        with_transmit_mode(trait.mode_t.send_receive).
                        with_concurrent_operation(False)
    End Sub

    Public Sub New(ByVal host As String,
                   ByVal port As UInt16,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        Me.host = host
        Me.port = port
        Me.send_link_status = send_link_status
        Me.receive_link_status = receive_link_status
        Me.id = strcat(Me.GetType().Name(), "@", host, ":", port)
    End Sub

    Public Sub New(ByVal host As IPAddress,
                   ByVal port As UInt16,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        Me.New(Convert.ToString(host), port, send_link_status, receive_link_status)
    End Sub

    Public Sub New(ByVal remote As IPEndPoint,
                   Optional ByVal send_link_status As link_status = Nothing,
                   Optional ByVal receive_link_status As link_status = Nothing)
        Me.New(If(remote Is Nothing, [default](Of IPAddress).null, remote.Address()),
               If(remote Is Nothing, uint16_0, CUShort(remote.Port())),
               send_link_status,
               receive_link_status)
    End Sub

    'for configuration or arguments
    Public Sub New(ByVal host As String,
                   ByVal port As String,
                   ByVal connect_timeout_ms As String,
                   ByVal response_timeout_ms As String,
                   ByVal buff_size As String,
                   ByVal rate_sec As String,
                   ByVal max_content_length As String)
        Me.New(host,
               port.to(Of UInt16)(),
               New link_status(connect_timeout_ms,
                               buff_size,
                               rate_sec,
                               max_content_length),
               New link_status(response_timeout_ms,
                               buff_size,
                               rate_sec,
                               max_content_length))
    End Sub

    Public Function question(Of R As {client.response, New})(ByVal sr As Func(Of R, event_comb)) As event_comb
        Dim ec As event_comb = Nothing
        Dim result As R = Nothing
        Return New event_comb(Function() As Boolean
                                  If sr Is Nothing Then
                                      Return False
                                  Else
                                      result = New R()
                                      ec = sr(result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         result.status() = HttpStatusCode.OK AndAlso
                                         result.headers() IsNot Nothing AndAlso
                                         strsame(result.headers()(HttpResponseHeader.ContentType),
                                                 constants.commander_content_type,
                                                 strlen(constants.commander_content_type),
                                                 False) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Shared Function identity(ByVal c As client_dev) As String
        assert(c IsNot Nothing)
        Return c.id
    End Function

    Protected Shared Sub close(ByVal c As client_dev)
    End Sub

    Protected Shared Function validate(ByVal c As client_dev) As Boolean
        Return True
    End Function
End Class
