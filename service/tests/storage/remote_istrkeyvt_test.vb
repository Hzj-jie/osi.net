
Imports System.IO
Imports System.Net
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.storage
Imports osi.service.http
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.device
Imports http = osi.service.http
Imports constants = osi.service.storage.constants
Imports questioner = osi.service.commander.questioner

Public Class remote_istrkeyvt_test
    Inherits istrkeyvt_case

    Private Shared ReadOnly ls As link_status
    Private ReadOnly http_port As UInt16
    Private ReadOnly s As server
    Private ReadOnly istrkeyvt_name As String
    Private ReadOnly dispatcher As istrkeyvt_dispatcher

    Shared Sub New()
        ls = New link_status(timeout_ms:=npos,
                             rate_sec:=0,
                             max_content_length:=0)
    End Sub

    Public Sub New()
        Me.New(New default_istrkeyvt_case())
    End Sub

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
        s = New server()
        http_port = rnd_port()
        dispatcher = New istrkeyvt_dispatcher(New dispatcher())
        istrkeyvt_name = guid_str()
    End Sub

    Public Overrides Function preserved_processors() As Int16
        Return max(MyBase.preserved_processors(), CShort(2))
    End Function

    Private Function register_local_istrkeyvt() As Boolean
        Dim l As istrkeyvt = Nothing
        l = memory.ctor()
        Return assert_not_nothing(l) AndAlso
               assert_true(manager.register(istrkeyvt_name, l))
    End Function

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        assert_true(s.add_port(http_port))
        If Not assert_true(s.start()) Then
            'the server cannot start, so cannot go on with the following test
            Return Nothing
        End If
        assert_true(http.responder.respond(s, ++dispatcher, ls))
        If Not register_local_istrkeyvt() Then
            'the local istrkeyvt cannot work, so cannot go on with the following test
            Return Nothing
        End If
        Return istrkeyvt_questioner.ctor(istrkeyvt_name,
                                         New questioner(client_post_dev.herald_device_pool(IPAddress.Loopback,
                                                                                           http_port,
                                                                                           ls,
                                                                                           ls)))
    End Function

    Protected Overrides Function clean_up() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  assert_true(dispatcher.ignore())
                                  s.stop(30)
                                  assert_true(manager.erase(istrkeyvt_name, [default](Of istrkeyvt).null))
                                  ec = MyBase.clean_up()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class

Public Class remote_istrkeyvt_perf_test
    Inherits remote_istrkeyvt_test

    Public Sub New()
        MyBase.New(New default_istrkeyvt_perf_case())
    End Sub
End Class