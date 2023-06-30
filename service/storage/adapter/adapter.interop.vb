
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class adapter
    Public Shared Function isynckeyvalue2_ikeyvalue2(Of T)(ByVal i As isynckeyvalue2(Of T)) As ikeyvalue2(Of T)
        If i Is Nothing Then
            Return i
        Else
            Return New isynckeyvalue2_ikeyvalue2(Of T)(i)
        End If
    End Function

    Public Shared Function ikeyvalue2_ikeyvt2(Of T)(ByVal i As ikeyvalue2(Of T),
                                                    ByVal j As ikeyvalue2(Of T)) As ikeyvt2(Of pair(Of T, T))
        If i Is Nothing OrElse j Is Nothing Then
            If i Is Nothing Then
                Return i
            Else
                assert(j Is Nothing)
                Return j
            End If
        Else
            Return New ikeyvalue2_ikeyvt2(Of T)(i, j)
        End If
    End Function

    Public Shared Function ikeyvalue2_ikeyvalue(Of T)(ByVal i As ikeyvalue2(Of T)) As ikeyvalue
        If i Is Nothing Then
            Return i
        Else
            Return New ikeyvalue2_ikeyvalue(Of T)(i)
        End If
    End Function
End Class
