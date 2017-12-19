
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter

Public MustInherit Class client_block_dev
    Inherits client_dev
    Implements block

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
    Public Sub New(ByVal host As String,
                   ByVal port As String,
                   ByVal connect_timeout_ms As String,
                   ByVal response_timeout_ms As String,
                   ByVal buff_size As String,
                   ByVal rate_sec As String,
                   ByVal max_content_length As String)
        MyBase.New(host, port, connect_timeout_ms, response_timeout_ms, buff_size, rate_sec, max_content_length)
    End Sub

    Protected MustOverride Function store_request(ByVal buff() As Byte,
                                                  ByVal offset As UInt32,
                                                  ByVal count As UInt32) As Boolean

    Public Function send(ByVal buff() As Byte,
                         ByVal offset As UInt32,
                         ByVal count As UInt32) As event_comb Implements block_injector.send
        Return sync_async(Function()
                              Return array_size(buff) >= offset + count AndAlso store_request(buff, offset, count)
                          End Function)
    End Function

    Protected MustOverride Function issue_request(ByVal r As client.memory_stream_response) As event_comb

    Public Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Return question(Function(ByVal r As client.memory_stream_response) As event_comb
                            assert(Not r Is Nothing)
                            Dim ec As event_comb = Nothing
                            Return New event_comb(Function() As Boolean
                                                      ec = issue_request(r)
                                                      Return waitfor(ec) AndAlso
                                                             goto_next()
                                                  End Function,
                                                  Function() As Boolean
                                                      Return ec.end_result() AndAlso
                                                             eva(result, r.ms.ToArray()) AndAlso
                                                             goto_end()
                                                  End Function)
                        End Function)
    End Function

    Public Function sense(ByVal pending As pointer(Of Boolean),
                          ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
        Return sync_async(Sub()
                              eva(pending, True)
                          End Sub)
    End Function
End Class
