
Imports osi.root.template
Imports osi.root.connector

Public Module _adapter
    Public Function adapt(ByVal i As isynckeyvalue) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapt(New isynckeyvalue_ikeyvalue(i))
        End If
    End Function

    Public Function adapt(Of SEEK_RESULT)(ByVal i As isynckeyvalue2(Of seek_result)) As istrkeyvt
        If i Is Nothing Then
            Return Nothing
        Else
            Return adapt(New isynckeyvalue2_isynckeyvalue(Of SEEK_RESULT)(i))
        End If
    End Function

    Public Function adapt(ByVal i As ikeyvalue) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapt(New ikeyvalue_ikeyvt_false(i))
        End If
    End Function

    Public Function adapt(Of SEEK_RESULT)(ByVal i As ikeyvalue2(Of SEEK_RESULT)) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapt(New ikeyvalue2_ikeyvalue(Of SEEK_RESULT)(i))
        End If
    End Function

    Public Function adapt(Of SEEK_RESULT)(ByVal i As ikeyvt2(Of SEEK_RESULT)) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapt(New ikeyvt2_ikeyvt_false(Of SEEK_RESULT)(i))
        End If
    End Function

    Public Function adapt(ByVal i As ikeyvt(Of _false)) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapt(New ikeyvt_false_ikeyvt_true(adapter.auto_heartbeat(i)))
        End If
    End Function

    Public Function adapt(ByVal i As ikeyvt(Of _true)) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapt(New ikeyvt_true_istrkeyvt(i))
        End If
    End Function

    Public Function adapt(ByVal i As istrkeyvt) As istrkeyvt
        If i Is Nothing Then
            Return i
        Else
            Return adapter.input_validation_wrappered(i)
        End If
    End Function
End Module
