
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.http
Imports counter = osi.root.utils.counter

<test>
Public NotInheritable Class module_handles_test
    Private Const concurrent_connection_count As UInt32 = 100
    Private Const repeat_count As UInt32 = 100
    Private Const question_path As String = "question"
    Private Const async_question_path As String = "aquestion"
    Private Const ask_path As String = "ask"
    Private Const async_ask_path As String = "aask"

    Private ReadOnly port As UInt16
    Private ReadOnly request_count As atomic_int
    Private ReadOnly response_count As atomic_int

    Private Shared Function create_answer(ByVal path As String) As String
        Return strcat(question_path, "-answer")
    End Function

    Private NotInheritable Class instance_async_exec_question
        Private value As Int32 = 0

        <request_path("/" + async_question_path)>
        <request_method(constants.request_method.POST)>
        Private Function filtered_async_exec_question(ByVal ctx As server.context) As event_comb
            assertion.equal(value, 0)
            value += 1
            assertion.is_not_null(ctx)
            assertion.equal(ctx.parse_method(), constants.request_method.POST)
            Dim path As vector(Of String) = Nothing
            path = ctx.parse_path()
            assertion.is_false(path.empty())
            assertion.is_true(strsame(path(0), async_question_path, False))
            Return ctx.respond(create_answer(async_question_path))
        End Function
    End Class

    <request_path("/" + question_path)>
    <request_method(constants.request_method.GET)>
    Private Shared Function filtered_sync_exec_question(ByVal ctx As server.context) As event_comb
        assertion.is_not_null(ctx)
        assertion.equal(ctx.parse_method(), constants.request_method.GET)
        Dim path As vector(Of String) = Nothing
        path = ctx.parse_path()
        assertion.is_false(path.empty())
        assertion.is_true(strsame(path(0), question_path, False))
        ctx.set_status(Net.HttpStatusCode.OK, create_answer(question_path))
        Return Nothing
    End Function

    Private Shared Function sync_exec_question(ByVal ctx As server.context, ByRef ec As event_comb) As Boolean
        assertion.is_not_null(ctx)
        assertion.is_null(ec)
        If ctx.parse_method() <> constants.request_method.POST Then
            Return False
        End If

        Dim path As vector(Of String) = Nothing
        path = ctx.parse_path()
        If Not path.empty() AndAlso strsame(path(0), question_path, False) Then
            ctx.set_status(Net.HttpStatusCode.OK, create_answer(question_path))
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function async_exec_question(ByVal ctx As server.context, ByRef ec As event_comb) As Boolean
        assertion.is_not_null(ctx)
        assertion.is_null(ec)
        If ctx.parse_method() <> constants.request_method.GET Then
            Return False
        End If

        Dim path As vector(Of String) = Nothing
        path = ctx.parse_path()
        If Not path.empty() AndAlso strsame(path(0), async_question_path, False) Then
            ec = ctx.respond(create_answer(async_question_path))
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function sync_exec_ask(ByVal ctx As server.context, ByRef ec As event_comb) As Boolean
        assertion.is_not_null(ctx)
        assertion.is_null(ec)
        Dim path As vector(Of String) = Nothing
        path = ctx.parse_path()
        If Not path.empty() AndAlso strsame(path(0), ask_path, False) Then
            ctx.set_status(Net.HttpStatusCode.OK, create_answer(ask_path))
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function async_exec_ask(ByVal ctx As server.context, ByRef ec As event_comb) As Boolean
        assertion.is_not_null(ctx)
        assertion.is_null(ec)
        Dim path As vector(Of String) = Nothing
        path = ctx.parse_path()
        If Not path.empty() AndAlso strsame(path(0), async_ask_path, False) Then
            ec = ctx.respond(create_answer(async_ask_path))
            Return True
        Else
            Return False
        End If
    End Function

    Private Function should_not_be_called(ByVal ctx As server.context, ByRef ec As event_comb) As Boolean
        assertion.not_reach()
        Return False
    End Function

    <request_path("")>
    Private Shared Function filtered_should_not_be_called(ByVal ctx As server.context) As event_comb
        assertion.not_reach()
        Return Nothing
    End Function

    <prepare>
    Private Sub initialize()
        request_count.set(0)
        response_count.set(0)
    End Sub

    Private Function send_requests() As event_comb
        Dim i As Int32 = 0
        Dim ec As event_comb = Nothing
        Dim r As client.string_response = Nothing
        Dim path As String = Nothing
        Return New event_comb(Function() As Boolean
                                  If i = repeat_count Then
                                      Return goto_end()
                                  End If
                                  i += 1

                                  Dim rnd As Int32 = 0
                                  rnd = rnd_int(0, 5)
                                  Select Case rnd
                                      Case 0
                                          path = question_path
                                      Case 1
                                          path = async_question_path
                                      Case 2
                                          path = ask_path
                                      Case 3
                                          path = async_ask_path
                                      Case 4
                                          path = guid_str()
                                  End Select

                                  request_count.increment()
                                  r = New client.string_response()
                                  Dim request As request_builder = Nothing
                                  request = request_builder.
                                                  [New]().
                                                  with_url(generate_url("localhost", port, path)).
                                                  with_method(If(rnd_bool(),
                                                                 constants.request_method.POST,
                                                                 constants.request_method.GET))
                                  ec = request.request(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Select Case path
                                          Case question_path, ask_path
                                              If r.status() = Net.HttpStatusCode.OK AndAlso
                                                 strsame(r.status_description(), create_answer(path)) Then
                                                  response_count.increment()
                                              Else
                                                  raise_error(path)
                                              End If
                                          Case async_question_path, async_ask_path
                                              If r.status() = Net.HttpStatusCode.OK AndAlso
                                                 strsame(r.result(), create_answer(path)) Then
                                                  response_count.increment()
                                              Else
                                                  raise_error(path)
                                              End If
                                          Case Else
                                              If r.status() = Net.HttpStatusCode.NotImplemented Then
                                                  response_count.increment()
                                              Else
                                                  raise_error(path)
                                              End If
                                      End Select
                                  End If
                                  Return goto_begin()
                              End Function)
    End Function

    <test>
    Private Sub run()
        Dim s As server = Nothing
        s = New server(New server.configuration() With {.ls = New link_status(seconds_to_milliseconds(15))})
        Dim m As module_handle = Nothing
        m = New module_handle(s)

        assertion.is_true(m.add("osi.tests.service.http.module_handles_test",
                          default_str,
                          binding_flags.static_private_method,
                          "sync_exec_question"))
        assertion.is_true(m.add("osi.tests.service.http.module_handles_test",
                          default_str,
                          binding_flags.static_private_method,
                          "async_exec_question"))
        assertion.is_true(m.add("osi.tests.service.http.module_handles_test",
                          default_str,
                          binding_flags.static_private_method,
                          "filtered_sync_exec_question"))
        assertion.is_true(m.add(New var(strcat("--type=osi.tests.service.http.",
                                                "module_handles_test+instance_async_exec_question ",
                                         "--function=filtered_async_exec_question ",
                                         "--binding-flags=private,instance"))))
        assertion.is_true(m.add(New var(strcat("--type=osi.tests.service.http.module_handles_test ",
                                         "--function=sync_exec_ask ",
                                         "--binding-flags=static,private"))))
        assertion.is_true(m.add(New var(strcat("--type=osi.tests.service.http.module_handles_test ",
                                         "--function=async_exec_ask ",
                                         "--binding-flags=static|private"))))
        assertion.is_false(m.add(New var(strcat("--type=osi.tests.service.http.module_handles_test ",
                                          "--function=aasync_exec_ask ",
                                          "--binding-flags=static|private"))))
        assertion.is_false(m.add(New var(strcat("--type=osi.tests.service.http.module_handles_test ",
                                          "--function=should_not_be_called ",
                                          "--binding-flags=private"))))
        assertion.is_true(m.add(New var(strcat("--type=osi.tests.service.http.module_handles_test ",
                                         "--function=filtered_should_not_be_called ",
                                         "--binding-flags=static|private"))))

        assertion.equal(m.module_count(), CUInt(7))

        If Not assertion.is_true(s.add_port(port)) Then
            Return
        End If

        If Not assertion.is_true(s.start()) Then
            Return
        End If

        Dim ecs As vector(Of event_comb) = Nothing
        ecs = New vector(Of event_comb)()
        For i As UInt32 = 0 To concurrent_connection_count - uint32_1
            ecs.emplace_back(send_requests())
        Next
        async_sync(New event_comb(Function() As Boolean
                                      Return waitfor(+ecs) AndAlso
                                             goto_end()
                                  End Function))
        s.stop(30)
        assertion.equal(s.connection_count(), 0)
        assertion.more_or_equal_and_less_or_equal(+response_count, (+request_count) * 0.999, (+request_count))
        For i As UInt32 = 0 To m.module_count() - uint32_1
            Dim snapshot As counter.snapshot = Nothing
            snapshot = m.module_counter_snapshot(i)
            assertion.is_not_null(snapshot)
            assertion.is_not_null(snapshot.count)
            If strcontains(snapshot.name, "should_not_be_called") Then
                assertion.equal(+snapshot.count, 0, snapshot.name)
            Else
                assertion.more(+snapshot.count, 0, snapshot.name)
            End If
        Next
    End Sub

    Private Sub New()
        port = rnd_port()
        request_count = New atomic_int()
        response_count = New atomic_int()
    End Sub
End Class
