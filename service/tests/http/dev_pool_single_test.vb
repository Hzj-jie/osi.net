
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device
Imports osi.service.http

Public Class dev_pool_single_test
    Inherits commandline_specific_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New dev_pool_case(uint32_1), 1024))
    End Sub

    Public Class dev_pool_case
        Inherits event_comb_case

        Private ReadOnly s As server
        Private ReadOnly sp As idevice_pool(Of text)
        Private ReadOnly cgp As idevice_pool(Of text)
        Private ReadOnly cpp As idevice_pool(Of text)
        Private ReadOnly p As UInt16
        Private ReadOnly responder_count As UInt32

        Public Sub New(ByVal rc As UInt32)
            assert(rc > 0)
            p = rnd_port()
            s = New server()
            s.add_port(p)
            sp = server_dev.device_pool(s)
            cgp = client_get_dev.device_pool("localhost", p)
            cpp = client_post_dev.device_pool("localhost", p)
            responder_count = rc
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                If s.start() Then
                    For i As UInt32 = 0 To responder_count - uint32_1
                        respond()
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Overrides Function finish() As Boolean
            s.stop()
            assert_equal(sp.total_count(), uint32_0)
            assert_equal(sp.free_count(), uint32_0)
            assert_equal(cgp.total_count(), uint32_0)
            assert_equal(cgp.free_count(), uint32_0)
            assert_equal(cpp.total_count(), uint32_0)
            assert_equal(cpp.free_count(), uint32_0)
            Return MyBase.finish()
        End Function

        Private Sub respond()
            Dim h As idevice(Of text) = Nothing
            Dim p As pointer(Of String) = Nothing
            Dim ec As event_comb = Nothing
            begin_lifetime_event_comb(expiration_controller.[New](Function() Not s.alive()),
                                      Function() As Boolean
                                          If sp.get(h) AndAlso
                                             assert_not_nothing(h) AndAlso
                                             assert_not_nothing(h.get()) Then
                                              ec = h.get().sense()
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          ElseIf rnd_bool() Then
                                              Return waitfor_yield()
                                          Else
                                              Return waitfor_nap()
                                          End If
                                      End Function,
                                      Function() As Boolean
                                          p.renew()
                                          If assert_true(ec.end_result()) Then
                                              assert(Not h.get() Is Nothing)
                                              ec = h.get().receive(p)
                                              Return waitfor(ec) AndAlso
                                                     goto_next()
                                          Else
                                              assert_false(sp.release(h))
                                              Return goto_begin()
                                          End If
                                      End Function,
                                      Function()
                                          assert_true(ec.end_result())
                                          assert_true(Not p.empty())
                                          assert(Not h.get() Is Nothing)
                                          ec = h.get().send(+p)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End Function,
                                      Function() As Boolean
                                          assert_true(ec.end_result())
                                          assert_false(sp.release(h))
                                          Return goto_begin()
                                      End Function)
        End Sub

        Private Function rnd_long_str() As String
            ' server and client use utf8 to encode and decode the path string
            Return strcat(rnd_utf8_chars(rnd_int(100, 2560)), guid_str())
        End Function

#If 0 Then
        Private Function rnd_short_str() As String
            Return strcat(rnd_en_chars(rnd_int(5, 10)), guid_str())
        End Function
#End If

        Public Overrides Function create() As event_comb
            Dim ec As event_comb = Nothing
            Dim cp As idevice_pool(Of text) = Nothing
            Dim h As idevice(Of text) = Nothing
            Dim s As String = Nothing
            Dim p As pointer(Of String) = Nothing
            Return New event_comb(Function() As Boolean
                                      If rnd_bool() Then
                                          cp = cpp
                                          s = rnd_long_str()
                                      Else
                                          cp = cgp
                                          s = rnd_long_str()
                                          'short path for get method
                                          's = rnd_short_str()
                                      End If
                                      assert(Not cp Is Nothing)
                                      If assert_true(cp.get(h)) AndAlso
                                         assert_not_nothing(h) AndAlso
                                         assert_not_nothing(h.get()) Then
                                          ec = h.get().send(s)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assert_true(ec.end_result())
                                      p = New pointer(Of String)()
                                      assert(Not h Is Nothing AndAlso Not h.get() Is Nothing)
                                      ec = h.get().receive(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      assert_true(ec.end_result())
                                      assert_true(Not p.empty())
                                      ' Why cannot this pass with String.CompareOrdinal?
                                      assert_true(String.Compare(+p, s) = 0)
                                      assert_true(cp.release(h))
                                      Return goto_end()
                                  End Function)
        End Function
    End Class
End Class
