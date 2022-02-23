
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.envs

Public NotInheritable Class listeners
    Inherits _46_collection(Of listener, _New)

    Public NotInheritable Class _New
        Inherits __do(Of powerpoint, UInt16, listener, Boolean)

        Public Overrides Function at(ByRef i As powerpoint, ByRef j As UInt16, ByRef k As listener) As Boolean
            Return listener.[New](i, j, k)
        End Function
    End Class

    Public Shared Sub listen(ByVal p As powerpoint)
        assert(p IsNot Nothing)
        assert(p.local_defined() AndAlso Not p.remote_defined())
        Dim l As listener = Nothing
        If [New](p, p.local_port, l) Then
            l.bind()
            AddHandler p.udp_dev_manual_device_exporter().after_stop,
                       Sub()
                           assert(l.release())
                           assert([erase](p))
                       End Sub
        ElseIf udp_trace Then
            raise_error(error_type.warning,
                        "failed to listen for ",
                        p.identity,
                        ", this usually means another process is using this port")
        End If
    End Sub

    Private Sub New()
        MyBase.New()
    End Sub
End Class
