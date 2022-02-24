
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation

'this is a handwriting expression interpreter, support very basic functionalities, mainly for test purpose
Public Class basic_expression(Of T)
    Private Const default_base As Byte = 10

    Private Class operators
        Public Const increment As String = "+"
        Public Const decrement As String = "-"
        Public Const multiply As String = "*"
        Public Const divide As String = "/"
        Public Const [mod] As String = "%"
        Public Const less As String = "<"
        Public Const equal As String = "=="
        Public Const less_or_equal As String = "<="
        Public Shared ReadOnly not_equals() As String = {"<>", "!="}
        Public Const more As String = ">"
        Public Const more_or_equal As String = ">="
        Public Const left_shift As String = "<<"
        Public Const right_shift As String = ">>"
        Public Const power As String = "^"
        Public Const extract As String = "/_"
        Public Shared ReadOnly left_brackets() As String = {"{", "[", "("}
        Public Shared ReadOnly right_brackets() As String = {"}", "]", ")"}

        Private Shared ReadOnly all As vector(Of String)

        Shared Sub New()
            all = New vector(Of String)()
            all.emplace_back(increment)
            all.emplace_back(decrement)
            all.emplace_back(multiply)
            all.emplace_back(divide)
            all.emplace_back([mod])
            all.emplace_back(less)
            all.emplace_back(equal)
            all.emplace_back(less_or_equal)
            all.emplace_back(not_equals)
            all.emplace_back(more)
            all.emplace_back(more_or_equal)
            all.emplace_back(left_shift)
            all.emplace_back(right_shift)
            all.emplace_back(power)
            all.emplace_back(extract)
            all.emplace_back(left_brackets)
            all.emplace_back(right_brackets)
        End Sub

        Public Shared Function size() As UInt32
            Return all.size()
        End Function

        Public Shared Function at(ByVal i As UInt32) As String
            assert(i < size())
            Return all(i)
        End Function

        Public Shared Function is_operator(ByVal i As String) As Boolean
            Return (+all).has(i)
        End Function
    End Class

    Private ReadOnly c As calculator(Of T)
    Private ReadOnly n As iparser(Of T)
    Private ReadOnly o As ioutputter(Of T)

    Public Sub New(ByVal c As calculator(Of T),
                   ByVal n As iparser(Of T),
                   ByVal o As ioutputter(Of T))
        assert(Not c Is Nothing)
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        Me.c = c
        Me.n = n
        Me.o = o
    End Sub

    Private Shared Sub emplace_clear(ByVal o As vector(Of String), ByVal v As vector(Of Char))
        assert(Not o Is Nothing)
        assert(Not v Is Nothing)
        If Not v.empty() Then
            o.emplace_back(v.str(""))
            v.clear()
        End If
    End Sub

    Private Shared Function parse(ByVal input As String, ByRef o As vector(Of String)) As Boolean
        o.renew()
        Dim v As vector(Of Char) = Nothing
        v = New vector(Of Char)()
        For i As Int32 = 0 To strlen(input) - 1
            If input(i).digit() Then
                v.emplace_back(input(i))
            ElseIf Not input(i).space() Then
                Dim longest_match As Int32 = 0
                Dim match_index As UInt32 = 0
                longest_match = npos
                assert(operators.size() > 0)
                For j As UInt32 = 0 To operators.size() - uint32_1
                    If strsame(input, CUInt(i), operators.at(j), uint32_0, strlen(operators.at(j))) Then
                        If strlen(operators.at(j)) > longest_match Then
                            longest_match = strlen(operators.at(j))
                            match_index = j
                        End If
                    End If
                Next
                If longest_match = npos Then
                    Return False
                Else
                    emplace_clear(o, v)
                    o.emplace_back(operators.at(match_index))
                    i += longest_match - 1
                End If
            End If
        Next
        emplace_clear(o, v)
        Return True
    End Function

    Private Function emplace(ByVal w() As String) As expression_result(Of T)
        Dim err As calculator_error = Nothing
        assert(array_size(w) > 0)
        For i As UInt32 = 0 To array_size(w) - uint32_1
            If String.IsNullOrEmpty(w(i)) Then
                Return expression_result(Of T).parse_error_result
            Else
                w(i) = w(i).TrimStart()
                If operators.is_operator(w(i)) Then
                    Select Case w(i)
                        Case operators.increment
                            c.emplace(calculator(Of T).operator.increment, err)
                        Case operators.decrement
                            c.emplace(calculator(Of T).operator.decrement, err)
                        Case operators.multiply
                            c.emplace(calculator(Of T).operator.multiply, err)
                        Case operators.divide
                            c.emplace(calculator(Of T).operator.divide, err)
                        Case operators.mod
                            c.emplace(calculator(Of T).operator.mod, err)
                        Case operators.less
                            c.emplace(calculator(Of T).operator.less, err)
                        Case operators.equal
                            c.emplace(calculator(Of T).operator.equal, err)
                        Case operators.less_or_equal
                            c.emplace(calculator(Of T).operator.less_or_equal, err)
                        Case operators.more
                            c.emplace(calculator(Of T).operator.more, err)
                        Case operators.more_or_equal
                            c.emplace(calculator(Of T).operator.more_or_equal, err)
                        Case operators.left_shift
                            c.emplace(calculator(Of T).operator.left_shift, err)
                        Case operators.right_shift
                            c.emplace(calculator(Of T).operator.right_shift, err)
                        Case operators.power
                            c.emplace(calculator(Of T).operator.power, err)
                        Case operators.extract
                            c.emplace(calculator(Of T).operator.extract, err)
                        Case Else
                            If operators.not_equals.has(w(i)) Then
                                c.emplace(calculator(Of T).operator.not_equal, err)
                            ElseIf operators.left_brackets.has(w(i)) Then
                                c.emplace(calculator(Of T).bracket.left, err)
                            ElseIf operators.right_brackets.has(w(i)) Then
                                c.emplace(calculator(Of T).bracket.right, err)
                            Else
                                assert(False)
                            End If
                    End Select
                    If err.has_error() Then
                        Return expression_result(Of T).calculator_error_result(err)
                    End If
                Else
                    Dim r As T = Nothing
                    If n.parse(w(i), default_base, r) Then
                        c.emplace(r, err)
                        If err.has_error() Then
                            Return expression_result(Of T).calculator_error_result(err)
                        End If
                    Else
                        Return expression_result(Of T).parse_error_result
                    End If
                End If
            End If
        Next
        Return Nothing
    End Function

    Public Function execute(ByVal w() As String) As expression_result(Of T)
        If isemptyarray(w) Then
            Return expression_result(Of T).parse_error_result
        Else
            c.reset()
            Dim er As expression_result(Of T) = Nothing
            er = emplace(w)
            If er Is Nothing Then
                Dim r As T = Nothing
                Dim err As calculator_error = Nothing
                r = c.execute(err)
                If err.has_error() Then
                    Return expression_result(Of T).calculator_error_result(err)
                Else
                    Return expression_result(Of T).expression_result(r, o, default_base)
                End If
            Else
                Return er
            End If
        End If
    End Function

    Public Function execute(ByVal w As vector(Of String)) As expression_result(Of T)
        Return execute(+w)
    End Function

    Public Function execute(ByVal input As String) As expression_result(Of T)
        Dim w As vector(Of String) = Nothing
        If parse(input, w) Then
            Return execute(w)
        Else
            Return expression_result(Of T).lex_error_result
        End If
    End Function
End Class
