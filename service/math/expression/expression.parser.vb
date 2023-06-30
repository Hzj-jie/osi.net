
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.automata

Partial Public Class expression(Of T)
    Private Function parse_number_emplace(ByVal words() As lexer.typed_word,
                                          ByVal pos As UInt32,
                                          ByVal negative As Boolean) As Boolean
        Dim r As T = Nothing
        If n.parse(If(negative, [operator].decrement, Nothing) + words(pos).text(),
                   default_base,
                   r) Then
            c.emplace(r, err)
            Return Not err.has_error()
        Else
            Return False
        End If
    End Function

    Private Function parse_operator_emplace(ByVal words() As lexer.typed_word,
                                            ByVal pos As UInt32) As Boolean
        Dim op As calculator(Of T).operator = Nothing
        Select Case words(pos).text()
            Case [operator].decrement
                op = calculator(Of T).operator.decrement
            Case [operator].divide
                op = calculator(Of T).operator.divide
            Case [operator].equal
                op = calculator(Of T).operator.equal
            Case [operator].extract
                op = calculator(Of T).operator.extract
            Case [operator].increment
                op = calculator(Of T).operator.increment
            Case [operator].left_shift
                op = calculator(Of T).operator.left_shift
            Case [operator].less
                op = calculator(Of T).operator.less
            Case [operator].less_or_equal
                op = calculator(Of T).operator.less_or_equal
            Case [operator].mod
                op = calculator(Of T).operator.mod
            Case [operator].more
                op = calculator(Of T).operator.more
            Case [operator].more_or_equal
                op = calculator(Of T).operator.more_or_equal
            Case [operator].multiply
                op = calculator(Of T).operator.multiply
            Case [operator].not_equal
                op = calculator(Of T).operator.not_equal
            Case [operator].not_equal2
                op = calculator(Of T).operator.not_equal
            Case [operator].power
                op = calculator(Of T).operator.power
            Case [operator].right_shift
                op = calculator(Of T).operator.right_shift
            Case Else
                assert(False)
        End Select
        c.emplace(op, err)
        Return Not err.has_error()
    End Function

    Private Function parse_number_with_base_emplace(ByVal words() As lexer.typed_word,
                                                    ByVal pos As UInt32,
                                                    ByVal negative As Boolean) As Boolean
        Dim b As Byte = 0
        Dim r As T = Nothing
        If Byte.TryParse(words(pos + 2).text(), b) AndAlso
           n.parse(If(negative, [operator].decrement, Nothing) + words(pos).text(),
                   b,
                   r) Then
            c.emplace(r, err)
            Return Not err.has_error()
        Else
            Return False
        End If
    End Function

    Private Function output(ByVal base As Byte) As Boolean
        Me.base = base
        Return True
    End Function

    Private Function output() As Boolean
        Return output(default_base)
    End Function

    Private Function output_with_base(ByVal words() As lexer.typed_word,
                                      ByVal pos As UInt32) As Boolean
        Dim b As Byte = 0
        If Byte.TryParse(words(pos + 2).text(), b) Then
            Return output(b)
        Else
            Return False
        End If
    End Function

    Private Shared Function pass_left_bracket(ByVal words() As lexer.typed_word,
                                              ByVal pos As UInt32,
                                              ByVal result As expression(Of T)) As Boolean
        result.c.emplace(calculator(Of T).bracket.left, result.err)
        Return result.err.has_no_error()
    End Function

    Private Shared Function number_op(ByVal words() As lexer.typed_word,
                                      ByVal pos As UInt32,
                                      ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, False) AndAlso
               result.parse_operator_emplace(words, pos)
    End Function

    Private Shared Function neg_number_op(ByVal words() As lexer.typed_word,
                                          ByVal pos As UInt32,
                                          ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, True) AndAlso
               result.parse_operator_emplace(words, pos)
    End Function

    Private Shared Function number_with_base_op(ByVal words() As lexer.typed_word,
                                                ByVal pos As UInt32,
                                                ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), False) AndAlso
               result.parse_operator_emplace(words, pos)
    End Function

    Private Shared Function neg_number_with_base_op(ByVal words() As lexer.typed_word,
                                                    ByVal pos As UInt32,
                                                    ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), True) AndAlso
               result.parse_operator_emplace(words, pos)
    End Function

    Private Shared Function number_pass_right_bracket(ByVal words() As lexer.typed_word,
                                                      ByVal pos As UInt32,
                                                      ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, False) AndAlso
               pass_right_bracket(words, pos, result)
    End Function

    Private Shared Function pass_right_bracket(ByVal words() As lexer.typed_word,
                                               ByVal pos As UInt32,
                                               ByVal result As expression(Of T)) As Boolean
        result.c.emplace(calculator(Of T).bracket.right, result.err)
        Return result.err.has_no_error()
    End Function

    Private Shared Function neg_number_pass_right_bracket(ByVal words() As lexer.typed_word,
                                                          ByVal pos As UInt32,
                                                          ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, True) AndAlso
               pass_right_bracket(words, pos, result)
    End Function

    Private Shared Function number_with_base_pass_right_bracket(ByVal words() As lexer.typed_word,
                                                                ByVal pos As UInt32,
                                                                ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), False) AndAlso
               pass_right_bracket(words, pos, result)
    End Function

    Private Shared Function neg_number_with_base_pass_right_bracket(ByVal words() As lexer.typed_word,
                                                                    ByVal pos As UInt32,
                                                                    ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), True) AndAlso
               pass_right_bracket(words, pos, result)
    End Function

    Private Shared Function number_end(ByVal words() As lexer.typed_word,
                                       ByVal pos As UInt32,
                                       ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, False) AndAlso
               result.output()
    End Function

    Private Shared Function neg_number_end(ByVal words() As lexer.typed_word,
                                           ByVal pos As UInt32,
                                           ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, True) AndAlso
               result.output()
    End Function

    Private Shared Function number_with_base_end(ByVal words() As lexer.typed_word,
                                                 ByVal pos As UInt32,
                                                 ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), False) AndAlso
               result.output()
    End Function

    Private Shared Function neg_number_with_base_end(ByVal words() As lexer.typed_word,
                                                     ByVal pos As UInt32,
                                                     ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), True) AndAlso
               result.output()
    End Function

    Private Shared Function pnumber_end(ByVal words() As lexer.typed_word,
                                        ByVal pos As UInt32,
                                        ByVal result As expression(Of T)) As Boolean
        Return result.output()
    End Function

    Private Shared Function parse_number(ByVal words() As lexer.typed_word,
                                         ByVal pos As UInt32,
                                         ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, False)
    End Function

    Private Shared Function parse_neg_number(ByVal words() As lexer.typed_word,
                                             ByVal pos As UInt32,
                                             ByVal result As expression(Of T)) As Boolean
        assert(pos > 0)
        Return result.parse_number_emplace(words, pos - uint32_1, True)
    End Function

    Private Shared Function parse_number_with_base(ByVal words() As lexer.typed_word,
                                                   ByVal pos As UInt32,
                                                   ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), False)
    End Function

    Private Shared Function parse_neg_number_with_base(ByVal words() As lexer.typed_word,
                                                       ByVal pos As UInt32,
                                                       ByVal result As expression(Of T)) As Boolean
        assert(pos > 2)
        Return result.parse_number_with_base_emplace(words, pos - CUInt(3), True)
    End Function

    Private Shared Function output_with_base(ByVal words() As lexer.typed_word,
                                             ByVal pos As UInt32,
                                             ByVal result As expression(Of T)) As Boolean
        assert(pos > 1)
        Return result.output_with_base(words, pos - CUInt(2))
    End Function

    Private Shared Function output(ByVal words() As lexer.typed_word,
                                   ByVal pos As UInt32,
                                   ByVal result As expression(Of T)) As Boolean
        Return result.output()
    End Function

    Private Shared Function op(ByVal words() As lexer.typed_word,
                               ByVal pos As UInt32,
                               ByVal result As expression(Of T)) As Boolean
        Return result.parse_operator_emplace(words, pos)
    End Function
End Class
