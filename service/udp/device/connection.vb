
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.selector
Imports envs = osi.root.envs

Public NotInheritable Class connection
    Public Shared Function [New](ByVal c As async_getter(Of UdpClient)) As idevice(Of async_getter(Of UdpClient))
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public Shared Function [New](ByVal c As UdpClient) As idevice(Of UdpClient)
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public Shared Function [New](ByVal c As async_getter(Of delegator)) As idevice(Of async_getter(Of delegator))
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public Shared Function [New](ByVal c As delegator) As idevice(Of delegator)
        Return c.make_device(AddressOf validate, AddressOf close, AddressOf identity)
    End Function

    Public Shared Function [New](ByVal sources() As IPEndPoint,
                                 ByVal c As UdpClient,
                                 ByVal p As powerpoint) As idevice(Of delegator)
        Return [New](New delegator(sources, c, p))
    End Function

    Private Shared Sub close(ByVal shutdown As Action, ByVal id As Func(Of String))
        assert(Not shutdown Is Nothing)
        assert(Not id Is Nothing)
        If envs.udp_trace Then
            raise_error("connection ",
                        id(),
                        " has been closed by closer delegate")
        End If
        shutdown()
    End Sub

    Private Shared Sub close(ByVal c As UdpClient)
        close(Sub()
                  c.shutdown()
              End Sub,
              Function() As String
                  Return identity(c)
              End Function)
    End Sub

    Private Shared Function validate(ByVal c As UdpClient) As Boolean
        assert(Not c Is Nothing)
        Return c.alive()
    End Function

    Private Shared Function identity(ByVal c As UdpClient) As String
        assert(Not c Is Nothing)
        Return c.identity(0)
    End Function

    Private Shared Sub close(ByVal c As delegator)
        close(Sub()
                  c.shutdown()
              End Sub,
              Function() As String
                  Return identity(c)
              End Function)
    End Sub

    Private Shared Function validate(ByVal c As delegator) As Boolean
        assert(Not c Is Nothing)
        Return c.alive()
    End Function

    Private Shared Function identity(ByVal c As delegator) As String
        assert(Not c Is Nothing)
        Return c.id
    End Function

    Private Sub New()
    End Sub
End Class
