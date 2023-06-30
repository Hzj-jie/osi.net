
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.selector

Partial Public Class sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public Class collection(Of _RESOURCE_COUNT As _int64,
                               _PORT_TO_UINT32 As _to_uint32(Of PORT_T),
                               _FUNCTOR As functor)
        Implements collection, exporter(Of sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T))

        Private Shared ReadOnly resource_count As UInt32
        Private Shared ReadOnly port_to_uint32 As _to_uint32(Of PORT_T)
        Private Shared ReadOnly functor As functor
        Private ReadOnly devs As arrayless(Of ref_instance(Of COMPONENT_T))
        Private ReadOnly dispensers As arrayless(Of dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))
        Public Event new_sharedtransmitter_exported(
                ByVal d As sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)) _
                Implements exporter(Of sharedtransmitter(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)) _
                .new_sharedtransmitter_exported

        Shared Sub New()
            port_to_uint32 = alloc(Of _PORT_TO_UINT32)()
            assert(Not port_to_uint32 Is Nothing)
            resource_count = alloc(Of _RESOURCE_COUNT)().as_uint32()
            functor = alloc(Of _FUNCTOR)()
            assert(Not functor Is Nothing)
        End Sub

        Public Sub New()
            devs = New arrayless(Of ref_instance(Of COMPONENT_T))(resource_count)
            dispensers = New arrayless(Of dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)))(resource_count)
        End Sub

        Private Function new_component(ByVal p As PARAMETER_T, ByVal id As PORT_T) As ref_instance(Of COMPONENT_T)
            Return ref_instance.[New](Function() As COMPONENT_T
                                          Dim x As COMPONENT_T = Nothing
                                          If functor.create_component(p, id, x) Then
                                              Return x
                                          Else
                                              Return Nothing
                                          End If
                                      End Function,
                                      0,
                                      AddressOf functor.dispose_component)
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByRef local_port As PORT_T,
                              ByRef o As ref_instance(Of COMPONENT_T)) As Boolean Implements collection.New
            If functor.is_valid_port(local_port) Then
                Dim lp As PORT_T = Nothing
                lp = local_port
                If Not devs.[New](port_to_uint32(local_port),
                                  Function() new_component(p, lp),
                                  o) Then
                    Return False
                End If
            Else
                Dim id As UInteger = 0
                If Not devs.next(Function(ByVal current_id As UInteger) As ref_instance(Of COMPONENT_T)
                                     Dim port As PORT_T = Nothing
                                     port = port_to_uint32.reverse(current_id)
                                     If functor.is_valid_port(port) Then
                                         Return new_component(p, port)
                                     Else
                                         Return Nothing
                                     End If
                                 End Function,
                                 id,
                                 o) Then
                    Return False
                End If
                local_port = port_to_uint32.reverse(id)
            End If
            assert(Not o Is Nothing)
            Return True
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByVal local_port As PORT_T,
                              ByVal i As ref_instance(Of COMPONENT_T),
                              ByRef dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) As Boolean _
                             Implements collection.New
            Return dispensers.[New](port_to_uint32(local_port),
                                    Function(ByVal current_id As UInt32) _
                                            As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))
                                        Dim x As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)) = Nothing
                                        If functor.create_dispenser(p, port_to_uint32.reverse(current_id), i, x) Then
                                            AddHandler x.before_init, Sub()
                                                                          i.ref()
                                                                      End Sub
                                            AddHandler x.after_final, Sub()
                                                                          i.unref()
                                                                      End Sub
                                            ' Usually it's not reasonable to accept new connections from a random port.
                                            If functor.accept_new_component(p) AndAlso
                                               functor.is_valid_port(local_port) Then
                                                AddHandler x.unaccepted,
                                                           Sub(ByVal buff As DATA_T,
                                                               ByVal remote As const_pair(Of ADDRESS_T, PORT_T))
                                                               RaiseEvent new_sharedtransmitter_exported(
                                                                       functor.new_sharedtransmitter(p,
                                                                                                    local_port,
                                                                                                    i,
                                                                                                    x,
                                                                                                    remote,
                                                                                                    functor,
                                                                                                    buff))
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
