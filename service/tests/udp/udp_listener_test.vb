
Imports osi.root.procedure
Imports osi.root.utt

' TODO
Public Class udp_listener_test
    Inherits event_comb_case

    Private ReadOnly port As UInt16

    Public Sub New()
        port = rnd_port()
    End Sub

    Public Overrides Function create() As event_comb
        Return event_comb.succeeded()
    End Function
End Class
