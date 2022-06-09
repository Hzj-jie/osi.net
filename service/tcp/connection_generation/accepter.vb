
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

'public for test, do not try to use it directly
Partial Public NotInheritable Class accepter
    Private Const port_count As Int32 = IPEndPoint.MaxPort - IPEndPoint.MinPort + 1
    Private Shared ReadOnly adapters As arrayless(Of adapter)

    Shared Sub New()
        adapters = New arrayless(Of adapter)(port_count)
    End Sub

    Public Shared Function attached_powerpoint_count() As UInt32
        Dim o As UInt32 = 0
        For i As UInt32 = 0 To adapters.size() - uint32_1
            Dim a As adapter = Nothing
            If adapters.get(i, a) AndAlso Not a Is Nothing Then
                o += a.attached_powerpoint_count()
            End If
        Next
        Return o
    End Function

    Public Shared Function port_attached(ByVal p As UInt32) As Boolean
        Dim a As adapter = Nothing
        If Not adapters.get(p, a) Then
            Return False
        End If
        Return Not a Is Nothing AndAlso a.attached_powerpoint_count() > 0
    End Function

    Public Shared Sub listen(ByVal p As powerpoint)
        assert(Not p Is Nothing)
        assert(Not p.is_outgoing)
        assert(p.host_or_ip.null_or_empty())
        adapters.[New](p.port,
                       Function() As adapter
                           Return New adapter(p)
                       End Function).attach(p)
    End Sub
End Class
