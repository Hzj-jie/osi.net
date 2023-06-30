
Imports osi.root.formation

Friend Class math_expression
    Public Class error_defination
        'coming from calculator_error
        Public Const divide_by_zero As String = "divide by zero"
        Public Const imaginary_number As String = "imaginary number"
        Public Const overflow As String = "overflow"
        Public Const operand_mismatch As String = "operand mismatch"
        Public Const bracket_mismatch As String = "bracket mismatch"
        'coming from expression_result
        Public Const lex_error As String = "expression lexing error"
        Public Const parse_error As String = "expression parsing error"
    End Class

    Public Shared ReadOnly predefined_cases(,) As String = {{"1+2", "3"},
                                                            {"1+2*3", "7"},
                                                            {"(1+2)*3", "9"},
                                                            {"(1-(2+3))*4", "-16"},
                                                            {"(((1*2)+3)-4)*5", "5"},
                                                            {"100+200", "300"},
                                                            {"    200 +   300        ", "500"},
                                                            {"(    33 +              44) / 20", "3"},
                                                            {"1 < 2", "-1"},
                                                            {"1 <= 2", "-1"},
                                                            {"1 > 2", "0"},
                                                            {"1 >= 2", "0"},
                                                            {"1 == 2", "0"},
                                                            {"1 != 2", "-1"},
                                                            {"1 <> 2", "-1"},
                                                            {"1 == 1", "-1"},
                                                            {"35 % 3", "2"},
                                                            {"2 << 1", "4"},
                                                            {"2 >> 1", "1"},
                                                            {"3 ^ 5", "243"},
                                                            {"9 /_ 2", "3"}}

    Public Shared ReadOnly predefined_cases2(,) As String = {{"-1+2", "1"},
                                                             {"- 1 -  2", "-3"},
                                                             {"1*-3", "-3"},
                                                             {"1*(-3)", "-3"},
                                                             {"1--2", "3"},
                                                             {"1+2.|2", "11"}}

    Public Shared ReadOnly failure_cases(,) As String = {{"1 2 +", error_defination.operand_mismatch},
                                                         {"1--2", error_defination.operand_mismatch}}

    Public Shared ReadOnly failure_cases2() As pair(Of String(), String) = {
                                pair.of({"-1", "/_", "2"}, error_defination.imaginary_number)}

    'expression can handle - correctly, but 1 2 + will trigger parse_error instead of operand_mismatch
    Public Shared ReadOnly failure_cases3(,) As String = {{"1 2+", error_defination.parse_error},
                                                          {"-1/_2", error_defination.imaginary_number},
                                                          {"1-*3", error_defination.parse_error}}
End Class
