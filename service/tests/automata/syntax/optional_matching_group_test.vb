
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public NotInheritable Class optional_matching_group_test
        Inherits matching_test

        Protected Overrides Function create() As matching_test.matching_case()
            Return {New matching_case(New optional_matching_group(c, 0), True, 0, {1}),
                    New matching_case(New optional_matching_group(c, 0), True, 1, {0}),
                    New matching_case(New optional_matching_group(c, 0), True, 1, {0, 0}),
                    New matching_case(New optional_matching_group(c, 0), True, 1, {0, 0, 0})}
        End Function
    End Class
End Namespace
