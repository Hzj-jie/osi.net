
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class streams
    Public Shared Function [of](Of T)(ByVal i() As T) As stream(Of T)
        Return New stream(Of T)(container_operator.enumerators.from_array(i))
    End Function

    Private Sub New()
    End Sub
End Class
