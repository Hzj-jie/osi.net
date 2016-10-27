
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.selector
Imports envs = osi.root.envs

Public NotInheritable Class connection
    Public Shared Function [New](ByVal p As powerpoint,
                                 ByVal c As async_getter(Of ref_client)) As idevice(Of async_getter(Of ref_client))
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity, checker(p))
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByVal c As ref_client) As idevice(Of ref_client)
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity, checker(p))
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByVal c As TcpClient) As idevice(Of ref_client)
        Return [New](p, New ref_client(p, c))
    End Function

    Private Sub New()
    End Sub

    Private Shared Sub close(ByVal c As ref_client)
        assert(Not c Is Nothing)
        If envs.tcp_trace Then
            raise_error("connection ",
                        identity(c),
                        " has been closed by closer delegate")
        End If
        c.no_refer_client().shutdown()
    End Sub

    Private Shared Function validate(ByVal c As ref_client) As Boolean
        assert(Not c Is Nothing)
        Return c.no_refer_client().alive()
    End Function

    Private Shared Function identity(ByVal c As ref_client) As String
        assert(Not c Is Nothing)
        Return c.id
    End Function

    Private Shared Sub check_expiration(ByVal p As powerpoint, ByVal c As ref_client)
        assert(Not p Is Nothing)
        assert(Not c Is Nothing)
        'do not shutdown outgoing connections, since we need to generate it again
        '2014 Apr. 15th
        '? to make sure the connection is alive, sometime we need to shutdown and regenerate the connection
        If p.max_lifetime_ms >= 0 AndAlso Not p.is_outgoing Then
            Dim exp As Boolean = False
            exp = (nowadays.milliseconds() - c.last_refer_ms() >= p.max_lifetime_ms)
            If exp Then
                If envs.tcp_trace Then
                    raise_error("shutdown a connection ",
                                c.id,
                                " on powerpoint ",
                                p.identity,
                                ", because of expiration, ",
                                "max_lifetime_ms = ",
                                p.max_lifetime_ms,
                                ", now - last_refer_ms = ",
                                nowadays.milliseconds() - c.last_refer_ms())
                End If
                close(c)
            End If
        End If
    End Sub

    Private Shared Sub check_connectivity(ByVal p As powerpoint, ByVal c As ref_client)
        assert(Not p Is Nothing)
        assert(Not c Is Nothing)
        c.no_refer_client().free_poll_alive(p)
    End Sub

    Private Shared Sub check(ByVal p As powerpoint, ByVal c As ref_client)
        check_expiration(p, c)
        check_connectivity(p, c)
    End Sub

    Private Shared Function checker(ByVal p As powerpoint) As Action(Of ref_client)
        Return Sub(c As ref_client)
                   check(p, c)
               End Sub
    End Function
End Class
