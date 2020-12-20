
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class func_t
    Public Shared Function [of](Of T)(ByVal i As T) As Func(Of T)
        Return Function() As T
                   Return i
               End Function
    End Function

    Private Sub New()
    End Sub
End Class
