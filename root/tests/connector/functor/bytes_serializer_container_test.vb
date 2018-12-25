
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class bytes_serializer_container_test
    <test>
    Private Shared Sub from_vector()
        Dim v As vector(Of String) = Nothing
        v = vector.emplace_of("abc", "bcd", "def")
        Dim r As vector(Of Byte()) = Nothing
        assert_true(bytes_serializer.from_container(Of String)().of(v).to(r))
        assert_equal(v.size(), r.size())
        Dim v2 As vector(Of String) = Nothing
        For i As Int32 = 0 To 1
            assert_true(bytes_serializer.to_container(Of String)().from(r).to(v2))
            assert_equal(v.size(), v2.size())
            assert_equal(v, v2)
        Next
    End Sub

    <test>
    Private Shared Sub from_vector_vector()
        Dim v As vector(Of vector(Of String)) = Nothing
        v = vector.emplace_of(vector.emplace_of("abc", "bcd"), vector.emplace_of("cde", "def", "efg"))
        Dim r As vector(Of Byte()) = Nothing
        assert_true(bytes_serializer.from_container(Of vector(Of String))().of(v).to(r))
        assert_equal(v.size(), r.size())
        Dim v2 As vector(Of vector(Of String)) = Nothing
        For i As Int32 = 0 To 1
            assert_true(bytes_serializer.to_container(Of vector(Of String))().from(r).to(v2))
            assert_equal(v.size(), v2.size())
            assert_equal(v, v2)
        Next
    End Sub

    <test>
    Private Shared Sub from_map()
        Dim m As map(Of String, Int32) = Nothing
        m = map.emplace_of(pair.emplace_of("abc", 1), pair.emplace_of("bcd", 2), pair.emplace_of("cde", 3))
        Dim r As vector(Of Byte()) = Nothing
        assert_true(bytes_serializer.from_container(Of first_const_pair(Of String, Int32))().of(m).to(r))
        assert_equal(m.size(), r.size())
        Dim m2 As map(Of String, Int32) = Nothing
        For i As Int32 = 0 To 1
            assert_true(bytes_serializer.to_container(Of first_const_pair(Of String, Int32))().from(r).to(m2))
            assert_equal(m.size(), m2.size())
            assert_equal(m, m2)
        Next
    End Sub

    <test>
    Private Shared Sub from_map_map()
        Dim m As map(Of String, map(Of String, Int32)) = Nothing
        m = map.emplace_of(pair.emplace_of("a", map.emplace_of(pair.emplace_of("ab", 1), pair.emplace_of("bc", 2))),
                           pair.emplace_of("b", map.emplace_of(pair.emplace_of("cd", 3), pair.emplace_of("de", 4))),
                           pair.emplace_of("c", map.emplace_of(pair.emplace_of("ef", 5), pair.emplace_of("fg", 5))))
        Dim r As vector(Of Byte()) = Nothing
        assert_true(bytes_serializer.
                        from_container(Of first_const_pair(Of String, map(Of String, Int32)))().of(m).to(r))
        assert_equal(m.size(), r.size())
        Dim m2 As map(Of String, map(Of String, Int32)) = Nothing
        For i As Int32 = 0 To 1
            assert_true(bytes_serializer.
                        to_container(Of first_const_pair(Of String, map(Of String, Int32)))().from(r).to(m2))
            assert_equal(m.size(), m2.size())
            assert_equal(m, m2)
        Next
    End Sub

    <test>
    Private Shared Sub from_map_vector()
        Dim m As map(Of String, vector(Of Int32)) = Nothing
        m = map.emplace_of(
                pair.emplace_of("abc", vector.of(1, 2, 3)),
                pair.emplace_of("bcd", vector.of(2, 3, 4, 5)),
                pair.emplace_of("cde", vector.of(3, 3)))
        Dim r As vector(Of Byte()) = Nothing
        assert_true(bytes_serializer.from_container(Of first_const_pair(Of String, vector(Of Int32)))().of(m).to(r))
        assert_equal(m.size(), r.size())
        Dim m2 As map(Of String, vector(Of Int32)) = Nothing
        For i As Int32 = 0 To 1
            assert_true(bytes_serializer.
                            to_container(Of first_const_pair(Of String, vector(Of Int32)))().from(r).to(m2))
            assert_equal(m.size(), m2.size())
            assert_equal(m, m2)
        Next
    End Sub

    Private Sub New()
    End Sub
End Class
