
Imports osi.root.connector
Imports osi.root.utt

Public Class str_bool_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assert_true("true".true())
        assert_true("   tRuE     ".true())
        assert_true("1".true())
        assert_true(" 1   ".true())
        assert_true("                        yes ".true())
        assert_false("  10".true())
        assert_false(DirectCast(Nothing, String).true())
        assert_false("     no ".true())
        assert_false("no".true())
        Return True
    End Function
End Class
