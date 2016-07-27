
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.service.device

<type_attribute()>
Public Class mock_block_dev
    Inherits mock_block_dev(Of _false, _false)

    Shared Sub New()
        type_attribute.of(Of mock_block_dev)().forward_from(type_attribute.of(Of mock_block_dev(Of _false, _false))())
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(ByVal send_q As qless2(Of piece),
                      ByVal receive_q As qless2(Of piece))
        MyBase.New(send_q, receive_q)
    End Sub

    Public Shadows Function the_other_end() As mock_block_dev
        Return MyBase.the_other_end(Function(x, y) New mock_block_dev(x, y))
    End Function
End Class

<type_attribute()>
Public Class mock_block_dev(Of _SEND_RANDOM_FAIL As _boolean,
                               _RECEIVE_RANDOM_FAIL As _boolean)
    Inherits mock_dev_T(Of piece)
    Implements block, piece_dev

    Private Shared ReadOnly send_random_fail As Boolean
    Private Shared ReadOnly receive_random_fail As Boolean

    Shared Sub New()
        send_random_fail = +alloc(Of _SEND_RANDOM_FAIL)()
        receive_random_fail = +alloc(Of _RECEIVE_RANDOM_FAIL)()
        type_attribute.of(Of mock_block_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL))().set(transmitter.[New]() _
            .with_concurrent_operation(True) _
            .with_transmit_mode(transmitter.mode_t.duplex))
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Sub New(ByVal send_q As qless2(Of piece),
                      ByVal receive_q As qless2(Of piece))
        MyBase.New(send_q, receive_q)
    End Sub

    Public Shared Function create_with_receive_buff(ByVal b() As Byte, ByRef o As mock_block_dev) As Boolean
        o = New mock_block_dev()
        Return o.push_receive_q(b)
    End Function

    Public Shared Function create_with_receive_buff(ByVal b() As Byte) As mock_block_dev
        Dim o As mock_block_dev = Nothing
        assert(create_with_receive_buff(b, o))
        Return o
    End Function

    Private Shared Function random_fail() As Boolean
        Return rnd_bool_trues(3)
    End Function

    Private Shared Function partial_io() As Boolean
        Return rnd_bool()
    End Function

    Protected Shadows Function the_other_end(Of RT As mock_block_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL)) _
                                            (ByVal c As Func(Of qless2(Of piece), qless2(Of piece), RT)) As RT
        Return MyBase.the_other_end(c)
    End Function

    Public Shadows Function the_other_end() As mock_block_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL)
        Return MyBase.the_other_end(Function(x, y) New mock_block_dev(Of _SEND_RANDOM_FAIL, _RECEIVE_RANDOM_FAIL)(x, y))
    End Function

    Protected Overrides Function consistent(ByVal x As piece, ByVal y As piece) As Boolean
        Return piece.compare(x, y) = 0
    End Function

    Public Overloads Function send(ByVal buff() As Byte,
                                   ByVal offset As UInt32,
                                   ByVal count As UInt32) As event_comb Implements block_injector.send
        Return sync_async(Function() As Boolean
                              If send_random_fail AndAlso random_fail() Then
                                  Return False
                              ElseIf count = uint32_0 Then
                                  Return True
                              Else
                                  Dim p As piece = Nothing
                                  Return piece.create(buff, offset, count, p) AndAlso
                                         assert(sync_send(p))
                              End If
                          End Function)

    End Function

    Public Overloads Function receive(ByVal result As pointer(Of Byte())) As event_comb Implements block_pump.receive
        Return sync_async(Function() As Boolean
                              If receive_random_fail AndAlso random_fail() Then
                                  Return False
                              Else
                                  Dim p As piece = Nothing
                                  If Not receive_q.pop(p) Then
                                      p = Nothing
                                  End If
                                  Return eva(result, p.export_or_null())
                              End If
                          End Function)
    End Function

    Private Overloads Shared Function consistent(ByVal q As qless2(Of piece), ByVal v() As Byte) As Boolean
        assert(Not q Is Nothing)
        If q.empty() Then
            Return isemptyarray(v)
        Else
            Dim r As Boolean = False
            r = True
            Dim i As UInt32 = uint32_0
            For j As UInt32 = uint32_0 To q.size() - uint32_1
                Dim p As piece = Nothing
                assert(q.pop(p))
                assert(Not p Is Nothing)
                If i + p.count > array_size(v) Then
                    r = False
                Else
                    r = (memcmp(p.buff, p.offset, v, i, p.count) = 0)
                End If
                i += p.count
                q.emplace(p)
            Next
            Return r
        End If
    End Function

    Public Overloads Function send_q_consistent(ByVal v() As Byte) As Boolean
        Return consistent(send_q, v)
    End Function

    Public Overloads Function receive_q_consistent(ByVal v() As Byte) As Boolean
        Return consistent(receive_q, v)
    End Function

    Public Overloads Function consistent(ByVal v() As Byte) As Boolean
        Return send_q_consistent(v)
    End Function

    Public Function push_receive_q(ByVal buff() As Byte) As Boolean
        If isemptyarray(buff) Then
            Return False
        Else
            Dim i As UInt32 = uint32_0
            While i < array_size(buff)
                Dim j As UInt32 = uint32_0
                If array_size(buff) - i = uint32_1 Then
                    j = uint32_1
                Else
                    j = rnd_uint(uint32_0, array_size(buff) - i)
                End If
                Dim p As piece = Nothing
                assert(piece.create(buff, i, j, p))
                receive_q.emplace(p)
                i += j
            End While
            Return True
        End If
    End Function
End Class
