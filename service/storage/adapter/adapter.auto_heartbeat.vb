
Imports osi.root.template
Imports osi.root.connector

Partial Public Class adapter
    Public Shared Function auto_heartbeat(ByVal i As ikeyvt(Of _false)) As ikeyvt(Of _false)
        If i Is Nothing OrElse TypeOf i Is auto_heartbeat_ikeyvt_false Then
            Return i
        Else
            Return New auto_heartbeat_ikeyvt_false(i)
        End If
    End Function
End Class
