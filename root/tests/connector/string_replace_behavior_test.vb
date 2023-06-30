
Imports osi.root.connector
Imports osi.root.utt

Public Class string_replace_behavior_test
    Inherits [case]

    Private Shared Function case1() As Boolean
        Dim l As String = Nothing
        l = "abc"
        Dim r As String = Nothing
        r = l.Replace("a", "b")
        assertion.equal(object_compare(l, r), object_compare_undetermined)
        Return True
    End Function

    Private Shared Function case2() As Boolean
        Dim l As String = Nothing
        l = "abc"
        Dim r As String = Nothing
        r = l.Replace("d", "e")
        assertion.equal(object_compare(l, r), 0)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2()
    End Function
End Class
