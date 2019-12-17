
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports nl = osi.service.automata.nlexer

Namespace nlexer
    <test>
    Public NotInheritable Class nlexer_a_function_test
        <test>
        Private Shared Sub run()
            Dim r As [optional](Of nl) = Nothing
            r = nl.of(
                "void void",
                "name [\w,_]+",
                "left-bracket (",
                "right-bracket )",
                "left-brace {",
                "right-brace }",
                "assignment =",
                "equal ==",
                "add +",
                "raw-string ""[\"",*|""]*""",
                "int [+,-]?[\d]+",
                "float [+,-]?[\d]*.[\d]+",
                "comma \,",
                "semi-colon ;",
                "space [\b]+")
            If Not assertions.of(r).has_value() Then
                Return
            End If
            Dim v As [optional](Of vector(Of nl.result)) = Nothing
            v = (+r).match(
                "void function_a(type1 param1, type2 param2) {",
                "  string s = ""a bc\"""";",
                "  double x = -1.9;",
                "  int y = +100;",
                "  return param1 + param2 + s;",
                "}")
            assertions.of(v).has_value()
            ' TODO: assert results
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
