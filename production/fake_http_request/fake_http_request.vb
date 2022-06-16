
Imports System.DateTime
Imports System.IO
Imports System.Net
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.threadpool
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.service.http

Public Module fake_http_request
    Private ReadOnly add_rnd_request_headers As Boolean
    Private ReadOnly consume_data As Boolean
    Private ReadOnly urls As vector(Of String)

    Sub New()
        enable_domain_unhandled_exception_handler()
        register_slimqless2_threadpool()
        set_connection_reuse(Not env_bool(env_keys("shutdown", "connection")) AndAlso
                             Not env_bool(env_keys("reset", "connection")))
        add_rnd_request_headers = env_bool(env_keys("add", "random", "request", "headers"))
        consume_data = env_bool(env_keys("consume", "data"))
        urls = New vector(Of String)()
    End Sub

    Private Function rnd_request_headers() As map(Of String, vector(Of String))
        If add_rnd_request_headers Then
            Dim s As Int32 = 0
            s = rnd_int(1, 5)
            Dim r(,) As String = Nothing
            ReDim r(s, 1)
            For i As Int32 = 0 To s - 1
                r(i, 0) = rnd_en_chars(rnd_int(5, 10))
                r(i, 1) = rnd_en_chars(rnd_int(10, 20))
            Next
            r(s, 0) = "referer"
            r(s, 1) = random_url()
            Return r.to_http_headers()
        Else
            Return Nothing
        End If
    End Function

    Private Function random_url() As String
        Return urls(rnd_int(0, urls.size()))
    End Function

    Public Sub main(ByVal args() As String)
        If array_size(args) > 0 Then
            Using r As StreamReader = New StreamReader(args(0))
                Dim s As String = Nothing
                s = r.ReadLine()
                While Not s Is Nothing
                    If Not s.null_or_empty() Then
                        urls.push_back(s)
                    End If
                    s = r.ReadLine()
                End While
            End Using
            If Not urls.empty() Then
                Dim tc As Int32 = 0
                If array_size(args) < 2 OrElse Not Int32.TryParse(args(1), tc) Then
                    tc = Environment.ProcessorCount()
                End If

                Dim req_times As Int64 = 0
                Dim start_ms As Int64 = 0
                start_ms = Now().milliseconds()
                Dim req_time_ms As Int64 = 0
                Dim run_time_s As Func(Of Int64) = Function() As Int64
                                                       Return milliseconds_to_seconds(Now().milliseconds() - start_ms)
                                                   End Function
                Dim suc_times As Int64 = 0
                Dim ok_times As Int64 = 0
                For i As Int32 = 0 To tc - 1
                    Dim ts As Int64 = 0
                    Dim ec As event_comb = Nothing
                    Dim status As pointer(Of HttpStatusCode) = Nothing
                    assert_begin(New event_comb(Function() As Boolean
                                                    ts = Now().milliseconds()
                                                    status.renew()
                                                    ec = client.spider(random_url(),
                                                                       rnd_request_headers(),
                                                                       status,
                                                                       consume_data:=consume_data)
                                                    Return waitfor(ec) AndAlso
                                                           goto_next()
                                                End Function,
                                                Function() As Boolean
                                                    If ec.end_result() Then
                                                        Interlocked.Increment(suc_times)
                                                        If (+status) = HttpStatusCode.OK Then
                                                            Interlocked.Increment(ok_times)
                                                        End If
                                                    End If
                                                    Interlocked.Increment(req_times)
                                                    Interlocked.Add(req_time_ms, Now().milliseconds() - ts)
                                                    Return goto_begin()
                                                End Function))
                Next
                assert_begin(New event_comb(Function() As Boolean
                                                'make sure now.milliseconds() > start_ms
                                                Return waitfor(1000) AndAlso
                                                       goto_next()
                                            End Function,
                                            Function() As Boolean
                                                rewrite_console("total ",
                                                                req_times,
                                                                ", suc ",
                                                                suc_times,
                                                                ", OK ",
                                                                ok_times,
                                                                ", qps ",
                                                                req_times \ run_time_s(),
                                                                ", suc qps ",
                                                                suc_times \ run_time_s(),
                                                                ", OK qps ",
                                                                ok_times \ run_time_s(),
                                                                ", suc rate ",
                                                                suc_times / If(req_times = 0, 1, req_times),
                                                                ", OK rate ",
                                                                ok_times / If(req_times = 0, 1, req_times),
                                                                ", response time ",
                                                                If(req_times > 0, req_time_ms \ req_times, 0),
                                                                "ms")
                                                Return goto_prev()
                                            End Function))
                gc_trigger()
            End If
        End If
    End Sub
End Module
