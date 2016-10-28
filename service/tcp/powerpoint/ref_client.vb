
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.utils
Imports envs = osi.root.envs

<type_attribute()>
Public Class ref_client
    Public ReadOnly id As String
    Private ReadOnly c As TcpClient
    Private lrm As Int64

    Shared Sub New()
        type_attribute.of(Of ref_client).set(trait.[New]().
            with_transmit_mode(trait.mode_t.duplex))
    End Sub

    Public Sub New(ByVal c As TcpClient)
        assert(Not c Is Nothing)
        Me.c = c
        Me.id = c.identity()
        update_refer_ms()
    End Sub

    Public Sub New(ByVal p As powerpoint, ByVal c As TcpClient)
        Me.New(c)
        assert(Not p Is Nothing)
        If envs.tcp_trace Then
            raise_error("new conneciton ",
                        id,
                        " has been generated to the powerpoint ",
                        p.identity)
        End If
        c.set_no_delay(p.no_delay)
        c.set_keepalive(p.enable_keepalive,
                        p.first_keepalive_ms,
                        p.keepalive_interval_ms)
    End Sub

    Public Shared Operator +(ByVal this As ref_client) As TcpClient
        assert(Not this Is Nothing)
        Return this.client()
    End Operator

    Public Function last_refer_ms() As Int64
        Return lrm
    End Function

    Public Function client() As TcpClient
        update_refer_ms()
        Return c
    End Function

    'return the client without update refer time, use only in internal status check
    Public Function no_refer_client() As TcpClient
        Return c
    End Function

    Public Sub update_refer_ms()
        lrm = nowadays.milliseconds()
    End Sub
End Class
