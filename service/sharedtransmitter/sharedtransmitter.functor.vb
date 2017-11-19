
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.service.selector

Partial Public Class sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public Interface functor
        Function is_valid_port(ByVal id As PORT_T) As Boolean
        Function accept_new_component(ByVal p As PARAMETER_T) As Boolean
        Function sense_timeout_ms(ByVal p As PARAMETER_T, ByVal id As PORT_T) As Int64
        Function create_component(ByVal p As PARAMETER_T,
                                  ByVal id As PORT_T,
                                  ByRef o As COMPONENT_T) As Boolean
        Sub dispose_component(ByVal c As COMPONENT_T)
        Function new_accepter(ByVal p As PARAMETER_T,
                              ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) _
                             As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter
        Function new_sharedtransmitter(
                ByVal p As PARAMETER_T,
                ByVal id As PORT_T,
                ByVal component As ref_instance(Of COMPONENT_T),
                ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                ByVal functor As functor,
                ByVal buff As DATA_T) _
                As sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
        Function create_receiver(ByVal p As PARAMETER_T,
                                 ByVal id As PORT_T,
                                 ByVal dev As ref_instance(Of COMPONENT_T),
                                 ByRef o As shared_receiver) As Boolean
        Function create_sender(ByVal p As PARAMETER_T,
                               ByVal id As PORT_T,
                               ByVal dev As ref_instance(Of COMPONENT_T),
                               ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                               ByRef o As exclusive_sender) As Boolean
        Function create_dispenser(ByVal p As PARAMETER_T,
                                  ByVal id As PORT_T,
                                  ByVal dev As ref_instance(Of COMPONENT_T),
                                  ByRef o As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) _
                                 As Boolean
    End Interface
End Class
