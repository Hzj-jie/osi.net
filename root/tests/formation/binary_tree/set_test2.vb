
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class set_test2
    <test>
    Private Shared Sub set_can_store_null()
        Dim s As [set](Of String) = Nothing
        s = [set].of("abc", "bcd", Nothing, Nothing, Nothing, "DEF")
        assertion.equal(s.size(), CUInt(4))
        assertion.not_equal(s.find(Nothing), s.end())
        assertion.not_equal(s.find("abc"), s.end())
        assertion.not_equal(s.find("bcd"), s.end())
        assertion.not_equal(s.find("DEF"), s.end())
        assertion.equal(s.find("???"), s.end())
    End Sub

    <test>
    Private Shared Sub set_can_find_null()
        Dim s As [set](Of String) = Nothing
        s = [set].of("a", "b", "c")
        assertion.equal(s.find(Nothing), s.end())
        assertion.not_equal(s.find("a"), s.end())
    End Sub

    Private Sub New()
    End Sub
End Class
