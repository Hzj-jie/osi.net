
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.device

<global_init(global_init_level.server_services)>
Friend Module _registry
    Private Sub parse_http_patameters(ByVal v As var, ByRef host As String, ByRef port As UInt16)
        Const p_host As String = "host"
        Const p_port As String = "port"
        assert(Not v Is Nothing)
        v.bind(p_host, p_port)
        host = v(p_host)
        port = v(p_port).to_uint16()
    End Sub

    Private Function create(ByVal v As var, ByRef o As client_get_dev) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Dim host As String = Nothing
            Dim port As UInt16 = uint16_0
            parse_http_patameters(v, host, port)
            o = New client_get_dev(host,
                                   port,
                                   link_status.create_request_link_status(v),
                                   link_status.create_response_link_status(v))
            Return True
        End If
    End Function

    Private Function create(ByVal v As var, ByRef o As client_post_dev) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Dim host As String = Nothing
            Dim port As UInt16 = uint16_0
            parse_http_patameters(v, host, port)
            o = New client_post_dev(host,
                                    port,
                                    link_status.create_request_link_status(v),
                                    link_status.create_response_link_status(v))
            Return True
        End If
    End Function

    Private Function create(ByVal v As var, ByRef o As client_text_dev) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Const p_mode As String = "mode"
            v.bind(p_mode)
            If strsame(v(p_mode), "get", False) Then
                Return create_get(v, o)
            Else
                Return create_post(v, o)
            End If
        End If
    End Function

    Private Function create_get(ByVal v As var, ByRef o As client_text_dev) As Boolean
        Dim d As client_get_dev = Nothing
        If create(v, d) Then
            o = d
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create_post(ByVal v As var, ByRef o As client_text_dev) As Boolean
        Dim d As client_post_dev = Nothing
        If create(v, d) Then
            o = d
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create(ByVal v As var, ByRef o As text) As Boolean
        Dim d As client_text_dev = Nothing
        If create(v, d) Then
            o = d
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create_get(ByVal v As var, ByRef o As text) As Boolean
        Dim d As client_text_dev = Nothing
        If create_get(v, d) Then
            o = d
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create_post(ByVal v As var, ByRef o As text) As Boolean
        Dim d As client_text_dev = Nothing
        If create_post(v, d) Then
            o = d
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create(ByVal v As var, ByRef o As idevice(Of text)) As Boolean
        Dim d As client_text_dev = Nothing
        If create(v, d) Then
            o = d.as_device()
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create_get(ByVal v As var, ByRef o As idevice(Of text)) As Boolean
        Dim d As client_get_dev = Nothing
        If create(v, d) Then
            o = d.as_device()
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create_post(ByVal v As var, ByRef o As idevice(Of text)) As Boolean
        Dim d As client_post_dev = Nothing
        If create(v, d) Then
            o = d.as_device()
            Return True
        Else
            Return False
        End If
    End Function

    Private Function create(ByVal v As var) As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(Function(ByRef d As idevice(Of text)) As Boolean
                                                          Return create(v, d)
                                                      End Function)
    End Function

    Private Function create_get(ByVal v As var) As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(Function(ByRef d As idevice(Of text)) As Boolean
                                                          Return create_get(v, d)
                                                      End Function)
    End Function

    Private Function create_post(ByVal v As var) As idevice_creator(Of text)
        ' VS 2010
        Return delegate_device_creator.[New](Of text)(Function(ByRef d As idevice(Of text)) As Boolean
                                                          Return create_post(v, d)
                                                      End Function)
    End Function

    Sub New()
        assert(constructor.register(Function(v As var, ByRef o As client_get_dev) As Boolean
                                        Return create(v, o)
                                    End Function))
        assert(constructor.register(Function(v As var, ByRef o As client_post_dev) As Boolean
                                        Return create(v, o)
                                    End Function))
        assert(constructor.register(Function(v As var, ByRef o As client_text_dev) As Boolean
                                        Return create(v, o)
                                    End Function))
        assert(constructor.register("get",
                                    Function(v As var, ByRef o As client_text_dev) As Boolean
                                        Return create_get(v, o)
                                    End Function))
        assert(constructor.register("post",
                                    Function(v As var, ByRef o As client_text_dev) As Boolean
                                        Return create_post(v, o)
                                    End Function))
        assert(constructor.register("http-client",
                                    Function(v As var, ByRef o As text) As Boolean
                                        Return create(v, o)
                                    End Function))
        assert(constructor.register("http-get",
                                    Function(v As var, ByRef o As text) As Boolean
                                        Return create_get(v, o)
                                    End Function))
        assert(constructor.register("http-post",
                                    Function(v As var, ByRef o As text) As Boolean
                                        Return create_post(v, o)
                                    End Function))
        assert(constructor.register("http",
                                    Function(v As var, ByRef o As idevice(Of text)) As Boolean
                                        Return create(v, o)
                                    End Function))
        assert(constructor.register("http-get",
                                    Function(v As var, ByRef o As idevice(Of text)) As Boolean
                                        Return create_get(v, o)
                                    End Function))
        assert(constructor.register("http-post",
                                    Function(v As var, ByRef o As idevice(Of text)) As Boolean
                                        Return create_post(v, o)
                                    End Function))
        assert(constructor.register("http",
                                    Function(v As var) As idevice_creator(Of text)
                                        Return create(v)
                                    End Function))
        assert(constructor.register("http-get",
                                    Function(v As var) As idevice_creator(Of text)
                                        Return create_get(v)
                                    End Function))
        assert(constructor.register("http-post",
                                    Function(v As var) As idevice_creator(Of text)
                                        Return create(v)
                                    End Function))
    End Sub

    Private Sub init()
    End Sub
End Module
