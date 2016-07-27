
Imports osi.root.connector
Imports osi.root.utt

Public Class strrplc_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim o As String = Nothing
        assert_true(strrplc("abc", 1, "x", o))
        assert_equal(o, "axc")
        assert_true(strrplc("abc", 1, 2, "x", o))
        assert_equal(o, "ax")
        assert_true(strrplc("abcde", 1, "xyz", o))
        assert_equal(o, "axyze")
        assert_true(strrplc("abcde", 1, 2, "xyz", o))
        assert_equal(o, "axyzde")
        assert_true(strrplc("abcde", 1, "xy", o))
        assert_equal(o, "axyde")
        assert_true(strrplc("abcde", 1, 2, "xy", o))
        assert_equal(o, "axyde")
        Return True
    End Function
End Class
