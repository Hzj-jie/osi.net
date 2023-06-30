
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

'Not working multi-procedurally
Public Class block_pump_flow_pump_adapter
    Implements flow_pump

    Public ReadOnly block_pump_dev As block_pump
    Private buffered As piece

    Public Sub New(ByVal b As block_pump)
        assert(Not b Is Nothing)
        Me.block_pump_dev = b
        Me.buffered = Nothing
    End Sub

    Private Function receive_from_buffer(ByVal buff() As Byte,
                                         ByVal offset As UInt32,
                                         ByVal count As UInt32,
                                         ByVal result As ref(Of UInt32)) As Boolean
        If buffered Is Nothing Then
            Return False
        Else
            assert(Not buffered.empty())
            If buffered.count <= count Then
                arrays.copy(buff, offset, buffered.buff, buffered.offset, buffered.count)
                eva(result, buffered.count)
                buffered = Nothing
            Else
                assert(buffered.count > count)
                arrays.copy(buff, offset, buffered.buff, buffered.offset, count)
                eva(result, count)
                buffered = buffered.consume(count)
            End If
            Return True
        End If
    End Function

    Public Function receive(ByVal buff() As Byte,
                            ByVal offset As UInt32,
                            ByVal count As UInt32,
                            ByVal result As ref(Of UInt32)) As event_comb Implements flow_pump.receive
        Dim ec As event_comb = Nothing
        Dim b As ref(Of Byte()) = Nothing
        Return New event_comb(Function() As Boolean
                                  If receive_from_buffer(buff, offset, count, result) Then
                                      Return goto_end()
                                  Else
                                      b = New ref(Of Byte())()
                                      ec = block_pump_dev.receive(b)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Not b Is Nothing)
                                  If ec.end_result() Then
                                      If isemptyarray(+b) Then
                                          Return eva(result, uint32_0) AndAlso
                                                 goto_end()
                                      Else
                                          If array_size(+b) > count Then
                                              arrays.copy(buff, offset, +b, uint32_0, count)
                                              eva(result, count)
                                              buffered = New piece(+b, count, array_size(+b) - count)
                                              Return goto_end()
                                          Else
                                              arrays.copy(buff, offset, +b, uint32_0, array_size(+b))
                                              Return eva(result, array_size(+b)) AndAlso
                                                     goto_end()
                                          End If
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
