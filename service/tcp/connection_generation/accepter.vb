
Imports System.Net
Imports osi.root.formation
Imports osi.root.connector

'public for test, do not try to use it directly
Partial Public Class accepter
    Private Const port_count As Int32 = IPEndPoint.MaxPort - IPEndPoint.MinPort + 1
    Private Shared ReadOnly adapters As arrayless(Of adapter)

    Shared Sub New()
        adapters = New arrayless(Of adapter)(port_count)
    End Sub

    Public Shared Function attached_powerpoint_count() As Int64
        Dim o As Int64 = 0
        For i As Int32 = 0 To adapters.size() - 1
            Dim a As adapter = Nothing
            If adapters.get(i, a) AndAlso a IsNot Nothing Then
                o += a.attached_powerpoint_count()
            End If
        Next
        Return o
    End Function

    Public Shared Function port_attached(ByVal p As Int32) As Boolean
        Dim a As adapter = Nothing
        If adapters.get(p, a) Then
            Return a IsNot Nothing AndAlso a.attached_powerpoint_count() > 0
        Else
            Return False
        End If
    End Function

    Public Shared Sub listen(ByVal p As powerpoint)
        assert(p IsNot Nothing)
        assert(Not p.is_outgoing)
        assert(String.IsNullOrEmpty(p.host_or_ip))
        adapters.[New](p.port,
                       Function() As adapter
                           Return New adapter(p)
                       End Function).attach(p)
    End Sub
End Class
