
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.resource

Public NotInheritable Class math
    Public Shared Function pi_1k() As String
        Return pi_1k_holder.v
    End Function

    Public Shared Function e_1k() As String
        Return e_1k_holder.v
    End Function

    Private NotInheritable Class pi_1k_holder
        Public Shared ReadOnly v As String

        Shared Sub New()
            v = math_constants.pi_1k.as_text()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class e_1k_holder
        Public Shared ReadOnly v As String

        Shared Sub New()
            v = math_constants.e_1k.as_text()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
