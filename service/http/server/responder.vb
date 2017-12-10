
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.template
Imports osi.service.commander

Public NotInheritable Class responder
    Public Shared Function respond(ByVal s As server,
                                   ByVal e As executor,
                                   Optional ByVal ls As link_status = Nothing) As Boolean
        If s Is Nothing Then
            Return False
        Else
            AddHandler http_listener_context_handle.[New](s).handle_context_async,
                       Sub(ctx As HttpListenerContext, ByRef ec As event_comb)
                           ec = +(New commander.responder(Of _false) _
                                                         (New text_herald_adapter(New server_dev(ctx, ls)), npos, e))
                       End Sub
            Return True
        End If
    End Function

    Private Sub New()
    End Sub
End Class
