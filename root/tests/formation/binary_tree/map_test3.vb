
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class map_test3
    <test>
    Private Shared Sub map_can_store_null()
        Dim m As map(Of String, String) = Nothing
        m = map.of(pair.of(default_str, "ABC"),
                   pair.of("bcd", default_str),
                   pair.of("abc", "def"),
                   pair.of(default_str, default_str))
        assertion.equal(m.size(), CUInt(3))
        assertion.not_equal(m.find(default_str), m.end())
        assertion.equal((+m.find(default_str)).first, default_str)
        assertion.equal((+m.find(default_str)).second, "ABC")
        assertion.equal((+m.find("bcd")).second, default_str)
        assertion.equal((+m.find("abc")).second, "def")
    End Sub

    <test>
    Private Shared Sub map_can_find_null()
        Dim m As map(Of String, String) = Nothing
        m = map.of(pair.of("bcd", "def"), pair.of("abc", "def"))
        assertion.equal(m.size(), CUInt(2))
        assertion.equal(m.find(default_str), m.end())
    End Sub

    Private Sub New()
    End Sub
End Class
