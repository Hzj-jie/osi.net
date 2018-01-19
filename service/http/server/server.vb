
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.http.constants.interval_ms
Imports constructor = osi.service.device.constructor

<global_init(global_init_level.server_services)>
Partial Public NotInheritable Class server
    Public Event context_received(ByVal ctx As context)

    Private ReadOnly max_connection_count As Int32
    Private ReadOnly ls As link_status
    Private ReadOnly encoder As Encoding

    Private ReadOnly listener As HttpListener
    Private ReadOnly cc As atomic_int

    Public NotInheritable Class configuration
        Public max_connection_count As Int32 = constants.default_value.max_connection_count
        Public ls As link_status = Nothing
        Public encoder As Encoding = Nothing
    End Class

    Shared Sub New()
        assert(HttpListener.IsSupported())
        ServicePointManager.DefaultConnectionLimit() = max_int32
        ServicePointManager.UseNagleAlgorithm() = False
        Try
            DirectCast(Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Nothing) _
                .GetSection("system.web/httpRuntime"),
                Web.Configuration.HttpRuntimeSection).EnableHeaderChecking() = False
        Catch ex As Exception
            raise_error(error_type.warning, "failed to set EnableHeaderChecking to false, ex ", ex.Message())
        End Try
    End Sub

    Public Sub New(ByVal c As configuration)
        If c Is Nothing Then
            c = New configuration()
        End If

        If c.max_connection_count <= 0 Then
            c.max_connection_count = max_int32
        End If
        max_connection_count = c.max_connection_count

        If c.ls Is Nothing Then
            c.ls = link_status.server
        End If
        ls = c.ls

        If c.encoder Is Nothing Then
            c.encoder = constants.default_value.encoder
        End If
        encoder = c.encoder

        cc = New atomic_int()
        listener = New HttpListener()
        listener.IgnoreWriteExceptions() = True
    End Sub

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Function prefixes_count() As Int32
        Return listener.Prefixes().Count()
    End Function

    Public Function has_prefixes() As Boolean
        Return prefixes_count() > 0
    End Function

    Public Function identity() As String
        Return strcat("http_server@", strjoin(character.and_mark, listener.Prefixes()))
    End Function

    Public Function connection_count() As Int32
        Return +cc
    End Function

    Public Function add_prefix(ByVal prefix As String) As Boolean
        Try
            listener.Prefixes().Add(prefix)
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation,
                        "failed to add prefix ", prefix, " to HttpListener, ex ", ex.Message)
            Return False
        End Try
    End Function

    Public Function add_port(ByVal port As UInt16) As Boolean
        Return add_prefix(constants.protocol_address_head.http +
                          character.plus_sign +
                          constants.uri.port_mark +
                          Convert.ToString(port) +
                          constants.uri.path_separator)
    End Function

    Public Function add_port(ByVal port As String) As Boolean
        Return add_port(port.to(Of UInt16)())
    End Function

    Private Function add_several(ByVal s As String, ByVal a As Func(Of String, Boolean)) As Boolean
        assert(Not a Is Nothing)
        Dim vs As vector(Of String) = Nothing
        If Not vs.split_from(s) Then
            Return False
        End If
        assert(Not vs.null_or_empty())
        For i As UInt32 = 0 To vs.size() - uint32_1
            If Not a(vs(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function add_prefixes(ByVal prefixes As String) As Boolean
        Return add_several(prefixes, AddressOf add_prefix)
    End Function

    Public Function add_ports(ByVal ports As String) As Boolean
        Return add_several(ports, AddressOf add_port)
    End Function

    Private Sub end_context(ByVal ctx As HttpListenerContext, ByVal abort As Boolean)
        assert(Not ctx Is Nothing)
        ctx.shutdown(abort)
        assert(cc.decrement() >= 0)
    End Sub

    Private Sub listen()
        For i As Int32 = 0 To min(CInt(thread_pool().thread_count()), Environment.ProcessorCount()) - 1
            Dim ctx As pointer(Of HttpListenerContext) = Nothing
            Dim ec As event_comb = Nothing
            assert_begin(New event_comb(Function() As Boolean
                                            If listener.IsListening() Then
                                                ec = Nothing
                                                ctx.renew()
                                                If cc.increment() <= max_connection_count Then
                                                    ec = listener.get_context(ctx)
                                                    Return waitfor(ec) AndAlso
                                                           goto_next()
                                                Else
                                                    assert(cc.decrement() >= 0)
                                                    Return waitfor(over_max_connection_count_wait_time) AndAlso
                                                           goto_next()
                                                End If
                                            Else
                                                Return goto_end()
                                            End If
                                        End Function,
                                        Function() As Boolean
                                            If Not ec Is Nothing AndAlso
                                               ec.end_result() AndAlso
                                               Not +ctx Is Nothing Then
                                                RaiseEvent context_received(New context(Me, +ctx, ls, encoder))
                                            Else
                                                assert(cc.decrement() >= 0)
                                            End If
                                            Return goto_prev()
                                        End Function))
        Next
    End Sub

    Public Function start() As Boolean
        If listener.IsListening() OrElse Not has_prefixes() Then
            Return False
        Else
            Try
                listener.Start()
            Catch ex As Exception
                raise_error(error_type.exclamation, "cannot start HttpListener, ex ", ex.Message())
                Return False
            End Try
            listen()
            Return True
        End If
    End Function

    Public Sub [stop](Optional ByVal wait_connections_ms As UInt32 = 0)
        If listener.IsListening() Then
            listener.Stop()
            listener.Prefixes().Clear()
            timeslice_sleep_wait_until(Function() connection_count() = 0, wait_connections_ms)
        End If
    End Sub

    Public Function alive() As Boolean
        Return listener.IsListening()
    End Function

    Protected Overrides Sub Finalize()
        listener.Close()
        MyBase.Finalize()
    End Sub

    Public Shared Function create(ByVal v As var, ByRef o As server) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Const p_ports As String = "ports"
            Const p_prefixes As String = "prefixes"
            Const p_max_connection_count As String = "max-connection-count"
            Const p_encoder As String = "encoder"
            v.bind(p_ports,
                   p_prefixes,
                   p_max_connection_count,
                   p_encoder)
            Dim c As configuration = Nothing
            c = New configuration()
            c.max_connection_count = v(p_max_connection_count).to(Of Int32)(c.max_connection_count)
            c.ls = link_status.create_server_link_status(v)
            If Not try_get_encoding(v(p_encoder), c.encoder) Then
                raise_error(error_type.warning, "Cannot get encoder from ", v(p_encoder))
            End If
            o = New server(c)
            Dim s As String = Nothing
            If v.value(p_ports, s) Then
                If Not o.add_ports(s) Then
                    Return False
                End If
            End If
            If v.value(p_prefixes, s) Then
                If Not o.add_prefixes(s) Then
                    Return False
                End If
            End If
            Return o.start()
        End If
    End Function

    Private Shared Sub init()
        assert(constructor.register(Function(v As var, ByRef o As server) As Boolean
                                        Return create(v, o)
                                    End Function))
    End Sub
End Class
