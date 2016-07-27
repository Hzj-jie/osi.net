
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.device
Imports envs = osi.root.envs

<type_attribute()>
Public Class ref_client
    Public ReadOnly p As powerpoint
    Public ReadOnly id As String
    Public ReadOnly sources As const_array(Of IPEndPoint)
    Private ReadOnly c As UdpClient
    Private lrm As Int64

    Shared Sub New()
        type_attribute.of(Of ref_client).set(transmitter.[New]().
            with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New(ByVal sources() As IPEndPoint, ByVal c As UdpClient)
        Me.New(sources, c, Nothing)
    End Sub

    Public Sub New(ByVal sources() As IPEndPoint, ByVal c As UdpClient, ByVal p As powerpoint)
        assert(Not c Is Nothing)
        Me.c = c
        Me.sources = sources
        Me.id = c.identity()
        Me.p = p
        update_refer_ms()
        If Not p Is Nothing Then
            If envs.udp_trace Then
                raise_error("new conneciton ",
                            id,
                            " has been generated to the powerpoint ",
                            p.identity)
            End If
        End If
    End Sub

    Public Shared Operator +(ByVal this As ref_client) As UdpClient
        assert(Not this Is Nothing)
        Return this.client()
    End Operator

    Public Function last_refer_ms() As Int64
        Return lrm
    End Function

    Public Function client() As UdpClient
        update_refer_ms()
        Return c
    End Function

    'return the client without update refer time, use only in internal status check
    Public Function no_refer_client() As UdpClient
        Return c
    End Function

    Public Sub update_refer_ms()
        lrm = nowadays.milliseconds()
    End Sub
End Class
