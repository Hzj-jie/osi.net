
Imports System.IO
Imports System.Net
Imports System.Text
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation
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
        Me.New(If(remote Is Nothing, DirectCast(Nothing, IPAddress), remote.Address()),
               If(remote Is Nothing, 0, remote.Port()),
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
               port.to_uint16(),
               New link_status(connect_timeout_ms,
                               buff_size,
                               rate_sec,
                               max_content_length),
               New link_status(response_timeout_ms,
                               buff_size,
                               rate_sec,
                               max_content_length))
    End Sub

    Public Function question(ByVal sr As Func(Of pointer(Of HttpStatusCode), 
                                                 pointer(Of WebHeaderCollection), 
                                                 event_comb)) As event_comb
        Dim ec As event_comb = Nothing
        Dim sc As pointer(Of HttpStatusCode) = Nothing
        Dim hc As pointer(Of WebHeaderCollection) = Nothing
        Return New event_comb(Function() As Boolean
                                  If sr Is Nothing Then
                                      Return False
                                  Else
                                      sc = New pointer(Of HttpStatusCode)()
                                      hc = New pointer(Of WebHeaderCollection)()
                                      ec = sr(sc, hc)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (+sc) = HttpStatusCode.OK AndAlso
                                         Not (+hc) Is Nothing AndAlso
                                         strsame((+hc)(HttpResponseHeader.ContentType),
                                                 constants.commander_content_type,
                                                 strlen(constants.commander_content_type),
                                                 False) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Shared Function identity(ByVal c As client_dev) As String
        assert(Not c Is Nothing)
        Return c.id
    End Function

    Protected Shared Sub close(ByVal c As client_dev)
    End Sub

    Protected Shared Function validate(ByVal c As client_dev) As Boolean
        Return True
    End Function
End Class
