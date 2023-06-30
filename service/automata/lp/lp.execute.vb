
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class lp(Of MAX_TYPE As _int64, RESULT_T)
    Public Function execute(ByVal input As String, ByVal r As RESULT_T) As result
        Dim words As vector(Of lexer.word) = Nothing
        If l.parse(input, words) Then
            If p.parse(+words, r) Then
                Return If(r Is Nothing, result.success, result.succeeded(r))
            Else
                Return result.parse_error_result
            End If
        Else
            Return result.lex_error_result
        End If
    End Function

    Private Function execute(ByVal input As String, ByVal ctor As Func(Of RESULT_T)) As result
        assert(Not ctor Is Nothing)
        Dim r As RESULT_T = Nothing
        r = ctor()
        assert(Not r Is Nothing)
        Return execute(input, r)
    End Function

    Public Function execute(ByVal input As String) As result
        Return execute(input, Function() alloc(Of RESULT_T)())
    End Function

    Public Function execute(Of T As {New, RESULT_T})(ByVal input As String) As result
        Return execute(input, Function() New T())
    End Function
End Class
