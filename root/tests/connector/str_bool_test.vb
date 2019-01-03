
Imports osi.root.connector
Imports osi.root.utt

Public Class str_bool_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.is_true("true".true())
        assertion.is_true("   tRuE     ".true())
        assertion.is_true("1".true())
        assertion.is_true(" 1   ".true())
        assertion.is_true("                        yes ".true())
        assertion.is_false("  10".true())
        assertion.is_false(DirectCast(Nothing, String).true())
        assertion.is_false("     no ".true())
        assertion.is_false("no".true())
        Return True
    End Function
End Class
