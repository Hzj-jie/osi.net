
#Const REGISTER_RAW_DEVICE = True
Imports osi.root.constants
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.selector

<global_init(global_init_level.services)>
Public NotInheritable Class adapter_registry
    Public Shared Sub register(Of T1, T2)(ByVal type_name As String,
                                          ByVal secondary_type_name As String,
                                          ByVal adapter_new As Func(Of T1, T2))
        assert(Not adapter_new Is Nothing)
#If REGISTER_RAW_DEVICE Then
        assert(constructor.register(type_name,
                                    Function(v As var, ByRef o As T2) As Boolean
                                        Return secondary_resolve(v,
                                                                 secondary_type_name,
                                                                 Function(f As T1) As T2
                                                                     Return adapter_new(f)
                                                                 End Function,
                                                                 o)
                                    End Function))
#End If
        assert(constructor.register(type_name,
                                    Function(v As var, ByRef o As idevice(Of T2)) As Boolean
                                        Return secondary_resolve(
                                                   v,
                                                   secondary_type_name,
                                                   Function(f As idevice(Of T1)) As idevice(Of T2)
                                                       ' VS 2010
                                                       Return device_adapter.[New](Of T1, T2)(f, adapter_new)
                                                   End Function,
                                                   o)
                                    End Function))
        assert(constructor.register(type_name,
                                    Function(v As var, ByRef o As idevice_creator(Of T2)) As Boolean
                                        Return secondary_resolve(
                                                   v,
                                                   secondary_type_name,
                                                   Function(f As idevice_creator(Of T1)) As idevice_creator(Of T2)
                                                       Return device_creator_adapter.[New](f, adapter_new)
                                                   End Function,
                                                   o)
                                    End Function))
    End Sub

    Shared Sub New()
        register(Of flow, block)(constants.flow_block_adapter_type,
                                 constants.flow_secondary_type_name,
                                 AddressOf flow_block_adapter.[New])
        register(Of block, flow)(constants.block_flow_adapter_type,
                                 constants.block_secondary_type_name,
                                 AddressOf block_flow_adapter.[New])
        register(Of flow, datagram)(constants.flow_datagram_adapter_type,
                                    constants.flow_secondary_type_name,
                                    AddressOf flow_datagram_adapter.[New])
        register(Of datagram, flow)(constants.datagram_flow_adapter_type,
                                    constants.datagram_secondary_type_name,
                                    AddressOf datagram_flow_adapter.[New])
        register(Of block, piece_dev)(constants.block_piece_dev_adapter_type,
                                      constants.block_secondary_type_name,
                                      AddressOf block_piece_dev_adapter.[New])
        register(Of piece_dev, block)(constants.piece_dev_block_adapter_type,
                                      constants.piece_dev_secondary_type_name,
                                      AddressOf piece_dev_block_adapter.[New])
        register(Of flow, piece_dev)(constants.flow_piece_dev_adapter_type,
                                     constants.flow_secondary_type_name,
                                     AddressOf flow_piece_dev_adapter.[New])
        register(Of piece_dev, flow)(constants.piece_dev_flow_adapter_type,
                                     constants.piece_dev_secondary_type_name,
                                     AddressOf piece_dev_flow_adapter.[New])
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class registry(Of T)
    Private Shared Function identity(ByVal v As var) As String
        assert(Not v Is Nothing)
        Const s As String = "identity"
        v.bind(s)
        Return v(s)
    End Function

    Private Shared Function max_count(ByVal v As var) As UInt32
        assert(Not v Is Nothing)
        Const s As String = "max-count"
        v.bind(s)
        Return v(s).to_uint32()
    End Function

    Private Shared Sub attach_checker(ByVal v As var, ByVal d As device_pool(Of T))
        assert(Not v Is Nothing)
        Const s As String = "check-interval-ms"
        v.bind(s)
        Dim o As Int64 = int64_0
        If v.value(s, o) AndAlso o > 0 Then
            assert(d.attach_checker(o))
        End If
    End Sub

    Public Shared ReadOnly bypass As Boolean

    Shared Sub New()
        bypass = GetType(T).is(GetType(idevice_creator(Of ))) OrElse
                 GetType(T).is(GetType(idevice_exporter(Of ))) OrElse
                 GetType(T).is(GetType(idevice(Of ))) OrElse
                 GetType(T).is(GetType(idevice_pool(Of )))
        If Not bypass Then
            assert(wrapper.register(
                       Function(v As var, i As idevice(Of T), ByRef o As idevice(Of T)) As Boolean
                           Return device_adapter.wrap(v, i, o)
                       End Function))
            assert(wrapper.register(
                       Function(v As var, i As idevice_creator(Of T), ByRef o As idevice_creator(Of T)) As Boolean
                           o = New wrappered_device_creator(Of T)(i, v)
                           Return True
                       End Function))

            If async_device_device_converter(Of T).registered() Then
                assert(constructor.register(
                           Function(v As var, ByRef o As idevice_creator(Of T)) As Boolean
                               Dim c As iasync_device_creator(Of T) = Nothing
                               If constructor.resolve(v, c) Then
                                   o = device_creator_adapter.[New](c, async_device_device_converter(Of T).[default]())
                                   Return True
                               Else
                                   Return False
                               End If
                           End Function))
            End If
            assert(constructor.register(
                       Function(v As var, ByRef o As iauto_device_exporter(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Dim x As idevice_creator(Of T) = Nothing
                           Dim y As iasync_device_creator(Of T) = Nothing
                           If constructor.resolve(v, x) OrElse constructor.resolve(v, y) Then
                               Const check_interval_ms As String = "check-interval-ms"
                               Const failure_wait_ms As String = "failure-wait-ms"
                               Const max_concurrent_generations As String = "max-concurrent-generations"
                               v.bind(check_interval_ms, failure_wait_ms, max_concurrent_generations)
                               Dim cms As Int64 = uint64_0
                               Dim fms As Int64 = uint64_0
                               Dim mc As Int32 = 0
                               cms = v(check_interval_ms).to_int64(constants.default_auto_generation_check_interval_ms)
                               fms = v(failure_wait_ms).to_int64(constants.default_auto_generation_failure_wait_ms)
                               mc = v(max_concurrent_generations).to_int32(
                                        constants.default_auto_generation_max_concurrent_generations)
                               If Not x Is Nothing Then
                                   o = auto_device_exporter.[New](x, cms, fms, mc)
                               ElseIf Not y Is Nothing Then
                                   o = auto_device_exporter.[New](y, cms, fms, mc)
                               Else
                                   assert(False)
                                   Return False
                               End If
                               Return True
                           Else
                               Return False
                           End If
                       End Function))

            assert(constructor.register(
                       Function(v As var, ByRef o As auto_pre_generated_device_pool(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Dim d As iauto_device_exporter(Of T) = Nothing
                           If constructor.resolve(v, d) Then
                               o = auto_pre_generated_device_pool.[New](d, max_count(v), identity(v))
                               attach_checker(v, o)
                               Return True
                           Else
                               Return False
                           End If
                       End Function))

            assert(constructor.register(
                       Function(v As var, ByRef o As delay_generate_device_pool(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Dim d As idevice_creator(Of T) = Nothing
                           If constructor.resolve(v, d) Then
                               o = delay_generate_device_pool.[New](d, max_count(v), identity(v))
                               attach_checker(v, o)
                               Return True
                           Else
                               Return False
                           End If
                       End Function))

            assert(constructor.register(
                       Function(v As var, ByRef o As manual_pre_generated_device_pool(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Dim i As imanual_device_exporter(Of T) = Nothing
                           If constructor.resolve(v, i) Then
                               o = New manual_pre_generated_device_pool(Of T)(i, max_count(v), identity(v))
                               attach_checker(v, o)
                               Return True
                           Else
                               Return False
                           End If
                           Return True
                       End Function))

            assert(constructor.register(
                       Function(v As var, ByRef o As one_off_device_pool(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Dim d As idevice_creator(Of T) = Nothing
                           If constructor.resolve(v, d) Then
                               o = one_off_device_pool.[New](d, max_count(v), identity(v))
                               Return True
                           Else
                               Return False
                           End If
                       End Function))

            assert(constructor.register(
                       Function(v As var, ByRef o As singleton_device_pool(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Dim d As idevice(Of T) = Nothing
                           If constructor.resolve(v, d) Then
                               o = singleton_device_pool.[New](d, identity(v))
                               Return True
                           Else
                               Return False
                           End If
                       End Function))

            assert(constructor.register(
                       Function(v As var, ByRef o As idevice_pool(Of T)) As Boolean
                           If v Is Nothing Then
                               Return False
                           End If

                           Const pool_type As String = "pool-type"
                           v.bind(pool_type)
                           Dim pt As String = Nothing
                           pt = v(pool_type)

                           If strsame(pt, "auto-pre-generated", False) Then
                               Return constructor(Of idevice_pool(Of T)) _
                                          .resolve(Of auto_pre_generated_device_pool(Of T))(v, o)
                           ElseIf strsame(pt, "delay-generate", False) Then
                               Return constructor(Of idevice_pool(Of T)) _
                                          .resolve(Of delay_generate_device_pool(Of T))(v, o)
                           ElseIf strsame(pt, "manual-pre-generated", False) Then
                               Return constructor(Of idevice_pool(Of T)) _
                                          .resolve(Of manual_pre_generated_device_pool(Of T))(v, o)
                           ElseIf strsame(pt, "one-off", False) Then
                               Return constructor(Of idevice_pool(Of T)).resolve(Of one_off_device_pool(Of T))(v, o)
                           ElseIf strsame(pt, "singleton", False) Then
                               Return constructor(Of idevice_pool(Of T)).resolve(Of singleton_device_pool(Of T))(v, o)
                           Else
                               Return False
                           End If
                       End Function))
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
