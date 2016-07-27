
Partial Public Class adapter
    Public Shared Function stopable_wrappered(ByVal i As istrkeyvt) As istrkeyvt
        If i Is Nothing OrElse TypeOf i Is stopable_istrkeyvt Then
            Return i
        Else
            Return New stopable_istrkeyvt(i)
        End If
    End Function
End Class
