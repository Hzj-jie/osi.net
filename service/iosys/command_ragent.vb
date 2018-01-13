
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.commander
Imports osi.service.device

Public Class command_ragent(Of CASE_T)
    Inherits agent(Of CASE_T)

    Public Function push(ByVal i As command) As Boolean
        Dim c As CASE_T = Nothing
        If Not i Is Nothing AndAlso
           i.action_is(constants.remote.action.push) AndAlso
           cast(i, c) Then
            Dim f As Boolean = False
            received(c, f)
            Return Not f
        Else
            Return False
        End If
    End Function

    Public Function as_device() As idevice(Of command_ragent(Of CASE_T))
        Return make_device()
    End Function

    Public Function device_pool() As idevice_pool(Of command_ragent(Of CASE_T))
        Return singleton_device_pool.[New](as_device())
    End Function
End Class
