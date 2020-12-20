
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class annotated_ref(Of K, T)
    Public ReadOnly v As T

    Public Sub New(ByVal v As T)
        Me.v = v
    End Sub
End Class

Public NotInheritable Class annotated_ref(Of K)
    Public Shared Function [with](Of T)(ByVal v As T) As annotated_ref(Of K, T)
        Return New annotated_ref(Of K, T)(v)
    End Function

    Private Sub New()
    End Sub
End Class
