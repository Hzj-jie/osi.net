
Imports System.Net.Sockets
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.service.device
Imports osi.service.iosys
Imports osi.service.input
Imports osi.service.output
Imports osi.service.tcp
Imports osi.service.commander
Imports osi.service.device
Imports input_case = osi.service.input.case
Imports responder = osi.service.tcp.responder

Public Class server
    Private Shared Function create_input_iosys(ByVal pp As powerpoint, ByVal c As TcpClient) As input
        assert(pp IsNot Nothing)
        assert(c IsNot Nothing)
        Dim bytes_ragent As bytes_ragent(Of input_case) = Nothing
        bytes_ragent = New bytes_ragent(Of input_case)(Function(r As pointer(Of Byte())) As event_comb
                                                           Return pp.as_block(c).receive(r)
                                                       End Function)
    End Function

    Public Shared Sub run(ByVal pp As powerpoint)
        assert(pp.get_all(Function(c As pre_initialized_device(Of ref_client)) As event_comb
                              Return New event_comb(Function() As Boolean

                                                    End Function)
                          End Function))
    End Sub
End Class
