
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.transmitter

Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public MustInherit Class collection(Of _RESOURCE_COUNT As _int64,
                                           _PORT_TO_UINT32 As _to_uint32(Of PORT_T),
                                           SHARED_COMPONENT_IMPL_T As shared_component(Of PORT_T,
                                                                                          ADDRESS_T,
                                                                                          COMPONENT_T,
                                                                                          DATA_T,
                                                                                          PARAMETER_T))
        Implements collection, exporter(Of SHARED_COMPONENT_IMPL_T)

        Private Shared ReadOnly resource_count As Int64
        Private Shared ReadOnly port_To_uint32 As _to_uint32(Of PORT_T)
        Private ReadOnly devs As arrayless(Of ref_instance(Of COMPONENT_T))
        Private ReadOnly dispensers As arrayless(Of dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))
        Public Event new_shared_component_exported(ByVal d As SHARED_COMPONENT_IMPL_T) _
                         Implements exporter(Of SHARED_COMPONENT_IMPL_T).new_shared_component_exported

        Shared Sub New()
            port_To_uint32 = alloc(Of _PORT_TO_UINT32)()
            assert(Not port_To_uint32 Is Nothing)
            resource_count = +alloc(Of _RESOURCE_COUNT)()
            assert(resource_count > 0)
            assert(resource_count <= max_uint32)
        End Sub

        Public Sub New()
            devs = New arrayless(Of ref_instance(Of COMPONENT_T))(resource_count)
            dispensers = New arrayless(Of dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))(resource_count)
        End Sub

        Protected MustOverride Function create_component(ByVal p As PARAMETER_T,
                                                         ByVal id As PORT_T,
                                                         ByRef o As COMPONENT_T) As Boolean
        Protected MustOverride Function is_valid_port(ByVal id As PORT_T) As Boolean
        Protected MustOverride Function local_port(ByVal p As PARAMETER_T) As PORT_T
        Protected MustOverride Function accept_new_component(ByVal p As PARAMETER_T) As Boolean
        Protected MustOverride Sub dispose_component(ByVal c As COMPONENT_T)

        Protected Overridable Function new_accepter(ByVal p As PARAMETER_T,
                                                    ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) _
                                                   As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter
            Return New default_accepter(remote)
        End Function

        Protected Overridable Function new_shared_component(
                 ByVal p As PARAMETER_T,
                 ByVal id As PORT_T,
                 ByVal component As ref_instance(Of COMPONENT_T),
                 ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                 ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter,
                 ByVal buff As DATA_T) As SHARED_COMPONENT_IMPL_T
            assert(False)
            Return Nothing
        End Function

        Protected Overridable Function new_shared_component(
                 ByVal p As PARAMETER_T,
                 ByVal id As PORT_T,
                 ByVal component As ref_instance(Of COMPONENT_T),
                 ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)),
                 ByVal buff As DATA_T,
                 ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) _
             As SHARED_COMPONENT_IMPL_T
            Dim accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter = Nothing
            accepter = new_accepter(p, remote)
            Return new_shared_component(p, id, component, dispenser, accepter, buff)
        End Function

        Protected Overridable Function sense_timeout_ms(ByVal p As PARAMETER_T, ByVal id As PORT_T) As Int64
            Return constants.default_sense_timeout_ms
        End Function

        Protected Overridable Function create_receiver(
               ByVal dev As ref_instance(Of COMPONENT_T)) _
               As T_receiver(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))
            assert(False)
            Return Nothing
        End Function

        Protected Overridable Function create_receiver(
                ByVal dev As ref_instance(Of COMPONENT_T),
                ByRef o As T_receiver(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))) As Boolean
            o = create_receiver(dev)
            Return Not o Is Nothing
        End Function

        Protected Overridable Function create_receiver(
                ByVal p As PARAMETER_T,
                ByVal dev As ref_instance(Of COMPONENT_T),
                ByRef o As T_receiver(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))) As Boolean
            Return create_receiver(dev, o)
        End Function

        Protected Overridable Function create_receiver(
                ByVal p As PARAMETER_T,
                ByVal id As PORT_T,
                ByVal dev As ref_instance(Of COMPONENT_T),
                ByRef o As T_receiver(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))) As Boolean
            Return create_receiver(p, dev, o)
        End Function

        Protected Overridable Function create_dispenser(ByVal p As PARAMETER_T,
                                                        ByVal id As PORT_T,
                                                        ByVal dev As ref_instance(Of COMPONENT_T),
                                                        ByRef o As dispenser(Of DATA_T,
                                                                                const_pair(Of ADDRESS_T, PORT_T))) _
                                                       As Boolean
            Dim r As T_receiver(Of pair(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) = Nothing
            If create_receiver(p, id, dev, r) Then
                o = selector.dispenser.[New](r, sense_timeout_ms(p, id))
                Return True
            Else
                Return False
            End If
        End Function

        Private Function new_component(ByVal p As PARAMETER_T, ByVal id As PORT_T) As ref_instance(Of COMPONENT_T)
            Return ref_instance.[New](Function() As COMPONENT_T
                                          Dim x As COMPONENT_T = Nothing
                                          If create_component(p, id, x) Then
                                              Return x
                                          Else
                                              Return Nothing
                                          End If
                                      End Function,
                                      0,
                                      AddressOf dispose_component)
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByRef local_port As PORT_T,
                              ByRef o As ref_instance(Of COMPONENT_T)) As Boolean Implements collection.New
            If is_valid_port(Me.local_port(p)) Then
                If Not devs.[New](port_To_uint32(local_port),
                                  Function() new_component(p, Me.local_port(p)),
                                  o) Then
                    Return False
                End If
                local_port = Me.local_port(p)
            Else
                Dim id As UInteger = 0
                If Not devs.next(Function(ByVal current_id As UInteger) As ref_instance(Of COMPONENT_T)
                                     Dim port As PORT_T = Nothing
                                     port = port_To_uint32.reverse(current_id)
                                     If is_valid_port(port) Then
                                         Return new_component(p, port)
                                     Else
                                         Return Nothing
                                     End If
                                 End Function,
                                 id,
                                 o) Then
                    Return False
                End If
                local_port = port_To_uint32.reverse(id)
            End If
            assert(Not o Is Nothing)
            Return True
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByVal local_port As PORT_T,
                              ByVal i As ref_instance(Of COMPONENT_T),
                              ByRef dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) As Boolean _
                             Implements collection.New
            Return dispensers.[New](port_To_uint32(local_port),
                                    Function(ByVal current_id As UInt32) _
                                            As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))
                                        Dim x As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)) = Nothing
                                        If create_dispenser(p, port_To_uint32.reverse(current_id), i, x) Then
                                            AddHandler x.before_init, Sub()
                                                                          i.ref()
                                                                      End Sub
                                            AddHandler x.after_final, Sub()
                                                                          i.unref()
                                                                      End Sub
                                            ' Usually it's not reasonable to accept new connections from a random port.
                                            If accept_new_component(p) AndAlso is_valid_port(Me.local_port(p)) Then
                                                assert(compare(Me.local_port(p), local_port) = 0)
                                                AddHandler x.unaccepted,
                                                           Sub(ByVal buff As DATA_T,
                                                               ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
                                                               RaiseEvent new_shared_component_exported(
                                                                          new_shared_component(p,
                                                                                               local_port,
                                                                                               i,
                                                                                               x,
                                                                                               buff,
                                                                                               remote))
                                                           End Sub
                                                assert(x.bind())
                                            End If
                                            Return x
                                        Else
                                            Return Nothing
                                        End If
                                    End Function,
                                    dispenser)
        End Function
    End Class
End Class
