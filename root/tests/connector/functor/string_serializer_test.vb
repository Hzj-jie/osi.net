
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class string_serializer_test
    <test>
    Private Shared Sub vector_case()
        Dim v As vector(Of String) = vector.of("a", "b", "c")
        Dim s As String = Nothing
        assertion.is_true(string_serializer.to_str(v, s))
        assertion.equal(s, "a,b,c")
        Dim v2 As vector(Of String) = Nothing
        assertion.is_true(string_serializer.from_str(s, v2))
        assertion.equal(v2, v)
    End Sub

    <test>
    Private Shared Sub set_case()
        Dim v As [set](Of String) = [set].of("a", "b", "c")
        Dim s As String = Nothing
        assertion.is_true(string_serializer.to_str(v, s))
        assertion.equal(s, "a,b,c")
        Dim v2 As [set](Of String) = Nothing
        assertion.is_true(string_serializer.from_str(s, v2))
        assertion.equal(v2, v)
    End Sub

    <test>
    Private Shared Sub map_case()
        Dim m As map(Of String, Int32) = map.of(pair.of("a", 1), pair.of("b", 2), pair.of("c", 3))
        Dim s As String = Nothing
        assertion.is_true(string_serializer.to_str(m, s))
        assertion.equal(s, "a:1,b:2,c:3")
        Dim m2 As map(Of String, Int32) = Nothing
        assertion.is_true(string_serializer.from_str(s, m2))
        assertion.equal(m2, m)
    End Sub

    Private Sub New()
    End Sub
End Class
