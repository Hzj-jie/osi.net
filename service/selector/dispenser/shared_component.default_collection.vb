
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class shared_component(Of RESOURCE_ID_T, REMOTE_ID_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public MustInherit Class collection(Of _RESOURCE_COUNT As _int64,
                                           _RESOURCE_ID_TO_UINT32 As _to_uint32(Of RESOURCE_ID_T),
                                           SHARED_COMPONENT_IMPL_T As shared_component(Of RESOURCE_ID_T,
                                                                                          REMOTE_ID_T,
                                                                                          COMPONENT_T,
                                                                                          DATA_T,
                                                                                          PARAMETER_T))
        Implements collection, exporter(Of SHARED_COMPONENT_IMPL_T)

        Private Shared ReadOnly resource_count As Int64
        Private Shared ReadOnly resource_id_to_uint32 As _to_uint32(Of RESOURCE_ID_T)
        Private ReadOnly devs As arrayless(Of ref_instance(Of COMPONENT_T))
        Private ReadOnly dispensers As arrayless(Of dispenser(Of DATA_T, endpoint))
        Public Event new_shared_component_exported(ByVal d As SHARED_COMPONENT_IMPL_T) _
                         Implements exporter(Of SHARED_COMPONENT_IMPL_T).new_shared_component_exported

        Shared Sub New()
            resource_id_to_uint32 = alloc(Of _RESOURCE_ID_TO_UINT32)()
            assert(Not resource_id_to_uint32 Is Nothing)
            resource_count = +alloc(Of _RESOURCE_COUNT)()
            assert(resource_count > 0)
            assert(resource_count <= max_uint32)
        End Sub

        Public Sub New()
            devs = New arrayless(Of ref_instance(Of COMPONENT_T))(resource_count)
            dispensers = New arrayless(Of dispenser(Of DATA_T, endpoint))(resource_count)
        End Sub

        Protected MustOverride Function create_component(ByVal p As PARAMETER_T,
                                                         ByVal id As RESOURCE_ID_T,
                                                         ByRef o As COMPONENT_T) As Boolean
        Protected MustOverride Function is_valid_resource_id(ByVal id As RESOURCE_ID_T) As Boolean
        Protected MustOverride Function parameter_id(ByVal p As PARAMETER_T) As RESOURCE_ID_T
        Protected MustOverride Function create_dispenser(ByVal p As PARAMETER_T,
                                                         ByVal id As RESOURCE_ID_T,
                                                         ByVal dev As ref_instance(Of COMPONENT_T),
                                                         ByRef o As dispenser(Of DATA_T, endpoint)) As Boolean
        Protected MustOverride Function accept_new_component(ByVal p As PARAMETER_T) As Boolean
        Protected MustOverride Function new_shared_component(ByVal p As PARAMETER_T,
                                                             ByVal d As COMPONENT_T,
                                                             ByVal buff As DATA_T,
                                                             ByVal remote As endpoint) As SHARED_COMPONENT_IMPL_T

        Private Function new_component(ByVal p As PARAMETER_T, ByVal id As RESOURCE_ID_T) As ref_instance(Of COMPONENT_T)
            Return ref_instance.[New](Function() As COMPONENT_T
                                          Dim x As COMPONENT_T = Nothing
                                          If create_component(p, id, x) Then
                                              Return x
                                          Else
                                              Return Nothing
                                          End If
                                      End Function,
                                      0)
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByRef local_resource_id As RESOURCE_ID_T,
                              ByRef o As ref_instance(Of COMPONENT_T)) As Boolean Implements collection.New
            If is_valid_resource_id(parameter_id(p)) Then
                If Not devs.[New](resource_id_to_uint32(local_resource_id),
                                  Function() new_component(p, parameter_id(p)),
                                  o) Then
                    Return False
                End If
                local_resource_id = parameter_id(p)
            Else
                Dim id As UInt32 = 0
                If Not devs.next(Function(ByVal current_id As UInt32) _
                           new_component(p, resource_id_to_uint32.reverse(current_id)), id, o) Then
                    Return False
                End If
                local_resource_id = resource_id_to_uint32.reverse(id)
            End If
            assert(Not o Is Nothing)
            Return True
        End Function

        Public Function [New](ByVal p As PARAMETER_T,
                              ByVal local_resource_id As RESOURCE_ID_T,
                              ByVal i As ref_instance(Of COMPONENT_T),
                              ByRef dispenser As dispenser(Of DATA_T, endpoint)) As Boolean Implements collection.New
            Return dispensers.[New](resource_id_to_uint32(local_resource_id),
                                    Function(ByVal current_id As UInt32) As dispenser(Of DATA_T, endpoint)
                                        Dim x As dispenser(Of DATA_T, endpoint) = Nothing
                                        If create_dispenser(p, resource_id_to_uint32.reverse(current_id), i, x) Then
                                            If accept_new_component(p) Then
                                                assert(is_valid_resource_id(parameter_id(p)))
                                                assert(compare(parameter_id(p), local_resource_id) = 0)
                                                AddHandler x.before_init, Sub()
                                                                              i.ref()
                                                                          End Sub
                                                AddHandler x.after_final, Sub()
                                                                              i.unref()
                                                                          End Sub
                                                AddHandler x.unaccepted,
                                                           Sub(ByVal buff As DATA_T, ByVal remote As endpoint)
                                                               Dim d As COMPONENT_T = Nothing
                                                               d = +i
                                                               If Not d Is Nothing Then
                                                                   RaiseEvent new_shared_component_exported(
                                                                              new_shared_component(p, d, buff, remote))
                                                               End If
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
