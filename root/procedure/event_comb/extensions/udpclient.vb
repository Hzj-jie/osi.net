
Imports System.Runtime.CompilerServices
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _udpclient
    Private Function send(ByVal this As UdpClient,
                          ByVal s As Func(Of AsyncCallback, IAsyncResult),
                          ByVal r As ref(Of UInt32)) As event_comb
        assert(Not s Is Nothing)
        Return create(Function() As Boolean
                          Return Not this Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                                     Return s(ac)
                                                                 End Function,
                                                                 Function(ar As IAsyncResult) As UInt32
                                                                     Return CUInt(this.EndSend(ar))
                                                                 End Function,
                                                                 r)
                      End Function)
    End Function

    <Extension()> Public Function send(ByVal this As UdpClient,
                                       ByVal d() As Byte,
                                       ByVal len As UInt32,
                                       ByVal remote_endpoint As IPEndPoint,
                                       Optional ByVal r As ref(Of UInt32) = Nothing) As event_comb
        Return send(this, Function(ac) this.BeginSend(d, len, remote_endpoint, ac, Nothing), r)
    End Function

    <Extension()> Public Function send(ByVal this As UdpClient,
                                       ByVal d() As Byte,
                                       ByVal remote_endpoint As IPEndPoint,
                                       Optional ByVal r As ref(Of UInt32) = Nothing) As event_comb
        Return send(this, d, array_size(d), remote_endpoint, r)
    End Function

    <Extension()> Public Function send(ByVal this As UdpClient,
                                       ByVal d() As Byte,
                                       ByVal len As UInt32,
                                       Optional ByVal r As ref(Of UInt32) = Nothing) As event_comb
        Return send(this, Function(ac) this.BeginSend(d, CInt(len), ac, Nothing), r)
    End Function

    <Extension()> Public Function send(ByVal this As UdpClient,
                                       ByVal d() As Byte,
                                       Optional ByVal r As ref(Of UInt32) = Nothing) As event_comb
        Return send(this, d, array_size(d), r)
    End Function

    <Extension()> Public Function send(ByVal this As UdpClient,
                                       ByVal d() As Byte,
                                       ByVal len As UInt32,
                                       ByVal host As String,
                                       ByVal port As UInt16,
                                       Optional ByVal r As ref(Of UInt32) = Nothing) As event_comb
        Return send(this, Function(ac) this.BeginSend(d, len, host, CInt(port), ac, Nothing), r)
    End Function

    <Extension()> Public Function send(ByVal this As UdpClient,
                                       ByVal d() As Byte,
                                       ByVal host As String,
                                       ByVal port As UInt16,
                                       Optional ByVal r As ref(Of UInt32) = Nothing) As event_comb
        Return send(this, d, array_size(d), host, port, r)
    End Function

    <Extension()> Public Function receive(ByVal this As UdpClient,
                                          Optional ByVal r As ref(Of Byte()) = Nothing,
                                          Optional ByVal ep As ref(Of IPEndPoint) = Nothing) _
                                         As event_comb
        Return create(Function() As Boolean
                          Return Not this Is Nothing
                      End Function,
                      Function() As event_comb
                          Return event_comb_async_operation.ctor(Function(ac As AsyncCallback) As IAsyncResult
                                                                     Return this.BeginReceive(ac, Nothing)
                                                                 End Function,
                                                                 Function(ar As IAsyncResult) As Byte()
                                                                     Dim rep As IPEndPoint = Nothing
                                                                     Dim rtn() As Byte = Nothing
                                                                     rtn = this.EndReceive(ar, rep)
                                                                     eva(ep, rep)
                                                                     Return rtn
                                                                 End Function,
                                                                 r)
                      End Function)
    End Function
End Module
