
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.selector
Imports envs = osi.root.envs

Public NotInheritable Class connection
    Public Shared Function [New](ByVal c As async_getter(Of ref_client)) As idevice(Of async_getter(Of ref_client))
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public Shared Function [New](ByVal c As ref_client) As idevice(Of ref_client)
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public Shared Function [New](ByVal sources() As IPEndPoint,
                                 ByVal c As UdpClient,
                                 ByVal p As powerpoint) As idevice(Of ref_client)
        Return [New](New ref_client(sources, c, p))
    End Function

    Private Shared Sub close(ByVal c As ref_client)
        assert(Not c Is Nothing)
        If envs.udp_trace Then
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

    Private Sub New()
    End Sub
End Class
