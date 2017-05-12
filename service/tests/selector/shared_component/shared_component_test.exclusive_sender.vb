
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private Class exclusive_sender
        Inherits shared_component(Of Byte, Byte, component, Int32, parameter).exclusive_sender

        Public Sub New(ByVal c As ref_instance(Of component), ByVal remote As const_pair(Of Byte, Byte))
            MyBase.New(c, remote)
        End Sub

        Public Overrides Function send(ByVal i As Int32) As event_comb
            Return New event_comb(Function() As Boolean
                                      If assert_true(referred()) Then
                                          component().send(i, address(), port())
                                          Return goto_end()
                                      Else
                                          Return False
                                      End If
                                  End Function)
        End Function
    End Class
End Class
