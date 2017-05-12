
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private NotInheritable Class functor
        Inherits shared_component(Of Byte, Byte, component, Int32, parameter).default_functor

        Public Shared address As Byte

        Public Overrides Sub dispose_component(ByVal c As component)
            assert_not_nothing(c)
            c.dispose()
        End Sub

        Public Overrides Function accept_new_component(ByVal p As parameter) As Boolean
            assert_not_nothing(p)
            Return p.allow_new_component
        End Function

        Public Overrides Function create_component(ByVal p As parameter,
                                                   ByVal id As Byte,
                                                   ByRef o As component) As Boolean
            assert_not_nothing(p)
            o = New component(address, id)
            Return True
        End Function

        Public Overrides Function is_valid_port(ByVal id As Byte) As Boolean
            Return component.is_valid_port(id)
        End Function

        Public Overrides Function create_receiver(
                ByVal p As parameter,
                ByVal id As Byte,
                ByVal dev As ref_instance(Of component),
                ByRef o As shared_component(Of Byte, Byte, component, Int32, parameter).shared_receiver) As Boolean
            assert_not_nothing(p)
            o = New shared_receiver(dev)
            Return True
        End Function

        Protected Overrides Function create_sender(
                ByVal dev As ref_instance(Of component),
                ByVal remote As const_pair(Of Byte, Byte)) _
                As shared_component(Of Byte, Byte, component, Int32, parameter).exclusive_sender
            Return New exclusive_sender(dev, remote)
        End Function
    End Class
End Class
