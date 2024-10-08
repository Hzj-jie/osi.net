
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt
Imports osi.service.http
Imports osi.service.transmitter

Public Class http_server_test
    Inherits processor_measured_case_wrapper

    Public Sub New()
        MyBase.New(New http_server_case())
    End Sub

    Private Class http_server_case
        Inherits [case]

        Private Shared ReadOnly repeat As Int32 = 2
        Private Shared ReadOnly parallel As Int32 = 8
        Private Shared ReadOnly iteration As Int32 = 1024
        Private Shared ReadOnly enc As Encoding
        Private ReadOnly port As UInt16
        Private ReadOnly request_times As atomic_int
        Private ReadOnly response_times As atomic_int
        Private started As Boolean = False

        Shared Sub New()
            ServicePointManager.DefaultConnectionLimit() = max_int32
            'ServicePointManager.UseNagleAlgorithm() = False
            parallel <<= If(isdebugbuild(), 0, 1)
            iteration <<= If(isdebugbuild(), 0, 4)
            repeat <<= If(isdebugbuild(), 0, 1)
            enc = default_encoding
        End Sub

        Public Sub New()
            port = rnd_port()
            request_times = New atomic_int()
            response_times = New atomic_int()
        End Sub

        Private Sub handle_context(ByVal ctx As HttpListenerContext, ByRef ec As event_comb)
            ec = New event_comb(Function() As Boolean
                                    If ctx Is Nothing Then
                                        Return False
                                    Else
                                        Dim buff() As Byte = Nothing
                                        buff = enc.GetBytes(Convert.ToString(request_times.increment()))
                                        ctx.Response().StatusCode() = HttpStatusCode.OK
                                        ctx.Response().StatusDescription() = "OK"
                                        Return waitfor(ctx.Response().write_response(buff,
                                                                                     0,
                                                                                     array_size(buff),
                                                                                     link_status.null)) AndAlso
                                               goto_end()
                                    End If
                                End Function)
        End Sub

        Private Function send_request() As event_comb
            Dim i As Int32 = 0
            Dim w As WebRequest = Nothing
            Dim ec As event_comb = Nothing
            Dim resp As ref(Of WebResponse) = Nothing
            Dim buff() As Byte = Nothing
            Return New event_comb(Function() As Boolean
                                      w = WebRequest.Create(strcat("http://localhost:", port, "/"))
                                      resp = New ref(Of WebResponse)()
                                      ec = w.get_response(resp)
                                      Return waitfor(ec, seconds_to_milliseconds(5)) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso
                                         assertion.is_not_null(+resp) Then
                                          ReDim buff(CInt((+resp).ContentLength() - 1))
                                          ec = (New stream_flow_adapter((+resp).GetResponseStream())). _
                                                   receive(buff,
                                                           0,
                                                           array_size(buff))
                                          Return waitfor(ec, seconds_to_milliseconds(5)) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_last()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If assertion.is_true(ec.end_result()) Then
                                          Dim j As Int32 = 0
                                          If assertion.is_true(Int32.TryParse(enc.GetString(buff),
                                                                        j)) Then
                                              assertion.less_or_equal(j, +request_times)
                                              assertion.less_or_equal(response_times.increment(), +request_times)
                                          End If
                                      End If
                                      If (+resp) Is Nothing Then
                                          w.Abort()
                                      Else
                                          Dim wr As HttpWebResponse = Nothing
                                          assertion.is_true(direct_cast(+resp, wr))
                                          assertion.is_not_null(Not wr Is Nothing)
                                          wr.GetResponseStream().Close()
                                          wr.GetResponseStream().Dispose()
                                          wr.Close()
                                      End If
                                      Return If(_inc(i) = iteration, goto_end(), goto_begin())
                                  End Function)
        End Function

        Public Overrides Function run() As Boolean
            started = True

            Dim p As UInt32 = 0
            Dim s As server = Nothing
            s = New server(New server.configuration() With {.ls = New link_status(seconds_to_milliseconds(15))})
            AddHandler http_listener_context_handle.[New](s).handle_context, AddressOf handle_context
            For j As Int32 = 0 To repeat - 1
                If assertion.is_true(s.add_port(port)) AndAlso
                   assertion.is_true(s.start()) Then
                    Dim ecs() As event_comb = Nothing
                    ReDim ecs(parallel - 1)
                    For i As Int32 = 0 To parallel - 1
                        ecs(i) = send_request()
                        assert_begin(ecs(i))
                    Next
                    timeslice_sleep_wait_until(Function() As Boolean
                                                   For i As Int32 = 0 To parallel - 1
                                                       If Not ecs(i).end() Then
                                                           Return False
                                                       End If
                                                   Next
                                                   Return True
                                               End Function)
                    s.stop(30)
                    assertion.equal(s.connection_count(), 0)
                End If
            Next
            Return True
        End Function

        Public Overrides Function reserved_processors() As Int16
            Return CShort(Environment.ProcessorCount())
        End Function

        Private Function success_rate(ByVal i As atomic_int) As Double
            assert(Not i Is Nothing)
            Dim r As Double = 0
            r = (+i) / parallel / iteration / repeat
            Return r
        End Function

        Private Function request_success_rate() As Double
            Return success_rate(request_times)
        End Function

        Private Function response_success_rate() As Double
            Return success_rate(response_times)
        End Function

        Public Overrides Function finish() As Boolean
            If started Then
                assertion.more_or_equal_and_less_or_equal(request_success_rate(), 0.9999, 1)
                assertion.more_or_equal_and_less_or_equal(response_success_rate(), 0.9999, 1)
            End If
            Return MyBase.finish()
        End Function
    End Class
End Class
