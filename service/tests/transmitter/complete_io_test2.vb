
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.transmitter

Public Class complete_io_test2
    Inherits complete_io_test2(Of block_flow_adapter)

    Private ReadOnly m As mock_block

    Public Sub New()
        MyBase.New()
        m = New mock_block()
    End Sub

    Protected NotOverridable Overrides Function create_receive_flow() As block_flow_adapter
        Return New block_flow_adapter(m)
    End Function

    Protected NotOverridable Overrides Function create_send_flow() As block_flow_adapter
        Return New block_flow_adapter(m.the_other_end())
    End Function
End Class

Public MustInherit Class complete_io_test2(Of T As flow)
    Inherits [case]

    Protected MustOverride Function create_send_flow() As T
    Protected MustOverride Function create_receive_flow() As T

    Private Shared Function start_receive(ByVal f As T,
                                          ByVal b() As Byte,
                                          ByVal finished As ManualResetEvent) As Boolean
        assert(Not f Is Nothing)
        assert(Not finished Is Nothing)
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        ec = DirectCast(f, flow).receive(b)
                                        Return waitfor(ec) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        assertion.is_true(ec.end_result())
                                        assert(finished.Set())
                                        Return goto_end()
                                    End Function))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim b1() As Byte = Nothing
        b1 = next_bytes(rnd_uint(8192 * 256, 8192 * 512))
        Dim b2() As Byte = Nothing
        ReDim b2(array_size_i(b1) - 1)
        Dim finished As ManualResetEvent = Nothing
        finished = New ManualResetEvent(False)
        assertion.is_true(start_receive(create_receive_flow(), b2, finished))
        Dim s As flow = Nothing
        s = create_send_flow()
        assert(Not s Is Nothing)
        assertion.is_true(async_sync(s.send(b1), minutes_to_milliseconds(1)))
        assertion.is_true(finished.WaitOne(CInt(minutes_to_milliseconds(1))))
        finished.Close()
        assertion.equal(memcmp(b1, b2), 0)
        Return True
    End Function
End Class
