
Imports osi.root.connector
Imports osi.service.automata

Partial Public Class expression(Of T)
    'this should be consistent with the ones in expression.syntax.txt
    Private Class [operator]
        Public Const increment As String = "+"
        Public Const decrement As String = "-"
        Public Const multiply As String = "*"
        Public Const divide As String = "/"
        Public Const [mod] As String = "%"
        Public Const less As String = "<"
        Public Const equal As String = "=="
        Public Const less_or_equal As String = "<="
        Public Const not_equal As String = "<>"
        Public Const not_equal2 As String = "!="
        Public Const more As String = ">"
        Public Const more_or_equal As String = ">="
        Public Const left_shift As String = "<<"
        Public Const right_shift As String = ">>"
        Public Const power As String = "^"
        Public Const extract As String = "/_"
    End Class
End Class
