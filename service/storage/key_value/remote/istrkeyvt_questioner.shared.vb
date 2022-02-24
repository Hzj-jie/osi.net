
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.service
Imports osi.service.commander
Imports osi.service.convertor
Imports osi.service.storage.constants
Imports osi.service.storage.constants.remote

Partial Public Class istrkeyvt_questioner
    Private Function request(ByVal action As SByte) As command
        Dim r As command = Nothing
        r = New command()
        r.attach(action)
        Return r
    End Function

    Private Function request(ByVal action As SByte, ByVal key As String) As command
        Dim r As command = Nothing
        r = request(action)
        r.attach(parameter.key, key)
        Return r
    End Function

    Private Function request(ByVal action As SByte,
                             ByVal key As String,
                             ByVal buff() As Byte,
                             ByVal ts As Int64) As command
        Dim r As command = Nothing
        r = request(action, key)
        r.attach(parameter.buff, buff)
        If ts >= 0 Then
            r.attach(parameter.timestamp, ts)
        End If
        Return r
    End Function

    Private Function response(ByVal c As command, ByVal result As ref(Of Boolean)) As Boolean
        Return Not c Is Nothing AndAlso
               c.parameter(Of parameter, Boolean)(parameter.result, result)
    End Function

    Private Function response(ByVal c As command, ByVal result As ref(Of Int64)) As Boolean
        Return Not c Is Nothing AndAlso
               c.parameter(Of parameter, Int64)(parameter.size, result)
    End Function
End Class
