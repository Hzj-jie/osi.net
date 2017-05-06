
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.transmitter

' Receive data from one COMPONENT_T identified by local port.
Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public MustInherit Class shared_receiver
        Implements T_receiver(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))

        Private ReadOnly c As ref_instance(Of COMPONENT_T)

        Public Sub New(ByVal c As ref_instance(Of COMPONENT_T))
            assert(Not c Is Nothing)
            Me.c = c
        End Sub

        Protected Function component() As COMPONENT_T
            assert(c.referred())
            Return +c
        End Function

        Protected Function referred() As Boolean
            Return c.referred()
        End Function

        Protected Function component_getter() As getter(Of COMPONENT_T)
            Return c.assert_getter()
        End Function

        Public MustOverride Function receive(
                ByVal o As pointer(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))) As event_comb _
                Implements T_pump(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))).receive
        Public MustOverride Function sense(ByVal pending As pointer(Of Boolean),
                                           ByVal timeout_ms As Int64) As event_comb Implements sensor.sense
    End Class
End Class
