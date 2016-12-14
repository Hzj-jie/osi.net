
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.transmitter

Partial Public Class shared_component_test
    Private Class receiver
        Implements T_receiver(Of pair(Of Int32, const_pair(Of UInt16, UInt16)))

        Public ReadOnly c As ref_instance(Of component)

        Public Sub New(ByVal c As ref_instance(Of component))
            assert(Not c Is Nothing)
            Me.c = c
        End Sub

        Public Function receive(ByVal o As pointer(Of pair(Of Int32, const_pair(Of UInt16, UInt16)))) As event_comb _
                               Implements T_pump(Of pair(Of Int32, const_pair(Of UInt16, UInt16))).receive
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      assert_true(c.referred())
                                      Dim i As component = Nothing
                                      i = c.get()
                                      If assert_not_nothing(i) Then
                                          ec = i.receiver.receive(o)
                                          Return waitfor(ec) AndAlso
                                             goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assert_true(ec.end_result())
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function

        Public Function sense(ByVal pending As pointer(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      assert_true(c.referred())
                                      Dim i As component = Nothing
                                      i = c.get()
                                      If assert_not_nothing(i) Then
                                          ec = i.receiver.sense(pending, timeout_ms)
                                          Return waitfor(ec) AndAlso
                                             goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assert_true(ec.end_result())
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function
    End Class
End Class
