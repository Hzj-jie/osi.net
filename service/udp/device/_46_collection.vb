
Imports System.Net
Imports osi.root.template
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.formation

Public MustInherit Class _46_collection(Of T, __NEW As __do(Of powerpoint, T, Boolean))
    Public Const port_count As Int32 = IPEndPoint.MaxPort - IPEndPoint.MinPort + 1
    Private Shared ReadOnly _new As _do(Of powerpoint, T, Boolean)
    Private Shared ReadOnly v4 As arrayless(Of T)
    Private Shared ReadOnly v6 As arrayless(Of T)

    Shared Sub New()
        _new = +alloc(Of __NEW)()
        v4 = New arrayless(Of T)(port_count)
        v6 = New arrayless(Of T)(port_count)
    End Sub

    Public Shared Function [New](ByVal p As powerpoint, ByRef o As T) As Boolean
        If Not p Is Nothing Then
            Dim a As arrayless(Of T) = Nothing
            a = If(p.ipv4, v4, v6)
            Return a.[New](p.local_port,
                           Function() As T
                               Dim r As T = Nothing
                               If _new(unref(p), r) Then
                                   Return r
                               Else
                                   Return Nothing
                               End If
                           End Function,
                           o)
        Else
            Return False
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint) As T
        Dim c As T = Nothing
        assert([New](p, c))
        Return c
    End Function

    Protected Sub New()
    End Sub
End Class
