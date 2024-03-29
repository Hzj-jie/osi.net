
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.device
Imports osi.service.http
Imports osi.service.iosys
Imports http = osi.service.http

Public NotInheritable Class remote_iosys_test
    Inherits flaky_case_wrapper

    Public Sub New()
        MyBase.New(New remote_iosys_case())
    End Sub

    Private NotInheritable Class remote_iosys_case
        Inherits iosys_test

        Private Shared ReadOnly ls As link_status
        Private ReadOnly p As UInt16
        Private ReadOnly s As server
        Private ReadOnly q As questioner
        Private ReadOnly ra_name As String
        Private ReadOnly ra As ragent(Of iosys_test_case)

        Shared Sub New()
            ls = New link_status(timeout_ms:=npos,
                             rate_sec:=0,
                             max_content_length:=0)
        End Sub

        Public Sub New()
            MyBase.New(flower_size * 2)
            p = rnd_port()
            s = New server()
            q = New questioner(client_post_dev.herald_device_pool(IPAddress.Loopback, p, ls, ls))
            ra_name = guid_str()
            ra = New ragent(Of iosys_test_case)()
        End Sub

        Protected Overrides Function create_iosys(ByRef first As iosys(Of iosys_test_case),
                                              ByRef last As iosys(Of iosys_test_case)) As Boolean
            first = New iosys(Of iosys_test_case)(uint32_0)
            last = New iosys(Of iosys_test_case)(uint32_0)
            Dim rr As rreceiver(Of iosys_test_case) = Nothing
            rr = New rreceiver(Of iosys_test_case)(ra_name, q)
            If Not assertion.is_true(first.notice(rr)) Then
                Return False
            End If
            Dim cra As command_ragent(Of iosys_test_case) = Nothing
            cra = New command_ragent(Of iosys_test_case)()
            If Not assertion.is_true(device_pool_manager.register(
                               ra_name,
                               singleton_device_pool.[New](cra.make_device()))) OrElse
           Not assertion.is_true(last.listen(cra)) Then
                Return False
            End If
            If Not assertion.is_true(s.add_port(p)) OrElse
           Not assertion.is_true(s.start()) Then
                Return False
            End If
            assertion.is_true(http.responder.respond(s, ++ra, ls))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            s.stop(30)
            Return assertion.is_true(device_pool_manager.erase(
                   ra_name, [default](Of singleton_device_pool(Of command_ragent(Of iosys_test_case))).null)) And
               ra.ignore() And
               MyBase.finish()
        End Function
    End Class
End Class
