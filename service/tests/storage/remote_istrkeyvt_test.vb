
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.service.http
Imports osi.service.storage
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.device
Imports http = osi.service.http
Imports questioner = osi.service.commander.questioner

' TODO: This test is flaky.
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

    Public Overrides Function reserved_processors() As Int16
        Return max(MyBase.reserved_processors(), CShort(2))
    End Function

    Private Function register_local_istrkeyvt() As Boolean
        Dim l As istrkeyvt = Nothing
        l = memory.ctor()
        Return assertion.is_not_null(l) AndAlso
               assertion.is_true(manager.register(istrkeyvt_name, l))
    End Function

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        assertion.is_true(s.add_port(http_port))
        If Not assertion.is_true(s.start()) Then
            'the server cannot start, so cannot go on with the following test
            Return Nothing
        End If
        assertion.is_true(http.responder.respond(s, ++dispatcher, ls))
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
                                  assertion.is_true(dispatcher.ignore())
                                  s.stop(30)
                                  assertion.is_true(manager.erase(istrkeyvt_name, [default](Of istrkeyvt).null))
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