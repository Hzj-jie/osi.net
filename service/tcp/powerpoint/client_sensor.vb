
Imports System.Net.Sockets
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.transmitter

Public Class client_sensor
    Private Class client_indicator
        Implements sync_indicator

        Private ReadOnly c As TcpClient
        Private ReadOnly checker As client_checker

        Public Sub New(ByVal c As TcpClient, ByVal p As powerpoint)
            assert(Not c Is Nothing)
            Me.c = c
            Me.checker = New client_checker(c, p)
        End Sub

        Public Function indicate(ByRef pending As Boolean) As Boolean Implements sync_indicator.indicate
            If checker.check() Then
                Dim b As Int32 = 0
                b = c.buffered_bytes()
                If b < 0 Then
                    Return False
                Else
                    pending = (b > 0)
                    Return True
                End If
            Else
                Return False
            End If
        End Function
    End Class

    Private Class tcpclient_sensor
        Implements sensor

        Private Shared ReadOnly buff() As Byte
        Private Shared ReadOnly buff_size As Int32
        Private ReadOnly c As TcpClient
        Private ReadOnly checker As client_checker

        Shared Sub New()
            buff_size = 1
            ReDim buff(buff_size - 1)
        End Sub

        Public Sub New(ByVal c As TcpClient, ByVal p As powerpoint)
            assert(Not c Is Nothing)
            Me.c = c
            Me.checker = New client_checker(c, p)
        End Sub

        Public Function sense(ByVal pending As pointer(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
            Dim ec As event_comb = Nothing
            Dim result As pointer(Of Int32) = Nothing
            Return New event_comb(Function() As Boolean
                                      If checker.check() Then
                                          result = New pointer(Of Int32)()
                                          ec = event_comb_async_operation.ctor(
                                                   Function(ac As AsyncCallback) As IAsyncResult
                                                       Return c.Client().BeginReceive(buff,
                                                                                      0,
                                                                                      buff_size,
                                                                                      SocketFlags.Peek,
                                                                                      ac,
                                                                                      Nothing)
                                                   End Function,
                                                   Function(ar As IAsyncResult) As Int32
                                                       Return c.Client().EndReceive(ar)
                                                   End Function,
                                                   result)
                                          Return waitfor(ec, timeout_ms) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      Return eva(pending, ec.end_result() AndAlso ((+result) = buff_size)) AndAlso
                                             goto_end()
                                  End Function)
        End Function
    End Class

    Public Shared Function socket_sensor(ByVal c As TcpClient, ByVal p As powerpoint) As sensor
        Return New indicator_sensor_adapter(New client_indicator(c, p))
    End Function

    Public Shared Function stream_sensor(ByVal c As TcpClient, ByVal p As powerpoint) As sensor
        Return New tcpclient_sensor(c, p)
    End Function

    Public Shared Function create(ByVal c As TcpClient, ByVal p As powerpoint) As sensor
        Return If(constants.use_socket, socket_sensor(c, p), stream_sensor(c, p))
    End Function
End Class
