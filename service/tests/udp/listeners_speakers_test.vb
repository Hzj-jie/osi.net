
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.udp

' Randomly select a speaker and a listener, and send data from speaker to listener.
Public NotInheritable Class listeners_speakers_test
    Inherits event_comb_case

    Private ReadOnly listener_port As UInt16 = rnd_port()
    Private ReadOnly speaker_port As UInt16 = rnd_port()

    Private Function retrieve_listener(ByRef r As listener) As Boolean
        Return listeners.[New](powerpoint.creator.
                                   [New]().
                                   with_ipv6().
                                   with_local_port(listener_port).
                                   with_accept_new_connection(False).
                                   create(), r)
    End Function

    Private Function send_and_receive(ByVal listener As listener, ByVal speaker As speaker) As event_comb
        assert(Not listener Is Nothing)
        assert(Not speaker Is Nothing)
        Dim ec As event_comb = Nothing
        Dim i As Int32 = 0
        Dim accepter As listener.one_accepter = Nothing
        Dim sent As ref(Of UInt32) = Nothing
        Dim send_data As vector(Of Byte()) = Nothing
        Dim receive_data As vector(Of Byte()) = Nothing
        Dim waited As Boolean = False
        Return New event_comb(Function() As Boolean
                                  If i < 1000 Then
                                      If i = 0 Then
                                          send_data = New vector(Of Byte())()
                                          receive_data = New vector(Of Byte())()
                                          accepter = New listener.one_accepter(
                                                         New IPEndPoint(IPAddress.IPv6Loopback, speaker_port))
                                          AddHandler accepter.received,
                                                     Sub(received_data() As Byte, remote_host As IPEndPoint)
                                                         receive_data.emplace_back(received_data)
                                                         assertion.equal(remote_host.Address(), IPAddress.IPv6Loopback)
                                                         assertion.equal(remote_host.AddressFamily(),
                                                                         Sockets.AddressFamily.InterNetworkV6)
                                                         assertion.equal(remote_host.Port(), speaker_port)
                                                     End Sub
                                          If Not assertion.is_true(listener.attach(accepter)) Then
                                              Return False
                                          End If
                                      End If
                                      i += 1
                                      Return waitfor_nap() AndAlso
                                             goto_next()
                                  End If
                                  If receive_data.size() < send_data.size() AndAlso Not waited Then
                                      waited = True
                                      Return waitfor(Function() As Boolean
                                                         Return receive_data.size() = send_data.size()
                                                     End Function,
                                                     seconds_to_milliseconds(100))
                                  End If
                                  assertion.equal(receive_data.size(), send_data.size())
                                  Dim j As UInt32 = 0
                                  While j < receive_data.size()
                                      assertion.array_equal(send_data(j), receive_data(j))
                                      j += uint32_1
                                  End While
                                  Return assertion.is_true(listener.detach(accepter)) AndAlso
                                         goto_end()
                              End Function,
                              Function() As Boolean
                                  ' <= 500 bytes should be safe for one datagram.
                                  send_data.emplace_back(rnd_bytes(rnd_uint(100, 500)))
                                  sent.renew()
                                  ec = speaker.send(IPAddress.IPv6Loopback, listener_port, send_data.back(), sent)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If assertion.is_true(ec.end_result()) Then
                                      assertion.equal(+sent, array_size(send_data.back()))
                                      Return goto_begin()
                                  End If
                                  Return False
                              End Function)
    End Function

    Public Overrides Function create() As event_comb
        Dim listener As listener = Nothing
        Dim speaker As speaker = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If assertion.is_true(retrieve_listener(listener)) AndAlso
                                     assertion.is_true(speakers.[New](powerpoint.creator.
                                                                    [New]().
                                                                    with_ipv6().
                                                                    with_local_port(speaker_port).
                                                                    with_accept_new_connection(False).
                                                                    create(), speaker)) Then
                                      ec = send_and_receive(listener, speaker)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                                  Return False
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Overrides Function finish() As Boolean
        Dim listener As listener = Nothing
        If assertion.is_true(retrieve_listener(listener)) Then
            assertion.is_true(listener.wait_for_stop(osi.service.selector.constants.default_sense_timeout_ms))
            assertion.is_true(listener.stopped())
            ' Ensure dispenser has fully stopped. The dispenser.work() has been canceled,
            ' but the T_receiver.sense may not.
            sleep(osi.service.selector.constants.default_sense_timeout_ms)
        End If
        Return MyBase.finish()
    End Function
End Class
