
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private NotInheritable Class functor
        Inherits shared_component(Of UInt16, UInt16, component, Int32, parameter).default_functor

        Public Overrides Sub dispose_component(ByVal c As component)
            assert_not_nothing(c)
            c.dispose()
        End Sub

        Public Overrides Function accept_new_component(ByVal p As parameter) As Boolean
            assert_not_nothing(p)
            Return p.allow_new_component
        End Function

        Public Overrides Function create_component(ByVal p As parameter,
                                                   ByVal id As UInt16,
                                                   ByRef o As component) As Boolean
            assert_not_nothing(p)
            o = New component(id)
            Return True
        End Function

        Public Overrides Function is_valid_port(ByVal id As UInt16) As Boolean
            Return component.is_valid_port(id)
        End Function

        Public Overrides Function create_receiver(
                ByVal p As parameter,
                ByVal id As UInt16,
                ByVal dev As ref_instance(Of component),
                ByRef o As shared_component(Of UInt16, UInt16, component, Int32, parameter).shared_receiver) As Boolean
            assert_not_nothing(p)
            o = New shared_receiver(dev)
            Return True
        End Function

        Protected Overrides Function create_sender(
                ByVal id As UInt16,
                ByVal dev As ref_instance(Of component),
                ByVal remote As const_pair(Of UInt16, UInt16)) _
                As shared_component(Of UInt16, UInt16, component, Int32, parameter).exclusive_sender
            Return New exclusive_sender(dev, remote)
        End Function
    End Class
End Class
