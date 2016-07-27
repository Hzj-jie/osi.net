﻿
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.iosys
Imports osi.service.convertor

Public Class iosys_test
    Inherits [case]

    Protected Const flower_size As UInt32 = 1024

    Protected Class iosys_test_case
        Public ReadOnly value As Int32

        Shared Sub New()
            bytes_convertor_register(Of iosys_test_case).assert_bind(
            Function(i() As Byte, ByRef offset As UInt32, ByRef o As iosys_test_case) As Boolean
                Dim b() As Byte = Nothing
                Return bytes_bytes_ref(i, b, offset) AndAlso
                       from_bytes(b, o)
            End Function,
            Function(i() As Byte, ii As UInt32, il As UInt32, ByRef o As iosys_test_case) As Boolean
                Dim b() As Byte = Nothing
                Return bytes_bytes_ref(i, ii, il, b) AndAlso
                       from_bytes(b, o)
            End Function,
            Function(i As iosys_test_case, o() As Byte, ByRef offset As UInt32) As Boolean
                Dim b() As Byte = Nothing
                Return to_bytes(i, b) AndAlso
                       bytes_bytes_val(b, o, offset)
            End Function,
            Function(i As iosys_test_case, ByRef o() As Byte) As Boolean
                Return to_bytes(i, o)
            End Function)
        End Sub

        Public Sub New()
            Me.New(rnd_int())
        End Sub

        Private Sub New(ByVal v As Int32)
            value = v
        End Sub

        Public Shared Function to_bytes(ByVal i As iosys_test_case, ByRef o() As Byte) As Boolean
            If i Is Nothing Then
                Return False
            Else
                o = i.value.to_bytes()
                Return True
            End If
        End Function

        Public Shared Function from_bytes(ByVal i() As Byte, ByRef o As iosys_test_case) As Boolean
            Dim v As Int32 = 0
            If bytes_int32(i, v) Then
                o = New iosys_test_case(v)
                Return True
            Else
                Return False
            End If
        End Function
    End Class

    Private Class iosys_test_ar
        Implements iagent(Of iosys_test_case), ireceiver(Of iosys_test_case)

        Private ReadOnly q As qless2(Of iosys_test_case)

        Public Event deliver(ByVal c As iosys_test_case,
                             ByRef failed As Boolean) Implements iagent(Of iosys_test_case).deliver

        Public Sub New()
            q = New qless2(Of iosys_test_case)()
        End Sub

        Public Function receive(ByVal c As iosys_test_case) _
                               As event_comb Implements ireceiver(Of iosys_test_case).receive
            If assert_not_nothing(c) Then
                Dim lc As iosys_test_case = Nothing
                While assert_true(q.pop(lc))
                    assert(Not lc Is Nothing)
                    If assert_equal(c.value, lc.value) Then
                        Exit While
                    End If
                End While
            End If
            Return Nothing
        End Function

        Public Sub start(ByVal round As Int64, ByVal interval_ms As Int64)
            assert_true(async_sync(New event_comb(Function() As Boolean
                                                      Return waitfor(Function() q.size() < flower_size,
                                                                     minute_to_milliseconds(1)) AndAlso
                                                             (assert_less_or_equal(q.size(), flower_size) AndAlso
                                                              goto_next()) OrElse
                                                             False
                                                  End Function,
                                                  Function() As Boolean
                                                      Dim c As iosys_test_case = Nothing
                                                      c = New iosys_test_case()
                                                      Dim f As Boolean = False
                                                      RaiseEvent deliver(c, f)
                                                      assert_false(f)
                                                      q.emplace(c)
                                                      If round = 0 Then
                                                          Return goto_next()
                                                      Else
                                                          round -= 1
                                                          Return waitfor(interval_ms) AndAlso
                                                                 goto_begin()
                                                      End If
                                                  End Function,
                                                  Function() As Boolean
                                                      Return waitfor(Function() q.empty(),
                                                                     minute_to_milliseconds(1)) AndAlso
                                                             goto_end()
                                                  End Function)))
            assert_true(q.empty())
        End Sub
    End Class

    Private ReadOnly round As Int64
    Private ReadOnly interval_ms As Int64

    Public Sub New()
        Me.New(32 * flower_size)
    End Sub

    Protected Sub New(ByVal round As Int64)
        Me.New(round, 0)
    End Sub

    Protected Sub New(ByVal round As Int64, ByVal interval_ms As Int64)
        assert(round > 0)
        assert(interval_ms >= 0)
        Me.round = round * If(isdebugbuild(), 1, 4)
        Me.interval_ms = interval_ms
    End Sub

    Protected Overridable Function create_iosys(ByRef first As iosys(Of iosys_test_case),
                                                ByRef last As iosys(Of iosys_test_case)) As Boolean
        first = New iosys(Of iosys_test_case)(flower_size)
        last = first
        Return True
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Dim f As iosys(Of iosys_test_case) = Nothing
        Dim l As iosys(Of iosys_test_case) = Nothing
        If assert_true(create_iosys(f, l)) Then
            Dim ar As iosys_test_ar = Nothing
            ar = New iosys_test_ar()
            If assert_true(f.listen(ar)) AndAlso
               assert_true(l.notice(ar)) Then
                ar.start(round, interval_ms)
            End If
        End If
        assert_true(If(object_compare(f, l) = 0, f.stop(), f.stop() AndAlso l.stop()))
        'wait a second to let the iosys stop working
        sleep()
        Return True
    End Function
End Class
