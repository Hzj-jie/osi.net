
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.service.http.client

Public Module _response_status
    <Extension()> Public Function informational(ByVal this As response) As Boolean
        Return Not this Is Nothing AndAlso _status.informational(this.status())
    End Function

    <Extension()> Public Function successful(ByVal this As response) As Boolean
        Return Not this Is Nothing AndAlso _status.successful(this.status())
    End Function

    <Extension()> Public Function redirection(ByVal this As response) As Boolean
        Return Not this Is Nothing AndAlso _status.redirection(this.status())
    End Function

    <Extension()> Public Function client_error(ByVal this As response) As Boolean
        Return Not this Is Nothing AndAlso _status.client_error(this.status())
    End Function

    <Extension()> Public Function server_error(ByVal this As response) As Boolean
        Return Not this Is Nothing AndAlso _status.server_error(this.status())
    End Function
End Module
