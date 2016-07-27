
Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.commander

Public MustInherit Class client_text_dev(Of C As client_text_dev.creator)
    Inherits client_text_dev

    Private Shared ReadOnly ctor As C

    Shared Sub New()
        ctor = alloc(Of C)()
    End Sub

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

    Public Shared Function device_creator(ByVal host As String,
                                          ByVal port As UInt16,
                                          Optional ByVal send_link_status As link_status = Nothing,
                                          Optional ByVal receive_link_status As link_status = Nothing) _
                                         As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(
                   Function() As idevice(Of text)
                       Return (ctor.N(host, port, send_link_status, receive_link_status)).as_device()
                   End Function)
    End Function

    Public Shared Function device_creator(ByVal host As IPAddress,
                                          ByVal port As UInt16,
                                          Optional ByVal send_link_status As link_status = Nothing,
                                          Optional ByVal receive_link_status As link_status = Nothing) _
                                         As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(
                   Function() As idevice(Of text)
                       Return (ctor.N(host, port, send_link_status, receive_link_status)).as_device()
                   End Function)
    End Function

    Public Shared Function device_creator(ByVal remote As IPEndPoint,
                                          Optional ByVal send_link_status As link_status = Nothing,
                                          Optional ByVal receive_link_status As link_status = Nothing) _
                                         As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(
                   Function() As idevice(Of text)
                       Return (ctor.N(remote, send_link_status, receive_link_status)).as_device()
                   End Function)
    End Function

    Public Shared Function device_creator(ByVal host As String,
                                          ByVal port As String,
                                          ByVal connect_timeout_ms As String,
                                          ByVal response_timeout_ms As String,
                                          ByVal buff_size As String,
                                          ByVal rate_sec As String,
                                          ByVal max_content_length As String) _
                                         As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(
                   Function() As idevice(Of text)
                       Return (ctor.N(host,
                                      port,
                                      connect_timeout_ms,
                                      response_timeout_ms,
                                      buff_size,
                                      rate_sec,
                                      max_content_length)).as_device()
                   End Function)
    End Function

    Public Shared Function device_pool(ByVal host As String,
                                       ByVal port As UInt16,
                                       Optional ByVal send_link_status As link_status = Nothing,
                                       Optional ByVal receive_link_status As link_status = Nothing,
                                       Optional ByVal max_connection As UInt32 = uint32_0) _
                                      As idevice_pool(Of text)
        Return one_off_device_pool.[New](device_creator(host, port, send_link_status, receive_link_status),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", host, ":", port))
    End Function

    Public Shared Function device_pool(ByVal host As IPAddress,
                                       ByVal port As UInt16,
                                       Optional ByVal send_link_status As link_status = Nothing,
                                       Optional ByVal receive_link_status As link_status = Nothing,
                                       Optional ByVal max_connection As UInt32 = uint32_0) _
                                      As idevice_pool(Of text)
        Return one_off_device_pool.[New](device_creator(host, port, send_link_status, receive_link_status),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", host, ":", port))
    End Function

    Public Shared Function device_pool(ByVal remote As IPEndPoint,
                                       Optional ByVal send_link_status As link_status = Nothing,
                                       Optional ByVal receive_link_status As link_status = Nothing,
                                       Optional ByVal max_connection As UInt32 = uint32_0) _
                                      As idevice_pool(Of text)
        Return one_off_device_pool.[New](device_creator(remote, send_link_status, receive_link_status),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", remote))
    End Function

    Public Shared Function device_pool(ByVal host As String,
                                       ByVal port As String,
                                       ByVal connect_timeout_ms As String,
                                       ByVal response_timeout_ms As String,
                                       ByVal buff_size As String,
                                       ByVal rate_sec As String,
                                       ByVal max_content_length As String,
                                       Optional ByVal max_connection As UInt32 = uint32_0) _
                                      As idevice_pool(Of text)
        Return one_off_device_pool.[New](device_creator(host,
                                                        port,
                                                        connect_timeout_ms,
                                                        response_timeout_ms,
                                                        buff_size,
                                                        rate_sec,
                                                        max_content_length),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", host, ":", port))
    End Function

    Public Shared Function herald_device_creator(ByVal host As String,
                                                 ByVal port As UInt16,
                                                 Optional ByVal send_link_status As link_status = Nothing,
                                                 Optional ByVal receive_link_status As link_status = Nothing) _
                                                As idevice_creator(Of herald)
        Return device_creator_adapter.[New](device_creator(host, port, send_link_status, receive_link_status),
                                            Function(i As text) As herald
                                                Return New text_herald_adapter(i)
                                            End Function)
    End Function

    Public Shared Function herald_device_creator(ByVal host As IPAddress,
                                                 ByVal port As UInt16,
                                                 Optional ByVal send_link_status As link_status = Nothing,
                                                 Optional ByVal receive_link_status As link_status = Nothing) _
                                                As idevice_creator(Of herald)
        Return device_creator_adapter.[New](device_creator(host, port, send_link_status, receive_link_status),
                                            Function(i As text) As herald
                                                Return New text_herald_adapter(i)
                                            End Function)
    End Function

    Public Shared Function herald_device_creator(ByVal remote As IPEndPoint,
                                                 Optional ByVal send_link_status As link_status = Nothing,
                                                 Optional ByVal receive_link_status As link_status = Nothing) _
                                                As idevice_creator(Of herald)
        Return device_creator_adapter.[New](device_creator(remote, send_link_status, receive_link_status),
                                            Function(i As text) As herald
                                                Return New text_herald_adapter(i)
                                            End Function)
    End Function

    Public Shared Function herald_device_creator(ByVal host As String,
                                                 ByVal port As String,
                                                 ByVal connect_timeout_ms As String,
                                                 ByVal response_timeout_ms As String,
                                                 ByVal buff_size As String,
                                                 ByVal rate_sec As String,
                                                 ByVal max_content_length As String) _
                                                As idevice_creator(Of herald)
        Return device_creator_adapter.[New](device_creator(host,
                                                           port,
                                                           connect_timeout_ms,
                                                           response_timeout_ms,
                                                           buff_size,
                                                           rate_sec,
                                                           max_content_length),
                                            Function(i As text) As herald
                                                Return New text_herald_adapter(i)
                                            End Function)
    End Function

    Public Shared Function herald_device_pool(ByVal host As String,
                                              ByVal port As UInt32,
                                              Optional ByVal send_link_status As link_status = Nothing,
                                              Optional ByVal receive_link_status As link_status = Nothing,
                                              Optional ByVal max_connection As UInt32 = uint32_0) _
                                             As idevice_pool(Of herald)
        Return one_off_device_pool.[New](herald_device_creator(host, port, send_link_status, receive_link_status),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", host, ":", port))
    End Function

    Public Shared Function herald_device_pool(ByVal host As IPAddress,
                                              ByVal port As UInt16,
                                              Optional ByVal send_link_status As link_status = Nothing,
                                              Optional ByVal receive_link_status As link_status = Nothing,
                                              Optional ByVal max_connection As UInt32 = uint32_0) _
                                             As idevice_pool(Of herald)
        Return one_off_device_pool.[New](herald_device_creator(host, port, send_link_status, receive_link_status),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", host, ":", port))
    End Function

    Public Shared Function herald_device_pool(ByVal remote As IPEndPoint,
                                              Optional ByVal send_link_status As link_status = Nothing,
                                              Optional ByVal receive_link_status As link_status = Nothing,
                                              Optional ByVal max_connection As UInt32 = uint32_0) _
                                             As idevice_pool(Of herald)
        Return one_off_device_pool.[New](herald_device_creator(remote, send_link_status, receive_link_status),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", remote))
    End Function

    Public Shared Function herald_device_pool(ByVal host As String,
                                              ByVal port As String,
                                              ByVal connect_timeout_ms As String,
                                              ByVal response_timeout_ms As String,
                                              ByVal buff_size As String,
                                              ByVal rate_sec As String,
                                              ByVal max_content_length As String,
                                              Optional ByVal max_connection As UInt32 = uint32_0) _
                                             As idevice_pool(Of herald)
        Return one_off_device_pool.[New](herald_device_creator(host,
                                                               port,
                                                               connect_timeout_ms,
                                                               response_timeout_ms,
                                                               buff_size,
                                                               rate_sec,
                                                               max_content_length),
                                         max_connection,
                                         strcat("http-", ctor.method_str(), "@", host, ":", port))
    End Function
End Class

Partial Public NotInheritable Class client_get_dev
    Public NotInheritable Shadows Class creator
        Inherits client_text_dev(Of creator).creator

        Public Overrides Function method_str() As String
            Return "get"
        End Function

        Public Overrides Function N(ByVal host As String,
                                    ByVal port As String,
                                    ByVal connect_timeout_ms As String,
                                    ByVal response_timeout_ms As String,
                                    ByVal buff_size As String,
                                    ByVal rate_sec As String,
                                    ByVal max_content_length As String) As client_text_dev
            Return New client_get_dev(host,
                                      port,
                                      connect_timeout_ms,
                                      response_timeout_ms,
                                      buff_size,
                                      rate_sec,
                                      max_content_length)
        End Function

        Public Overrides Function N(ByVal host As String,
                                    ByVal port As UInt16,
                                    Optional send_link_status As link_status = Nothing,
                                    Optional receive_link_status As link_status = Nothing) As client_text_dev
            Return New client_get_dev(host, port, send_link_status, receive_link_status)
        End Function

        Public Overrides Function N(ByVal host As IPAddress,
                                    ByVal port As UInt16,
                                    Optional send_link_status As link_status = Nothing,
                                    Optional receive_link_status As link_status = Nothing) As client_text_dev
            Return New client_get_dev(host, port, send_link_status, receive_link_status)
        End Function

        Public Overrides Function N(ByVal remote As IPEndPoint,
                                    Optional send_link_status As link_status = Nothing,
                                    Optional receive_link_status As link_status = Nothing) As client_text_dev
            Return New client_get_dev(remote, send_link_status, receive_link_status)
        End Function
    End Class
End Class

Partial Public NotInheritable Class client_post_dev
    Public NotInheritable Shadows Class creator
        Inherits client_text_dev(Of creator).creator

        Public Overrides Function method_str() As String
            Return "post"
        End Function

        Public Overrides Function N(ByVal host As String,
                                    ByVal port As String,
                                    ByVal connect_timeout_ms As String,
                                    ByVal response_timeout_ms As String,
                                    ByVal buff_size As String,
                                    ByVal rate_sec As String,
                                    ByVal max_content_length As String) As client_text_dev
            Return New client_post_dev(host,
                                       port,
                                       connect_timeout_ms,
                                       response_timeout_ms,
                                       buff_size,
                                       rate_sec,
                                       max_content_length)
        End Function

        Public Overrides Function N(ByVal host As String,
                                    ByVal port As UInt16,
                                    Optional send_link_status As link_status = Nothing,
                                    Optional receive_link_status As link_status = Nothing) As client_text_dev
            Return New client_post_dev(host, port, send_link_status, receive_link_status)
        End Function

        Public Overrides Function N(ByVal host As IPAddress,
                                    ByVal port As UInt16,
                                    Optional send_link_status As link_status = Nothing,
                                    Optional receive_link_status As link_status = Nothing) As client_text_dev
            Return New client_post_dev(host, port, send_link_status, receive_link_status)
        End Function

        Public Overrides Function N(ByVal remote As IPEndPoint,
                                    Optional send_link_status As link_status = Nothing,
                                    Optional receive_link_status As link_status = Nothing) As client_text_dev
            Return New client_post_dev(remote, send_link_status, receive_link_status)
        End Function
    End Class
End Class
