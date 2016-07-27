
Imports osi.root.connector
Imports osi.root.template

Partial Public Class adapter
    Public Shared Function input_validation_wrappered(ByVal i As isynckeyvalue) As isynckeyvalue
        If i Is Nothing OrElse TypeOf i Is isynckeyvalue_input_validation_wrapper Then
            Return i
        Else
            Return New isynckeyvalue_input_validation_wrapper(i)
        End If
    End Function

    Public Shared Function input_validation_wrappered(ByVal i As ikeyvalue) As ikeyvalue
        If i Is Nothing OrElse TypeOf i Is ikeyvalue_input_validation_wrapper Then
            Return i
        Else
            Return New ikeyvalue_input_validation_wrapper(i)
        End If
    End Function

    Public Shared Function input_validation_wrappered(Of TS As _boolean)(ByVal i As ikeyvt(Of TS)) As ikeyvt(Of TS)
        If i Is Nothing OrElse TypeOf i Is ikeyvt_input_validation_wrapper(Of TS) Then
            Return i
        Else
            Return New ikeyvt_input_validation_wrapper(Of TS)(i)
        End If
    End Function

    Public Shared Function input_validation_wrappered(ByVal i As istrkeyvt) As istrkeyvt
        If i Is Nothing OrElse TypeOf i Is istrkeyvt_input_validation_wrapper Then
            Return i
        Else
            Return New istrkeyvt_input_validation_wrapper(i)
        End If
    End Function
End Class
