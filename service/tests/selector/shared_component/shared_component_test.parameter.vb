
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector

Partial Public Class shared_component_test
    Public Class parameter
        Public ReadOnly local_port As UInt16
        Public ReadOnly allow_new_component As Boolean

        Public Sub New(ByVal local_port As UInt16, ByVal allow_new_component As Boolean)
            Me.local_port = local_port
            Me.allow_new_component = allow_new_component
        End Sub

        Public Sub New(ByVal allow_new_component As Boolean)
            Me.New(If(rnd_bool(), min_uint16, max_uint16), allow_new_component)
            assert(Not component.is_valid_port(Me.local_port))
        End Sub
    End Class
End Class
