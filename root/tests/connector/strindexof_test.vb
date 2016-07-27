
Imports osi.root.connector
Imports osi.root.utt

Public Class strindexof_test
    Inherits [case]

    Private Shared Function contains_any_test() As Boolean
        assert_true("abc".contains_any("d", "e", "a"))
        assert_true("abc".contains_any("d"c, "e"c, "a"c))
        assert_true("abc".contains_any({"d"}, {"e", "a"}))
        assert_true("abc".contains_any({"d"c}, {"e"c, "a"c}))
        assert_false("abc".contains_any("d", "e", "f"))
        assert_false("abc".contains_any("d"c, "e"c, "f"c))
        assert_false("abc".contains_any({"d", "e"}, {"f"}))
        assert_false("abc".contains_any({"d"c, "e"c}, {"f"c}))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return contains_any_test()
    End Function
End Class
