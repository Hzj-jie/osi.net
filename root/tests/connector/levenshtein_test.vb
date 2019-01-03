
Imports osi.root.utt
Imports osi.root.connector

Public Class levenshtein_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.equal(levenshtein("ABC", "ABC", True), 0)
        assertion.equal(levenshtein("ABC", "abc", False), 0)
        assertion.equal(levenshtein(Nothing, Nothing, True), 0)
        assertion.equal(levenshtein(Nothing, Nothing, False), 0)
        assertion.equal(levenshtein("ABC", Nothing, True), 3)
        assertion.equal(levenshtein("ABC", Nothing, False), 3)
        assertion.equal(levenshtein("ABC", String.Empty, True), 3)
        assertion.equal(levenshtein("ABC", String.Empty, False), 3)
        assertion.equal(levenshtein("ABC", "ABCD", True), 1)
        assertion.equal(levenshtein("ABC", "abcd", False), 1)
        assertion.equal(levenshtein("ABC", "ABCD", False), 1)
        assertion.equal(levenshtein("ABC", "abcd", True), 4)
        assertion.equal(levenshtein("ABC", "ADC", True), 1)
        assertion.equal(levenshtein("ABC", "adc", False), 1)
        assertion.equal(levenshtein("kitten", "sitten", True), 1)
        assertion.equal(levenshtein("sitten", "sittin", True), 1)
        assertion.equal(levenshtein("sittin", "sitting", True), 1)
        assertion.equal(levenshtein("kitten", "sitting", True), 3)
        assertion.equal(levenshtein("Saturday", "Sunday", True), 3)
        Return True
    End Function
End Class
