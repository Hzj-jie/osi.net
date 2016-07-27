
Imports System.IO
Imports System.Net
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.template
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.lock.slimlock
Imports osi.service.commander
Imports osi.service.tcp
Imports osi.service.http
Imports osi.service.convertor
Imports osi.service.device
Imports cmd = osi.service.commander
Imports tcp = osi.service.tcp
Imports http = osi.service.http

'different combinations will generate a different set of parameters
Public Class commander_case(Of _ENABLE_TCP As _boolean,
                               _ENABLE_HTTP_GET As _boolean,
                               _ENABLE_HTTP_POST As _boolean,
                               _CONNECTION_COUNT As _int64)
    Inherits case_wrapper

    Private Shared ReadOnly enable_tcp As Boolean
    Private Shared ReadOnly enable_http_get As Boolean
    Private Shared ReadOnly enable_http_post As Boolean
    Private Shared ReadOnly enable_http As Boolean
    Private Shared ReadOnly token As String
    Private Shared ReadOnly connection_count As Int32
    Private Shared ReadOnly ask_command As Int32
    Private Shared ReadOnly answer_command As Int32
    Private Shared ReadOnly ask_para As Int32
    Private Shared ReadOnly answer_para As Int32
    Private Shared ReadOnly tcp_port As UInt16
    Private Shared ReadOnly http_port As UInt16
    Private ReadOnly dispatcher As dispatcher

    Shared Sub New()
        enable_tcp = +(alloc(Of _ENABLE_TCP)())
        enable_http_get = +(alloc(Of _ENABLE_HTTP_GET)())
        enable_http_post = +(alloc(Of _ENABLE_HTTP_POST)())
        enable_http = enable_http_get OrElse enable_http_post
        assert(enable_tcp OrElse enable_http)
        If enable_tcp Then
            token = guid_str()
            tcp_port = rnd_port()
        End If
        If enable_http Then
            http_port = rnd_port()
        End If
        connection_count = +(alloc(Of _CONNECTION_COUNT)())
        If connection_count = npos Then
            connection_count = If(isdebugbuild(), 1, 2) * min((Environment.ProcessorCount() << 4), 64)
        End If
        ask_command = rnd_int(min_int32, max_int32)
        answer_command = rnd_int(min_int32, max_int32)
        ask_para = rnd_int(min_int32, max_int32)
        answer_para = rnd_int(min_int32, max_int32)
    End Sub

    Public Sub New()
        MyBase.New(If(connection_count > 1,
                      multi_procedure(repeat(New commander_case(), If(isdebugbuild(), 1, 2) * 4096), connection_count),
                      repeat(New commander_case(), If(isdebugbuild(), 1, 2) * 4096)))
        dispatcher = New dispatcher()
    End Sub

    Private Shared Function handle(ByVal i As command, ByVal o As command) As event_comb
        Return sync_async(Function() As Boolean
                              If i Is Nothing OrElse
                                 o Is Nothing Then
                                  Return False
                              Else
                                  If i.action().to_int32() = ask_command Then
                                      o.attach(answer_command) _
                                       .attach(answer_para, i.parameter(ask_para).to_int32() + 1)
                                  Else
                                      o.attach(answer_command + 1)
                                  End If
                                  Return True
                              End If
                          End Function)
    End Function

    Public Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        assert_true(dispatcher.register(ask_command, AddressOf handle))
        Dim pp As idevice_pool(Of herald) = Nothing
        Dim s As server = Nothing
        If enable_tcp Then
            pp = powerpoint.creator.[New]().
                 with_token(token).
                 with_port(tcp_port).
                 create().herald_device_pool()
            assert_true(-(New cmd.responder(pp, npos, dispatcher)))
            commander_case.opp.set(powerpoint.creator.[New]().
                                   with_token(token).
                                   with_endpoint(New IPEndPoint(IPAddress.Loopback, tcp_port)).
                                   with_max_connected(CUInt(connection_count)).
                                   create().herald_device_pool())
            assert_true(timeslice_sleep_wait_when(Function() pp.total_count() < connection_count,
                                                  seconds_to_milliseconds(30)))
        End If
        If enable_http Then
            s = New server(response_timeout_ms:=seconds_to_milliseconds(15))
            assert_true(s.add_port(http_port))
            assert_true(s.start())
            assert_true(http.responder.respond(s, dispatcher))
        End If
        Dim r As Boolean = False
        r = MyBase.run()
        If enable_tcp Then
            commander_case.opp.get().close()
            pp.close()
            powerpoint.waitfor_stop()
        End If
        If enable_http Then
            s.stop(30)
        End If
        assert_true(dispatcher.erase(ask_command))
        Return True
    End Function

    Private Class commander_case
        Inherits count_event_comb_case

        Public Shared ReadOnly opp As pointer(Of idevice_pool(Of herald))

        Shared Sub New()
            opp = New pointer(Of idevice_pool(Of herald))()
        End Sub

        Private ReadOnly tcp_suc As atomic_int
        Private ReadOnly http_get_suc As atomic_int
        Private ReadOnly http_post_suc As atomic_int
        Private tcp_q As questioner
        Private http_get_q As questioner
        Private http_post_q As questioner

        Public Sub New()
            MyBase.New(False)
            If enable_tcp Then
                tcp_suc = New atomic_int()
            End If
            If enable_http_get Then
                http_get_suc = New atomic_int()
            End If
            If enable_http_post Then
                http_post_suc = New atomic_int()
            End If
        End Sub

        Public Overrides Function prepare() As Boolean
            If enable_tcp Then
                tcp_suc.set(0)
            End If
            If enable_http_get Then
                http_get_suc.set(0)
            End If
            If enable_http_post Then
                http_post_suc.set(0)
            End If
            tcp_q = Nothing
            http_get_q = Nothing
            http_post_q = Nothing
            Return MyBase.prepare()
        End Function

        Protected Overrides Function create_case() As event_comb
            assert(Not +opp Is Nothing OrElse Not enable_tcp)
            If enable_tcp AndAlso tcp_q Is Nothing Then
                tcp_q = New questioner(+opp)
            End If
            If enable_http_get AndAlso http_get_q Is Nothing Then
                http_get_q = New questioner(client_get_dev.herald_device_pool(Convert.ToString(IPAddress.Loopback),
                                                                              http_port,
                                                                              max_connection:=connection_count))
            End If
            If enable_http_post AndAlso http_post_q Is Nothing Then
                http_post_q = New questioner(client_post_dev.herald_device_pool(Convert.ToString(IPAddress.Loopback),
                                                                                http_port,
                                                                                max_connection:=connection_count))
            End If
            Dim para As Int32 = 0
            Dim r As pointer(Of command) = Nothing
            Dim ec As event_comb = Nothing
            Dim choice As Int32 = 0
            Return New event_comb(Function() As Boolean
                                      Const max_choice As Int32 = 3
                                      Dim c As command = Nothing
                                      c = New command()
                                      para = rnd_int(min_int32, max_int32)
                                      c.attach(ask_command) _
                                       .attach(ask_para, para)
                                      r = New pointer(Of command)()
                                      choice = rnd_int(0, max_choice)
                                      For i As Int32 = 0 To max_choice - 2
                                          If Not enable_tcp AndAlso choice = 0 Then
                                              choice = 1
                                          End If
                                          If Not enable_http_get AndAlso choice = 1 Then
                                              choice = 2
                                          End If
                                          If Not enable_http_post AndAlso choice = 2 Then
                                              choice = 0
                                          End If
                                      Next
                                      Select Case choice
                                          Case 0
                                              assert(enable_tcp)
                                              ec = tcp_q(c, r)
                                          Case 1
                                              assert(enable_http_get)
                                              ec = http_get_q(c, r)
                                          Case 2
                                              assert(enable_http_post)
                                              ec = http_post_q(c, r)
                                          Case Else
                                              assert(False)
                                      End Select
                                      assert(Not ec Is Nothing)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso
                                         Not (+r) Is Nothing AndAlso
                                         (+r).action().to_int32(answer_command + 1) = answer_command AndAlso
                                         (+r).parameter(answer_para).to_int32(para) = para + 1 Then
                                          Select Case choice
                                              Case 0
                                                  assert(enable_tcp)
                                                  tcp_suc.increment()
                                              Case 1
                                                  assert(enable_http_get)
                                                  http_get_suc.increment()
                                              Case 2
                                                  assert(enable_http_post)
                                                  http_post_suc.increment()
                                              Case Else
                                                  assert(False)
                                          End Select
                                          trigger()
                                      Else
                                          assert_waitfor(sixteen_timeslice_length_ms)
                                      End If
                                      Return goto_end()
                                  End Function)
        End Function

        Public Overrides Function finish() As Boolean
            assert_more_or_equal_and_less_or_equal(trigger_times(), 0.9999 * run_times(), run_times())
            Dim div As Int32 = 0
            div = If(enable_tcp, 1, 0) +
                  If(enable_http_get, 1, 0) +
                  If(enable_http_post, 1, 0)
            assert(div > 0)
            If enable_tcp Then
                assert_more_or_equal_and_less_or_equal(+tcp_suc, run_times() / div * 0.9, run_times() / div * 1.1)
            End If
            If enable_http_get Then
                assert_more_or_equal_and_less_or_equal(+http_get_suc, run_times() / div * 0.9, run_times() / div * 1.1)
            End If
            If enable_http_post Then
                assert_more_or_equal_and_less_or_equal(+http_post_suc, run_times() / div * 0.9, run_times() / div * 1.1)
            End If
            Return MyBase.finish()
        End Function
    End Class
End Class
