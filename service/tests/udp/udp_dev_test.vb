
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.udp

' TODO
#If 0 Then
Public Class udp_dev_test
    Inherits event_comb_case

    Private ReadOnly port1 As UInt16
    Private ReadOnly port2 As UInt16

    Public Sub New()
        MyBase.New()
        port1 = rnd_port()
        port2 = rnd_port()
    End Sub

    Public Overrides Function create() As event_comb
        Return New event_comb(Function() As Boolean

                              End Function)
    End Function
End Class
#End If
