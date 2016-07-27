
Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public Class matching_group_test
        Inherits matching_test

        Protected Overrides Function create() As matching_case()
            Return {New matching_case(New matching_group(0, 1, 2), False, 0, {3, 4, 5}),
                    New matching_case(New matching_group(0, 1, 2), True, 1, {0, 1, 2}),
                    New matching_case(New matching_group(0, 1, 2), True, 1, {1, 2, 0}),
                    New matching_case(New matching_group(0, 1, 2), True, 1, {2, 0, 1}),
                    New matching_case(New matching_group(0, 1, 2), False, 3, 3, {0, 1, 2})}
        End Function
    End Class
End Namespace
