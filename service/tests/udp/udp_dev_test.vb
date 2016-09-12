﻿
Imports System.Net
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.udp

Public Class udp_dev_test
    Inherits event_comb_case

    Private ReadOnly p1 As powerpoint
    Private ReadOnly p2 As powerpoint

    Public Sub New()
        MyBase.New()
        p1 = powerpoint.creator.[New]().
                 with_host_or_ip(Convert.ToString(IPAddress.Loopback)).
                 with_local_port(rnd_port()).
                 with_remote_port(rnd_port()).create()
        p2 = powerpoint.creator.[New]().
                 with_host_or_ip(Convert.ToString(IPAddress.Loopback)).
                 with_local_port(p1.remote_port).
                 with_remote_port(p1.local_port).create()
    End Sub

    Private Shared Function send_and_receive_once(ByVal sender As udp_dev, ByVal receiver As udp_dev) As event_comb
        assert(Not sender Is Nothing)
        assert(Not receiver Is Nothing)
        Dim v() As Byte = Nothing
        Dim p As pointer(Of Boolean) = Nothing
        Dim r As pointer(Of Byte()) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  v = rndbytes(rnd_uint(100, 500))
                                  ec = sender.send(v)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If assert_true(ec.end_result()) Then
                                      p = New pointer(Of Boolean)()
                                      ec = receiver.sense(p, seconds_to_milliseconds(1))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If assert_true(ec.end_result()) AndAlso assert_true(+p) Then
                                      r = New pointer(Of Byte())()
                                      ec = receiver.receive(r)
                                      Return waitfor(ec, seconds_to_milliseconds(1)) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If assert_true(ec.end_result()) AndAlso assert_not_nothing(+r) Then
                                      assert_array_equal(+r, v)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Shared Function send_and_receive(ByVal u1 As udp_dev, ByVal u2 As udp_dev) As event_comb
        Dim ec As event_comb = Nothing
        Dim i As Int32 = 0
        Return New event_comb(Function() As Boolean
                                  If i < 50 Then
                                      If i > 0 Then
                                          assert(Not ec Is Nothing)
                                          If Not assert_true(ec.end_result()) Then
                                              Return False
                                          End If
                                      End If
                                      i += 1
                                      If rnd_bool() Then
                                          ec = send_and_receive_once(u1, u2)
                                      Else
                                          ec = send_and_receive_once(u2, u1)
                                      End If
                                      Return waitfor(ec)
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Public Overrides Function finish() As Boolean
        listeners.[New](p1).wait_for_stop()
        listeners.[New](p2).wait_for_stop()
        sleep(osi.service.device.constants.default_sense_timeout_ms)   ' Ensure dispenser has fully stopped.
        Return MyBase.finish()
    End Function

    Public Overrides Function create() As event_comb
        Dim u1 As pointer(Of udp_dev) = Nothing
        Dim u2 As pointer(Of udp_dev) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  u1 = New pointer(Of udp_dev)()
                                  ec = (New connector(p1)).udp_dev(u1)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If assert_true(ec.end_result()) AndAlso assert_false(u1.empty()) Then
                                      u2 = New pointer(Of udp_dev)()
                                      ec = (New connector(p2)).udp_dev(u2)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If assert_true(ec.end_result()) AndAlso assert_false(u2.empty()) Then
                                      ec = send_and_receive(+u1, +u2)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  u1.get().close()
                                  u2.get().close()
                                  Return assert_true(ec.end_result()) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
