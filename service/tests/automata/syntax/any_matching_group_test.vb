
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public NotInheritable Class any_matching_group_test
        Inherits matching_test

        Protected Overrides Function create() As matching_test.matching_case()
            Return {New matching_case(New any_matching_group(c, 0), True, 0, {1}),
                    New matching_case(New any_matching_group(c, 0), True, 1, {0}),
                    New matching_case(New any_matching_group(c, 0), True, 2, {0, 0}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 1, {0}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 1, {1}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 1, {2}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 2, {0, 1}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 2, {1, 2}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 2, {2, 0}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 3, {0, 1, 2}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 3, {1, 2, 0}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 3, {2, 0, 1}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 4, {0, 1, 2, 0}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 4, {1, 2, 0, 1}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 4, {2, 0, 1, 2}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 4, {0, 1, 2, 0, 3}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 4, {1, 2, 0, 1, 3}),
                    New matching_case(New any_matching_group(c, {0, 1, 2}), True, 4, {2, 0, 1, 2, 3})}
        End Function
    End Class
End Namespace
