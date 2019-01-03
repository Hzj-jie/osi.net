
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.sharedtransmitter

Partial Public Class sharedtransmitter_test
    Private NotInheritable Class shared_receiver
        Inherits sharedtransmitter(Of Byte, Byte, component, Int32, parameter).shared_receiver

        Public Sub New(ByVal c As ref_instance(Of component))
            MyBase.New(c)
        End Sub

        Public Overrides Function receive(
                ByVal o As pointer(Of pair(Of Int32, const_pair(Of Byte, Byte)))) As event_comb
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If assertion.is_true(referred()) Then
                                          ec = component().receiver.receive(o)
                                          Return waitfor(ec) AndAlso
                                             goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end_result())
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function

        Public Overrides Function sense(ByVal pending As pointer(Of Boolean),
                                        ByVal timeout_ms As Int64) As event_comb
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If assertion.is_true(referred()) Then
                                          ec = component().receiver.sense(pending, timeout_ms)
                                          Return waitfor(ec) AndAlso
                                             goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assertion.is_true(ec.end_result())
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Function
    End Class
End Class
