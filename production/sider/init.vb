
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.configuration
Imports osi.service.device
Imports osi.service.selector
Imports osi.service.storage
Imports osi.service.commander
Imports configuration = osi.service.storage.configuration

Public Module _init
    Private Const env_start_str As String = "%"
    Private Const env_end_str As String = "%"
    Private ReadOnly executor As executor

    Sub New()
        Dim dispatcher As istrkeyvt_dispatcher = Nothing
        dispatcher = New istrkeyvt_dispatcher()
        executor = ++dispatcher
        AddHandler control_c.press, AddressOf dispatcher.ignore
        assert(Not characters.default.preserved_str(env_start_str))
        assert(Not characters.default.preserved_str(env_end_str))
    End Sub

    Private Sub load_preserved_disk()
        Dim s As section = Nothing
        s = config("preserved_disk")
        If Not s Is Nothing Then
            Dim keys As vector(Of String) = Nothing
            keys = s.keys()
            If Not keys Is Nothing AndAlso
               Not keys.empty() Then
                For i As UInt32 = 0 To keys.size() - uint32_1
                    Dim v As UInt64 = 0
                    v = s(keys(i)).to_uint64()
                    configuration.preserved_disk_capacity(keys(i)) = v
                Next
            End If
        End If
    End Sub

    Public Function load_istrkeyvt(ByVal s As section,
                                   Optional ByVal r As pointer(Of istrkeyvt) = Nothing) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  assert(Not s Is Nothing)
                                  r.renew()
                                  ec = async_device_constructor.resolve(
                                           New var(s.values().env_transform(env_start_str, env_end_str)),
                                           r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not r.empty() Then
                                      application_lifetime.stopping_handle(AddressOf (+r).stop)
                                      Return manager.register(New var(s.values()), +r) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Sub load_istrkeyvt()
        Dim ss As vector(Of section) = Nothing
        ss = config.sections("istrkeyvt")
        assert(Not ss Is Nothing)
        If Not ss.empty() Then
            Dim loaded As atomic_int = Nothing
            loaded = New atomic_int()
            Using New scoped_environments({{"data", data_folder},
                                             {"service", service_name},
                                             {"service_data", service_data_folder}})
                For i As UInt32 = 0 To ss.size() - uint32_1
                    Dim ec As event_comb = Nothing
                    Dim s As section = Nothing
                    s = ss(i)
                    assert_begin(New event_comb(Function() As Boolean
                                                    ec = load_istrkeyvt(s)
                                                    Return waitfor(ec) AndAlso
                                                           goto_next()
                                                End Function,
                                                Function() As Boolean
                                                assert(ec.end_result())
                                                loaded.increment()
                                                Return goto_end()
                                            End Function))
                Next
                timeslice_sleep_wait_until(Function() (+loaded) = ss.size())
            End Using
        End If
    End Sub

    Public Function load_server(ByVal s As section, ByVal o As pointer(Of responder)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  o.renew()
                                  ec = async_device_constructor.resolve(New var(s.values()),
                                                                        executor,
                                                                        application_lifetime.stopping_signal(),
                                                                        o)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert(Not +o Is Nothing) AndAlso
                                         -+o AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function load_server(ByVal s As section) As event_comb
        Return load_server(s, Nothing)
    End Function

    Private Sub load_server()
        Dim ss As vector(Of section) = Nothing
        ss = config.sections("server")
        assert(Not ss Is Nothing)
        If Not ss.empty() Then
            Dim loaded As atomic_int = Nothing
            loaded = New atomic_int()
            For i As UInt32 = 0 To ss.size() - uint32_1
                Dim s As section = Nothing
                s = ss(i)
                Dim ec As event_comb = Nothing
                assert_begin(New event_comb(Function() As Boolean
                                                ec = load_server(s)
                                                Return waitfor(ec) AndAlso
                                                       goto_next()
                                            End Function,
                                            Function() As Boolean
                                                assert(ec.end_result())
                                                loaded.increment()
                                                Return goto_end()
                                            End Function))
            Next
            timeslice_sleep_wait_until(Function() (+loaded) = ss.size())
        End If
    End Sub

    Public Sub init()
        load_preserved_disk()
        load_istrkeyvt()
        load_server()
    End Sub
End Module
