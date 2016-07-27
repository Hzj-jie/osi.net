
Imports System.Collections.Generic
Imports System.Net
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.threadpool
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.device
Imports osi.service.http.constants.interval_ms
Imports osi.service.selector

<global_init(global_init_level.server_services)>
Public Class server
    Public Event handle_context(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
    Public Event handle_context_sync(ByVal ctx As HttpListenerContext)
    Public Event handle_context_async(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
    Public Event handle_context_offline(ByVal ctx As HttpListenerContext,
                                        ByVal response_timeout_ms As Int64,
                                        ByVal after_respond As Action)

    Private Const default_max_connection_count As Int32 = 1024
    Private Const default_response_timeout_ms As Int64 = 60 * minute_second * second_milli

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
        If vs Is Nothing OrElse vs.empty() Then
            Return False
        Else
            For i As Int32 = 0 To vs.size() - 1
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

    Private Function end_context(ByVal ctx As HttpListenerContext, ByVal abort As Boolean) As Boolean
        If ctx Is Nothing Then
            Return False
        Else
            ctx.shutdown(abort)
            Return assert(cc.decrement() >= 0)
        End If
    End Function

    Private Function end_context(ByVal ctx As HttpListenerContext) As Boolean
        Return end_context(ctx, False)
    End Function

    Private Sub start_context(ByVal ctx As HttpListenerContext)
        assert(Not ctx Is Nothing)
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        If event_attached(handle_contextEvent) Then
                                            RaiseEvent handle_context(ctx, ec)
                                            If ec Is Nothing Then
                                                Return end_context(ctx) AndAlso
                                                       goto_end()
                                            Else
                                                Return waitfor(ec, response_timeout_ms) AndAlso
                                                       goto_next()
                                            End If
                                        ElseIf event_attached(handle_context_syncEvent) Then
                                            RaiseEvent handle_context_sync(ctx)
                                            Return end_context(ctx) AndAlso
                                                   goto_end()
                                        ElseIf event_attached(handle_context_asyncEvent) Then
                                            RaiseEvent handle_context_async(ctx, ec)
                                            Return waitfor(ec, response_timeout_ms) AndAlso
                                                   goto_next()
                                        ElseIf event_attached(handle_context_offlineEvent) Then
                                            RaiseEvent handle_context_offline(ctx,
                                                                              response_timeout_ms,
                                                                              Sub()
                                                                                  assert(end_context(ctx))
                                                                              End Sub)
                                            Return goto_end()
                                        Else
                                            ctx.Response().StatusCode() = HttpStatusCode.InternalServerError
                                            ctx.Response().StatusDescription() = "NOT_IMPLEMENTED"
                                            Return end_context(ctx) AndAlso
                                                   goto_end()
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        assert(Not ec Is Nothing)
                                        Return end_context(ctx, Not ec.end_result()) AndAlso
                                               goto_end()
                                    End Function))
    End Sub

    Private Sub listen()
        For i As Int32 = 0 To min(thread_pool().thread_count(), Environment.ProcessorCount()) - 1
            Dim ctx As pointer(Of HttpListenerContext) = Nothing
            Dim ec As event_comb = Nothing
            assert_begin(New event_comb(Function() As Boolean
                                            If listener.IsListening() Then
                                                ec = Nothing
                                                If Not ctx Is Nothing Then
                                                    ctx.clear()
                                                End If
                                                If cc.increment() <= max_connection_count Then
                                                    If ctx Is Nothing Then
                                                        ctx = New pointer(Of HttpListenerContext)()
                                                    End If
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
                                                start_context(+ctx)
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
