
Imports osi.root.constants
Imports osi.root.connector

Partial Public Class shared_component_test
    Public Class parameter
        Public ReadOnly local_port As UInt16

        Public Sub New(ByVal local_port As UInt16)
            Me.local_port = local_port
        End Sub

        Public Sub New()
            Me.New(If(rnd_bool(), min_uint16, max_uint16))
            assert(Not component.is_valid_port(Me.local_port))
        End Sub
    End Class
End Class
