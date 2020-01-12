
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class arrays
    Public Shared Function type_erasure(Of T, IT As T)(ByVal i() As IT) As T()
        If i Is Nothing Then
            Return Nothing
        End If
        Dim o() As T = Nothing
        ReDim o(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            o(j) = i(j)
        Next
        Return o
    End Function

    Private Sub New()
    End Sub
End Class
