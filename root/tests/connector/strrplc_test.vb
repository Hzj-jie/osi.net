
Imports osi.root.connector
Imports osi.root.utt

Public Class strrplc_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim o As String = Nothing
        assertion.is_true(strrplc("abc", 1, "x", o))
        assertion.equal(o, "axc")
        assertion.is_true(strrplc("abc", 1, 2, "x", o))
        assertion.equal(o, "ax")
        assertion.is_true(strrplc("abcde", 1, "xyz", o))
        assertion.equal(o, "axyze")
        assertion.is_true(strrplc("abcde", 1, 2, "xyz", o))
        assertion.equal(o, "axyzde")
        assertion.is_true(strrplc("abcde", 1, "xy", o))
        assertion.equal(o, "axyde")
        assertion.is_true(strrplc("abcde", 1, 2, "xy", o))
        assertion.equal(o, "axyde")
        Return True
    End Function
End Class
