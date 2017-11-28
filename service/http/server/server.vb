
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.http.constants.interval_ms
Imports constructor = osi.service.device.constructor

<global_init(global_init_level.server_services)>
Public NotInheritable Class server
    Public Event context_received(ByVal ctx As context)

    Private Const default_max_connection_count As Int32 = 1024
    Private Const default_response_timeout_ms As Int64 = 60 * minute_second * second_milli

    Public Class context
        Public ReadOnly server As server
        Public ReadOnly context As HttpListenerContext
        Public ReadOnly response_timeout_ms As Int64
        Private ReadOnly f As once_action
        Private ReadOnly se As stopwatch.event

        Public abort As Boolean = False

        Public Sub New(ByVal server As server,
                       ByVal context As HttpListenerContext,
                       ByVal response_timeout_ms As Int64)
            assert(Not server Is Nothing)
            assert(Not context Is Nothing)
            assert(response_timeout_ms > 0)
            Me.server = server
            Me.context = context
            Me.response_timeout_ms = response_timeout_ms
            Me.f = New once_action(Sub()
                                       server.end_context(context, abort)
                                   End Sub)
            Me.se = stopwatch.push(response_timeout_ms,
                                   Sub()
                                       f.run()
                                   End Sub)
            assert(Not se Is Nothing)
        End Sub

        Public Sub finish()
            f.run()
            se.cancel()
        End Sub

        Public Sub finish(ByVal abort As Boolean)
            Me.abort = abort
            finish()
        End Sub
    End Class

    Private ReadOnly max_connection_count As Int32
    Private ReadOnly response_timeout_ms As Int64

    Private ReadOnly listener As HttpListener
    Private ReadOnly cc As atomic_int

    Shared Sub New()
        assert(HttpListener.IsSupported())
        ServicePointManager.DefaultConnectionLimit() = max_int32
        ServicePointManager.UseNagleAlgorithm() = False
        Try
            DirectCast(System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Nothing) _
                .GetSection("system.web/httpRuntime"),
                System.Web.Configuration.HttpRuntimeSection).EnableHeaderChecking() = False
        Catch ex As Exception
            raise_error(error_type.warning, "failed to set EnableHeaderChecking to false, ex ", ex.Message())
        End Try
    End Sub

    Public Sub New(ByVal max_connection_count As Int32,
                   ByVal response_timeout_ms As Int64)
        assert(max_connection_count > 0)
        assert(response_timeout_ms > 0)
        Me.max_connection_count = max_connection_count
        Me.response_timeout_ms = response_timeout_ms
        cc = New atomic_int()
        listener = New HttpListener()
        listener.IgnoreWriteExceptions() = True
    End Sub

    Public Sub New(ByVal max_connection_count As Int32)
        Me.New(max_connection_count, default_response_timeout_ms)
    End Sub

    Public Sub New(ByVal response_timeout_ms As Int64)
        Me.New(default_max_connection_count, response_timeout_ms)
    End Sub

    Public Sub New()
        Me.New(default_max_connection_count, default_response_timeout_ms)
    End Sub

    Public Sub New(ByVal max_connection_count As String,
                   ByVal response_timeout_ms As String)
        Me.New(max_connection_count.to_int32(default_max_connection_count),
               response_timeout_ms.to_int64(default_response_timeout_ms))
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
        Return add_port(port.to_uint16())
    End Function

    Private Function add_several(ByVal s As String, ByVal a As Func(Of String, Boolean)) As Boolean
        assert(Not a Is Nothing)
        Dim vs As vector(Of String) = Nothing
        vs = s.to_string_array()
        If vs.null_or_empty() Then
            Return False
        Else
            For i As UInt32 = 0 To vs.size() - uint32_1
                If Not a(vs(i)) Then
                    Return False
                End If
            Next
            Return True
        End If
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
                                                RaiseEvent context_received(New context(Me, +ctx, response_timeout_ms))
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
            Const p_response_timeout_ms As String = "response-timeout-ms"
            v.bind(p_ports,
                   p_prefixes,
                   p_max_connection_count,
                   p_response_timeout_ms)
            o = New server(v(p_max_connection_count), v(p_response_timeout_ms))
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
