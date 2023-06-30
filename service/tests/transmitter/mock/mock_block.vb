
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.service.transmitter

<type_attribute()>
Public Class mock_block
    Inherits mock_block(Of _false, _false)

    Shared Sub New()
        type_attribute.of(Of mock_block)().forward_from(Of mock_block(Of _false, _false))()
    End Sub

    Private Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of piece),
                    ByVal receive_pump As slimqless2_event_sync_T_pump(Of piece))
        MyBase.New(send_pump, receive_pump)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Shadows Function the_other_end() As mock_block
        Return New mock_block(receive_pump, send_pump)
    End Function
End Class

<type_attribute()>
Public Class mock_block(Of RANDOM_SEND_FAILURE As _boolean, RANDOM_RECEIVE_FAILURE As _boolean)
    Inherits mock_dev_T(Of piece, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
    Implements block, piece_dev

    Shared Sub New()
        type_attribute.of(Of mock_block(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE))().forward_from(
                Of mock_dev_T(Of piece, RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE))()
    End Sub

    Private ReadOnly block_dev As block

    Protected Sub New(ByVal send_pump As slimqless2_event_sync_T_pump(Of piece),
                      ByVal receive_pump As slimqless2_event_sync_T_pump(Of piece))
        MyBase.New(send_pump, receive_pump)
        Me.block_dev = New dev_piece_block_adapter(Me)
    End Sub

    Public Sub New()
        Me.New(new_pump(), new_pump())
    End Sub

    Public Shadows Function the_other_end() As mock_block(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)
        Return New mock_block(Of RANDOM_SEND_FAILURE, RANDOM_RECEIVE_FAILURE)(receive_pump, send_pump)
    End Function

    Public Overloads Function send(ByVal buff() As Byte,
                                   ByVal offset As UInt32,
                                   ByVal count As UInt32) As event_comb Implements block_injector.send
        Return block_dev.send(buff, offset, count)
    End Function

    Public Overloads Function receive(ByVal o As ref(Of Byte())) As event_comb Implements block_pump.receive
        Return block_dev.receive(o)
    End Function

    Private Overloads Shared Function equal(ByVal p As slimqless2_event_sync_T_pump(Of piece),
                                            ByVal v() As Byte) As Boolean
        Dim i As Int32 = 0
        Return equal(p,
                     Function(o As piece) As Boolean
                         assert(Not o Is Nothing)
                         If i + o.count > array_size(v) OrElse
                            o.compare(v, i, o.count) <> 0 Then
                             Return False
                         Else
                             i += o.count
                             Return True
                         End If
                     End Function,
                     Function() As Boolean
                         Return i = array_size(v)
                     End Function)
    End Function

    Public Overloads Function send_pump_equal(ByVal v() As Byte) As Boolean
        Return equal(send_pump, v)
    End Function

    Public Overloads Function receive_pump_equal(ByVal v() As Byte) As Boolean
        Return equal(receive_pump, v)
    End Function

    Public Function push_receive_pump(ByVal buff() As Byte) As Boolean
        If isemptyarray(buff) Then
            Return False
        Else
            Dim i As Int32 = 0
            While i < array_size(buff)
                Dim j As Int32 = 0
                If array_size(buff) - i = 1 Then
                    j = 1
                Else
                    j = rnd_int(0, array_size(buff) - i)
                End If
                Dim p As piece = Nothing
                assert(piece.create(buff, i, j, p))
                receive_pump.emplace(p)
                i += j
            End While
            Return True
        End If
    End Function
End Class
