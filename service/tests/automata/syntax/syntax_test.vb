
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public NotInheritable Class syntax_test
        Inherits matching_test

        Protected Overrides Function create() As matching_test.matching_case()
            Return {New matching_case(New syntax(c, 1, 2, 3), False, 0, {4}),
                    New matching_case(New syntax(c, 1, 2, 3), True, 3, {1, 2, 3}),
                    New matching_case(New syntax(c, 1, 2, 3), True, 3, {1, 2, 3, 4}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), True, 3, {1, 3, 5}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), True, 3, {2, 4, 6}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), True, 3, {1, 3, 6}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), False, 1, {1, 2, 5}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), False, 2, {2, 4, 4}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), False, 0, {3, 3, 6}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), True, 3, {1, 3, 5}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), True, 3, {2, 4, 6}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), True, 3, {1, 3, 6}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}),
                                      False,
                                      5,
                                      {0, 0, 1, 0, 3, 7}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}), False, 2, {2, 0, 2, 0, 6}),
                    New matching_case(New syntax(c, {1, 2}, {3, 4}, {5, 6}),
                                      False,
                                      4,
                                      {1, 0, 0, 0, 5, 0, 6})}
        End Function
    End Class
End Namespace
