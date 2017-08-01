
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.transmitter

' Send data to one specific receiver identified by remote address and port.
Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public MustInherit Class exclusive_sender
        Implements T_sender(Of DATA_T)

        Protected ReadOnly remote As const_pair(Of ADDRESS_T, PORT_T)
        Private ReadOnly c As ref_instance(Of COMPONENT_T)

        Public Sub New(ByVal c As ref_instance(Of COMPONENT_T), ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
            assert(Not c Is Nothing)
            assert(Not remote Is Nothing)
            Me.c = c
            Me.remote = remote
        End Sub

        Protected Function component() As COMPONENT_T
            assert(c.referred())
            Return +c
        End Function

        Protected Function referred() As Boolean
            Return c.referred()
        End Function

        Protected Function component_getter() As getter(Of COMPONENT_T)
            Return not_null_getter.[New](c.assert_getter())
        End Function

        Protected Function address() As ADDRESS_T
            Return remote.first
        End Function

        Protected Function port() As PORT_T
            Return remote.second
        End Function

        Public MustOverride Function send(ByVal i As DATA_T) As event_comb Implements T_injector(Of DATA_T).send
    End Class
End Class
