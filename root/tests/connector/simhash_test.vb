
Imports osi.root.connector
Imports osi.root.utt

Public Class simhash_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assert_equal(similarity("ABC", "ABC"), 1)
        assert_more_or_equal_and_less(similarity("ABC", "DEF"), 0, 1)
        Return True
    End Function
End Class
