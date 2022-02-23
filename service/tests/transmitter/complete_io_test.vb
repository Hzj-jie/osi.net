
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utils
Imports osi.service.transmitter

Public Class complete_io_test
    Inherits complete_io_test(Of mock_flow_dev)

    Protected NotOverridable Overrides Function create_send_flow(ByVal send_size As UInt32) As mock_flow_dev
        Return New mock_flow_dev(send_size, True, False)
    End Function

    Protected NotOverridable Overrides Function create_receive_flow(ByVal buff() As Byte) As mock_flow_dev
        Return New mock_flow_dev(buff, uint32_0, True, False)
    End Function

    Protected NotOverridable Overrides Function send_consistent(ByVal flow_dev As mock_flow_dev,
                                                                ByVal buff() As Byte) As Boolean
        assert(flow_dev IsNot Nothing)
        Return flow_dev.send_consistent(buff)
    End Function

    Protected NotOverridable Overrides Function receive_consistent(ByVal flow_dev As mock_flow_dev,
                                                                   ByVal buff() As Byte) As Boolean
        assert(flow_dev IsNot Nothing)
        Return flow_dev.receive_consistent(buff)
    End Function
End Class

Public MustInherit Class complete_io_test(Of T As flow)
    Inherits [case]

    Protected MustOverride Function create_send_flow(ByVal send_size As UInt32) As T
    Protected MustOverride Function create_receive_flow(ByVal buff() As Byte) As T
    Protected MustOverride Function send_consistent(ByVal flow_dev As T, ByVal buff() As Byte) As Boolean
    Protected MustOverride Function receive_consistent(ByVal flow_dev As T, ByVal buff() As Byte) As Boolean

    Private Function send_case() As Boolean
        Const send_size As Int32 = 65536 * 128
        Dim f As T = Nothing
        f = create_send_flow(send_size)
        Dim buff() As Byte = Nothing
        buff = next_bytes(send_size)
        assertion.is_true(async_sync(cast(Of flow)(f).send(buff)))
        assertion.is_true(send_consistent(f, buff))
        Return True
    End Function

    Private Function receive_case() As Boolean
        Const receive_size As Int32 = 65536 * 128
        Dim buff() As Byte = Nothing
        buff = next_bytes(receive_size)
        Dim f As T = Nothing
        f = create_receive_flow(buff)
        Dim buff2() As Byte = Nothing
        ReDim buff2(receive_size - 1)
        assertion.is_true(async_sync(cast(Of flow)(f).receive(buff2)))
        assertion.is_true(receive_consistent(f, buff2))
        assertion.equal(memcmp(buff, buff2), 0)
        Return True
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Using scoped.atomic_bool(suppress.pending_io_punishment)
            Return send_case() AndAlso
                   receive_case()
        End Using
    End Function
End Class
