
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.transmitter
Imports osi.service.selector

Partial Public Class shared_component_test
    Private Class collection
        Inherits shared_component(Of UInt16, UInt16, component, Int32, parameter).
                 collection(Of _max_uint16, _uint16_to_uint32)

        Protected Overrides Function accept_new_component(ByVal p As parameter) As Boolean
            assert_not_nothing(p)
            Return True
        End Function

        Protected Overrides Function create_component(ByVal p As parameter,
                                                      ByVal id As UInt16,
                                                      ByRef o As component) As Boolean
            assert_not_nothing(p)
            If is_valid_port(p.port) Then
                assert_equal(p.port, id)
            End If
            o = New component(id)
            Return True
        End Function

        Protected Overrides Function create_receiver(ByVal p As parameter,
                                                     ByVal id As UInt16,
                                                     ByVal dev As ref_instance(Of component),
                                                     ByRef o As T_receiver(Of pair(Of Int32,
                                                                                      const_pair(Of UInt16, UInt16)))) _
                                                    As Boolean
            assert_not_nothing(p)
            If is_valid_port(p.port) Then
                assert_equal(p.port, id)
            End If
            o = New receiver(dev)
            Return True
        End Function

        Protected Overrides Function is_valid_port(ByVal id As UInt16) As Boolean
            Return component.is_valid_port(id)
        End Function

        Protected Overrides Function port(ByVal p As parameter) As UInt16
            assert(Not p Is Nothing)
            Return p.port
        End Function

        Protected Overrides Sub dispose_component(ByVal p As component)
            assert(Not p Is Nothing)
            p.dispose()
        End Sub
    End Class
End Class
