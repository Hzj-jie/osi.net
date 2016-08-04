
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device

Partial Public Class dispenser_test
    Private Class receiver
        Implements T_receiver(Of pair(Of Int32, Int32))

        Private ReadOnly q As qless2(Of pair(Of Int32, Int32))
        Private ReadOnly s As sensor

        Public Sub New(ByVal q As qless2(Of pair(Of Int32, Int32)))
            assert(Not q Is Nothing)
            Me.q = q
            Me.s = as_sensor(Function() As Boolean
                                 Return Not q.empty()
                             End Function)
        End Sub

        Public Function sense(ByVal pending As pointer(Of Boolean),
                              ByVal timeout_ms As Int64) As event_comb _
                             Implements T_receiver(Of pair(Of Int32, Int32)).sense
            Return s.sense(pending, timeout_ms)
        End Function

        Public Function receive(ByVal o As pointer(Of pair(Of Int32, Int32))) As event_comb _
                               Implements T_receiver(Of pair(Of Int32, Int32)).receive
            Return sync_async(Sub()
                                  If assert_not_nothing(o) AndAlso assert_true(o.empty()) Then
                                      Dim p As pair(Of Int32, Int32) = Nothing
                                      If q.pop(p) Then
                                          eva(o, p)
                                      End If
                                  End If
                              End Sub)
        End Function
    End Class
End Class
