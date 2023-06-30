
Imports osi.root.connector

Partial Public Class adapter
    Public Shared Function cached(ByVal i As isynckeyvalue,
                                  Optional ByVal cached_count As UInt64 = 4096,
                                  Optional ByVal max_value_size As UInt64 = (1 << 20)) As ikeyvalue
        If i Is Nothing Then
            Return i
        Else
            Return cached(New isynckeyvalue_ikeyvalue(i), cached_count, max_value_size)
        End If
    End Function

    Public Shared Function cached(ByVal i As ikeyvalue,
                                  Optional ByVal cached_count As UInt64 = 4096,
                                  Optional ByVal max_value_size As UInt64 = (1 << 20)) As ikeyvalue
        If i Is Nothing OrElse TypeOf i Is cached_ikeyvalue Then
            Return i
        Else
            Return New cached_ikeyvalue(i, cached_count, max_value_size)
        End If
    End Function
End Class
