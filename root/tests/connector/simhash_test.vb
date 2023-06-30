
Imports osi.root.connector
Imports osi.root.utt

Public Class simhash_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.equal(similarity("ABC", "ABC"), 1)
        assertion.more_or_equal_and_less(similarity("ABC", "DEF"), 0, 1)
        Return True
    End Function
End Class
