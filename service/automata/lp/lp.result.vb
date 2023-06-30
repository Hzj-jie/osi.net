
Imports osi.root.template
Imports osi.root.connector

Partial Public Class lp(Of MAX_TYPE As _int64, RESULT_T)
    Public Class result
        Public Shared ReadOnly lex_error_result As result
        Public Shared ReadOnly parse_error_result As result
        Public Shared ReadOnly success As result

        Shared Sub New()
            lex_error_result = New result(lex_error:=True)
            parse_error_result = New result(parse_error:=True)
            success = New result()
        End Sub

        Public ReadOnly lex_error As Boolean
        Public ReadOnly parse_error As Boolean
        Public ReadOnly result As RESULT_T

        Private Sub New(Optional ByVal lex_error As Boolean = False,
                        Optional ByVal parse_error As Boolean = False,
                        Optional ByVal result As RESULT_T = Nothing)
            Me.lex_error = lex_error
            Me.parse_error = parse_error
            Me.result = result
        End Sub

        Public Shared Function succeeded(ByVal result As RESULT_T) As result
            assert(Not result Is Nothing)
            Return New result(result:=result)
        End Function

        Public Function has_error() As Boolean
            Return lex_error OrElse
                   parse_error
        End Function
    End Class
End Class
