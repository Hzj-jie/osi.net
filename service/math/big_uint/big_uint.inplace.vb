
Imports osi.root.connector

Partial Public Class big_uint
    Public Sub work_on(ByVal v() As UInt32)
        Me.v.replace_by(v)
        remove_extra_blank()
    End Sub

    Public Function work_on(ByVal v() As Byte) As Boolean
        Dim o() As UInt32 = Nothing
        If v.uint32_array(o) Then
            work_on(o)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function release() As UInt32()
        Dim r() As UInt32 = Nothing
        v.shrink_to_fit()
        r = v.data()
        v.replace_by(Nothing)
        Return r
    End Function

    Public Function release_as_bytes() As Byte()
        Dim o() As UInt32 = Nothing
        o = release()
        Return o.byte_array()
    End Function
End Class
