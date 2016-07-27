
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.procedure
Imports osi.service.device
Imports osi.service.commander

Public Class responder
    Public Shared Function [New](ByVal p As powerpoint,
                                 ByVal e As executor,
                                 ByRef o As responder(Of _true),
                                 Optional ByVal stopping As Func(Of Boolean) = Nothing) As Boolean
        If p Is Nothing Then
            Return False
        Else
            o = New responder(Of _true)(p.herald_device_pool(), npos, e, stopping)
            Return True
        End If
    End Function

    Public Shared Function respond(ByVal p As powerpoint,
                                   ByVal e As executor,
                                   Optional ByVal stopping As Func(Of Boolean) = Nothing) As Boolean
        Dim o As responder(Of _true) = Nothing
        If [New](p, e, o, stopping) Then
            Return -o
        Else
            Return False
        End If
    End Function

    Private Sub New()
    End Sub
End Class
