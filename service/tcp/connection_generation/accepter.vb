
Imports System.Reflection
Imports System.Net.Sockets
Imports System.Net
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.envs
Imports osi.root.utils
Imports osi.root.constants

'public for test, do not try to use it directly
Partial Public Class accepter
    Private Const port_count As Int32 = IPEndPoint.MaxPort - IPEndPoint.MinPort + 1
    Private Shared ReadOnly adapters() As adapter

    Shared Sub New()
        ReDim adapters(port_count - 1)
    End Sub

    Public Shared Function attached_powerpoint_count() As Int64
        Dim o As Int64 = 0
        For i As Int32 = 0 To array_size(adapters) - 1
            If Not adapters(i) Is Nothing Then
                o += adapters(i).attached_powerpoint_count()
            End If
        Next
        Return o
    End Function

    Public Shared Function port_attached(ByVal p As Int32) As Boolean
        If p > IPEndPoint.MaxPort OrElse p < IPEndPoint.MinPort Then
            Return False
        Else
            Return Not adapters(p) Is Nothing AndAlso adapters(p).attached_powerpoint_count() > 0
        End If
    End Function

    Public Shared Sub listen(ByVal p As powerpoint)
        assert(Not p Is Nothing)
        assert(Not p.is_outgoing)
        assert(String.IsNullOrEmpty(p.host_or_ip))
        atomic.create_if_nothing(adapters(p.port), Function() New adapter(p))
        adapters(p.port).attach(p)
    End Sub
End Class
