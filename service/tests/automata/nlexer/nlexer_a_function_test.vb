
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports nl = osi.service.automata.nlexer

Namespace nlexer
    <test>
    Public NotInheritable Class nlexer_a_function_test
        Private Shared ReadOnly rules() As String = {
                "return return",
                "name [\w,_][\w,\d,_]*",
                "left-bracket (",
                "right-bracket )",
                "left-brace {",
                "right-brace }",
                "assignment =",
                "equal ==",
                "raw-string ""[\"",*|""]*""",
                "float [+,-]?[\d]*.[\d]+",
                "int [+,-]?[\d]+",
                "add +",
                "comma \,",
                "semi-colon ;",
                "space [\b]+"
        }

        Private Shared ReadOnly program() As String = {
                "void function_a(type1 param1, type2 param2) {",
                "  string s = ""a bc\"""";",
                "  double x = -1.9;",
                "  int y = +100;",
                "  return param1 + param2 + s;",
                "}"
        }

        <test>
        Private Shared Sub original_result()
            Dim r As [optional](Of nl) = Nothing
            r = nl.of(rules)
            If Not assertions.of(r).has_value() Then
                Return
            End If
            Dim v As [optional](Of vector(Of nl.result)) = Nothing
            v = (+r).match(program)
            assertions.of(v).has_value(vector.of(
                (+r).result_of(0, 4, "name"),
                (+r).result_of(4, 5, "space"),
                (+r).result_of(5, 15, "name"),
                (+r).result_of(15, 16, "left-bracket"),
                (+r).result_of(16, 21, "name"),
                (+r).result_of(21, 22, "space"),
                (+r).result_of(22, 28, "name"),
                (+r).result_of(28, 29, "comma"),
                (+r).result_of(29, 30, "space"),
                (+r).result_of(30, 35, "name"),
                (+r).result_of(35, 36, "space"),
                (+r).result_of(36, 42, "name"),
                (+r).result_of(42, 43, "right-bracket"),
                (+r).result_of(43, 44, "space"),
                (+r).result_of(44, 45, "left-brace"),
                (+r).result_of(45, 48, "space"),
                (+r).result_of(48, 54, "name"),
                (+r).result_of(54, 55, "space"),
                (+r).result_of(55, 56, "name"),
                (+r).result_of(56, 57, "space"),
                (+r).result_of(57, 58, "assignment"),
                (+r).result_of(58, 59, "space"),
                (+r).result_of(59, 67, "raw-string"),
                (+r).result_of(67, 68, "semi-colon"),
                (+r).result_of(68, 71, "space"),
                (+r).result_of(71, 77, "name"),
                (+r).result_of(77, 78, "space"),
                (+r).result_of(78, 79, "name"),
                (+r).result_of(79, 80, "space"),
                (+r).result_of(80, 81, "assignment"),
                (+r).result_of(81, 82, "space"),
                (+r).result_of(82, 86, "float"),
                (+r).result_of(86, 87, "semi-colon"),
                (+r).result_of(87, 90, "space"),
                (+r).result_of(90, 93, "name"),
                (+r).result_of(93, 94, "space"),
                (+r).result_of(94, 95, "name"),
                (+r).result_of(95, 96, "space"),
                (+r).result_of(96, 97, "assignment"),
                (+r).result_of(97, 98, "space"),
                (+r).result_of(98, 102, "int"),
                (+r).result_of(102, 103, "semi-colon"),
                (+r).result_of(103, 106, "space"),
                (+r).result_of(106, 112, "return"),
                (+r).result_of(112, 113, "space"),
                (+r).result_of(113, 119, "name"),
                (+r).result_of(119, 120, "space"),
                (+r).result_of(120, 121, "add"),
                (+r).result_of(121, 122, "space"),
                (+r).result_of(122, 128, "name"),
                (+r).result_of(128, 129, "space"),
                (+r).result_of(129, 130, "add"),
                (+r).result_of(130, 131, "space"),
                (+r).result_of(131, 132, "name"),
                (+r).result_of(132, 133, "semi-colon"),
                (+r).result_of(133, 134, "space"),
                (+r).result_of(134, 135, "right-brace")))
        End Sub

        <test>
        Private Shared Sub filtered_result()
            Dim r As [optional](Of nl) = Nothing
            r = nl.of(rules)
            If Not assertions.of(r).has_value() Then
                Return
            End If
            Dim v As [optional](Of vector(Of nl.result)) = Nothing
            v = (+r).match(program, "space")
            assertions.of(v).has_value(vector.of(
                (+r).result_of(0, 4, "name"),
                (+r).result_of(5, 15, "name"),
                (+r).result_of(15, 16, "left-bracket"),
                (+r).result_of(16, 21, "name"),
                (+r).result_of(22, 28, "name"),
                (+r).result_of(28, 29, "comma"),
                (+r).result_of(30, 35, "name"),
                (+r).result_of(36, 42, "name"),
                (+r).result_of(42, 43, "right-bracket"),
                (+r).result_of(44, 45, "left-brace"),
                (+r).result_of(48, 54, "name"),
                (+r).result_of(55, 56, "name"),
                (+r).result_of(57, 58, "assignment"),
                (+r).result_of(59, 67, "raw-string"),
                (+r).result_of(67, 68, "semi-colon"),
                (+r).result_of(71, 77, "name"),
                (+r).result_of(78, 79, "name"),
                (+r).result_of(80, 81, "assignment"),
                (+r).result_of(82, 86, "float"),
                (+r).result_of(86, 87, "semi-colon"),
                (+r).result_of(90, 93, "name"),
                (+r).result_of(94, 95, "name"),
                (+r).result_of(96, 97, "assignment"),
                (+r).result_of(98, 102, "int"),
                (+r).result_of(102, 103, "semi-colon"),
                (+r).result_of(106, 112, "return"),
                (+r).result_of(113, 119, "name"),
                (+r).result_of(120, 121, "add"),
                (+r).result_of(122, 128, "name"),
                (+r).result_of(129, 130, "add"),
                (+r).result_of(131, 132, "name"),
                (+r).result_of(132, 133, "semi-colon"),
                (+r).result_of(134, 135, "right-brace")))
        End Sub

        <test>
        Private Shared Sub str_results()
            Dim r As [optional](Of nl) = Nothing
            r = nl.of(rules)
            If Not assertions.of(r).has_value() Then
                Return
            End If
            Dim v As [optional](Of vector(Of nl.str_result)) = Nothing
            v = (+r).match(program, "space").map(nl.str_result.map_from_str(program))
            assertions.of(v).has_value(vector.of(
                New nl.str_result("void", "name"),
                New nl.str_result("function_a", "name"),
                New nl.str_result("(", "left-bracket"),
                New nl.str_result("type1", "name"),
                New nl.str_result("param1", "name"),
                New nl.str_result(",", "comma"),
                New nl.str_result("type2", "name"),
                New nl.str_result("param2", "name"),
                New nl.str_result(")", "right-bracket"),
                New nl.str_result("{", "left-brace"),
                New nl.str_result("string", "name"),
                New nl.str_result("s", "name"),
                New nl.str_result("=", "assignment"),
                New nl.str_result("""a bc\""""", "raw-string"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("double", "name"),
                New nl.str_result("x", "name"),
                New nl.str_result("=", "assignment"),
                New nl.str_result("-1.9", "float"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("int", "name"),
                New nl.str_result("y", "name"),
                New nl.str_result("=", "assignment"),
                New nl.str_result("+100", "int"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("return", "return"),
                New nl.str_result("param1", "name"),
                New nl.str_result("+", "add"),
                New nl.str_result("param2", "name"),
                New nl.str_result("+", "add"),
                New nl.str_result("s", "name"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("}", "right-brace")
            ))
        End Sub

        Private Shared Function shuffled_rules() As String()
            Dim r() As String = Nothing
            r = rules.shuffle()
            Dim i As Int32 = 0
            i = r.index_of(rules(0))
            assert(i <> npos)
            If i <> 0 Then
                swap(r(0), r(i))
            End If
            Return r
        End Function

        <repeat(100)>
        <test>
        Private Shared Sub longest_match_str_results()
            Dim r As [optional](Of nl) = Nothing
            r = nl.of(shuffled_rules())
            If Not assertions.of(r).has_value() Then
                Return
            End If
            Dim v As [optional](Of vector(Of nl.str_result)) = Nothing
            v = (+r).match(program, "space").map(nl.str_result.map_from_str(program))
            assertions.of(v).has_value(vector.of(
                New nl.str_result("void", "name"),
                New nl.str_result("function_a", "name"),
                New nl.str_result("(", "left-bracket"),
                New nl.str_result("type1", "name"),
                New nl.str_result("param1", "name"),
                New nl.str_result(",", "comma"),
                New nl.str_result("type2", "name"),
                New nl.str_result("param2", "name"),
                New nl.str_result(")", "right-bracket"),
                New nl.str_result("{", "left-brace"),
                New nl.str_result("string", "name"),
                New nl.str_result("s", "name"),
                New nl.str_result("=", "assignment"),
                New nl.str_result("""a bc\""""", "raw-string"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("double", "name"),
                New nl.str_result("x", "name"),
                New nl.str_result("=", "assignment"),
                New nl.str_result("-1.9", "float"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("int", "name"),
                New nl.str_result("y", "name"),
                New nl.str_result("=", "assignment"),
                New nl.str_result("+100", "int"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("return", "return"),
                New nl.str_result("param1", "name"),
                New nl.str_result("+", "add"),
                New nl.str_result("param2", "name"),
                New nl.str_result("+", "add"),
                New nl.str_result("s", "name"),
                New nl.str_result(";", "semi-colon"),
                New nl.str_result("}", "right-brace")
            ))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
