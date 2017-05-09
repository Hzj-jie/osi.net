
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public MustInherit Class default_functor
        Implements functor

        Public MustOverride Function is_valid_port(ByVal id As PORT_T) As Boolean Implements functor.is_valid_port
        Public MustOverride Function accept_new_component(
                ByVal p As PARAMETER_T) As Boolean Implements functor.accept_new_component
        Public MustOverride Function create_component(
                ByVal p As PARAMETER_T,
                ByVal id As PORT_T,
                ByRef o As COMPONENT_T) As Boolean Implements functor.create_component
        Public MustOverride Sub dispose_component(ByVal c As COMPONENT_T) Implements functor.dispose_component

        Protected Overridable Function sense_timeout_ms(ByVal p As PARAMETER_T) As Int64
            Return constants.default_sense_timeout_ms
        End Function

        Public Overridable Function sense_timeout_ms(ByVal p As PARAMETER_T,
                                                     ByVal id As PORT_T) As Int64 Implements functor.sense_timeout_ms
            Return sense_timeout_ms(p)
        End Function

        Public Overridable Function create_dispenser(
                ByVal p As PARAMETER_T,
                ByVal id As PORT_T,
                ByVal dev As ref_instance(Of COMPONENT_T),
                ByRef o As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) _
                As Boolean Implements functor.create_dispenser
            Dim r As shared_receiver = Nothing
            If create_receiver(p, id, dev, r) Then
                o = selector.dispenser.[New](r, sense_timeout_ms(p, id))
                Return True
            Else
                Return False
            End If
        End Function

        Protected Overridable Function create_receiver(ByVal dev As ref_instance(Of COMPONENT_T)) As shared_receiver
            assert(False)
            Return Nothing
        End Function

        Protected Overridable Function create_receiver(ByVal dev As ref_instance(Of COMPONENT_T),
                                                       ByRef o As shared_receiver) As Boolean
            o = create_receiver(dev)
            Return Not o Is Nothing
        End Function

        Protected Overridable Function create_receiver(ByVal p As PARAMETER_T,
                                                       ByVal dev As ref_instance(Of COMPONENT_T),
                                                       ByRef o As shared_receiver) As Boolean
            Return create_receiver(dev, o)
        End Function

        Public Overridable Function create_receiver(ByVal p As PARAMETER_T,
                                                    ByVal id As PORT_T,
                                                    ByVal dev As ref_instance(Of COMPONENT_T),
                                                    ByRef o As shared_receiver) As Boolean _
                                                   Implements functor.create_receiver
            Return create_receiver(p, dev, o)
        End Function

        Protected Overridable Function create_sender(
                ByVal dev As ref_instance(Of COMPONENT_T),
                ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) As exclusive_sender
            assert(False)
            Return Nothing
        End Function

        Protected Overridable Function create_sender(ByVal dev As ref_instance(Of COMPONENT_T),
                                                     ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                                                     ByRef o As exclusive_sender) As Boolean
            o = create_sender(dev, remote)
            Return True
        End Function

        Protected Overridable Function create_sender(ByVal id As PORT_T,
                                                     ByVal dev As ref_instance(Of COMPONENT_T),
                                                     ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                                                     ByRef o As exclusive_sender) As Boolean
            Return create_sender(dev, remote, o)
        End Function

        Public Overridable Function create_sender(ByVal p As PARAMETER_T,
                                                  ByVal id As PORT_T,
                                                  ByVal dev As ref_instance(Of COMPONENT_T),
                                                  ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                                                  ByRef o As exclusive_sender) As Boolean _
                                                 Implements functor.create_sender
            Return create_sender(id, dev, remote, o)
        End Function

        Public Overridable Function new_accepter(ByVal p As PARAMETER_T,
                                                 ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) _
                                                As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter _
                                                Implements functor.new_accepter
            Return New default_accepter(remote)
        End Function

        Public Overridable Function new_shared_component(
                ByVal p As PARAMETER_T,
                ByVal id As PORT_T,
                ByVal component As ref_instance(Of COMPONENT_T),
                ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                ByVal remote As const_pair(Of ADDRESS_T, PORT_T),
                ByVal functor As functor,
                ByVal buff As DATA_T) _
                As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                Implements functor.new_shared_component
            Return creator.[New]().
                       with_parameter(p).
                       with_local_port(id).
                       with_component_ref(component).
                       with_dispenser(dispenser).
                       with_remote(remote).
                       with_data(buff).
                       with_functor(functor).
                       create()
        End Function
    End Class
End Class
