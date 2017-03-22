
' Sample:
' Server, osi.production.tcp_pair.exe --port=10000 --token=token
' Client, osi.production.tcp_pair.exe ~is-outgoing --host=localhost --port=10000 --token=token ~question
' Or
' Server, osi.production.tcp_pair.exe --port=10000 --token=token ~question
' Client, osi.production.tcp_pair.exe ~is-outgoing --host=localhost --port=10000 --token=token

' The expected QPS is ~3800 on a four-core machine.

Imports System.DateTime
Imports System.Threading
Imports osi.root.envs
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service
Imports osi.service.tcp
Imports osi.service.device
Imports osi.service.commander
Imports execution_wrapper = osi.service.commander.executor_wrapper
Imports responder = osi.service.commander.responder
Imports td = osi.service.tcp.constants.default_value.outgoing

Public Module tcp_pair

    Sub New()
        global_init.execute(load_assemblies:=True)
    End Sub

    Public Sub main(ByVal args() As String)
        debugpause()
        argument.parse(args)
        Dim p As idevice_pool(Of herald) = Nothing
        p = powerpoint.create(argument.default).herald_device_pool()
        Dim c As Int64 = 0
        If argument.switch("question") Then
            For i As Int32 = 0 To min(max(1, p.max_count()), td.max_connected) - 1
                Dim r As pointer(Of Byte()) = Nothing
                r = New pointer(Of Byte())
                Dim ec As event_comb = Nothing
                Dim s As String = Nothing
                begin_application_lifetime_event_comb(Function() As Boolean
                                                          s = guid_str()
                                                          ec = (New questioner(p))(str_bytes(s), r)
                                                          Return waitfor(ec) AndAlso
                                                                 goto_next()
                                                      End Function,
                                                      Function() As Boolean
                                                          If ec.end_result() Then
                                                              raise_error("question ", s, ", answer ", bytes_str(+r))
                                                              Interlocked.Increment(c)
                                                          Else
                                                              raise_error(error_type.warning,
                                                                          "failed to question ",
                                                                          argument.value("host"),
                                                                          ":",
                                                                          argument.value("port"))
                                                              assert_waitfor(sixteen_timeslice_length_ms)
                                                          End If
                                                          Return goto_begin()
                                                      End Function)
            Next
        Else
            assert(-(New responder(p,
                                   New execution_wrapper(
                                       Function(i() As Byte, o As pointer(Of Byte())) As event_comb
                                           Return New event_comb(Function() As Boolean
                                                                     Dim s As String = Nothing
                                                                     s = guid_str()
                                                                     raise_error("input ",
                                                                                 bytes_str(i),
                                                                                 ", output ",
                                                                                 s)
                                                                     Interlocked.Increment(c)
                                                                     Return eva(o, str_bytes(s)) AndAlso
                                                                            goto_end()
                                                                 End Function)
                                       End Function))))
        End If
        Dim start_ms As Int64 = 0
        start_ms = Now().milliseconds()
        begin_application_lifetime_event_comb(Function() As Boolean
                                                  Return waitfor(1000) AndAlso
                                                         goto_next()
                                              End Function,
                                              Function() As Boolean
                                                  rewrite_console("total requests ",
                                                                  c,
                                                                  ", qps ",
                                                                  seconds_to_milliseconds(c) \ _
                                                                  (Now().milliseconds() - start_ms))
                                                  Return goto_begin()
                                              End Function)
        gc_trigger()
    End Sub
End Module
