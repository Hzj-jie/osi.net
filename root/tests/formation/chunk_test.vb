
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class chunk_test
    <test>
    <repeat(1000, 40000)>
    Private Shared Sub random_case()
        Dim c As chunks = Nothing
        c = New chunks()
        For i As Int32 = 0 To rnd_int(20, 100) - 1
            c.emplace(next_bytes(rnd_uint(0, 256)))
        Next

        Dim d As chunks = Nothing
        ' TODO: How to compare null with empty array?
        assert_true(chunks.[New](c.export(), d))
        assert_equal(c, d)
    End Sub

    <test>
    Private Shared Sub predefined_case()
        Dim c As chunks = Nothing
        c = New chunks()
        c.insert(1)
        c.insert(1.0)
        c.insert("abc")
        c.insert(map.of(pair.of("abc", "def"), pair.of("efg", "hij")))
        c.insert(vector.of(1, 2, 3))

        Dim b() As Byte = Nothing
        b = c.export()

        Dim d As chunks = Nothing
        assert_true(chunks.[New](b, d))

        assert_equal(c, d)

        Using code_block
            Dim i As Int32 = 0
            assert_true(d.read(0, i))
            assert_equal(i, 1)
        End Using

        Using code_block
            Dim i As Double = 0
            assert_true(d.read(1, i))
            assert_equal(i, 1.0)
        End Using

        Using code_block
            Dim i As String = Nothing
            assert_true(d.read(2, i))
            assert_equal(i, "abc")
        End Using

        Using code_block
            Dim i As map(Of String, String) = Nothing
            assert_true(d.read(3, i))
            assert_equal(i, map.of(pair.of("abc", "def"), pair.of("efg", "hij")))
        End Using

        Using code_block
            Dim i As vector(Of Int32) = Nothing
            assert_true(d.read(4, i))
            assert_equal(i, vector.of(1, 2, 3))
        End Using
    End Sub

    <test>
    Private Shared Sub parse_head()
        Dim b() As Byte = Nothing
        b = rnd_bytes(rnd_uint(10, 100))
        Dim serialized() As Byte = Nothing
        Using ms As MemoryStream = New MemoryStream()
            assert_true(chunk.append_to(b, ms))
            serialized = ms.ToArray()
        End Using
        assert_true(chunk.parse_head(serialized, serialized))
        assert_equal(array_size(serialized), array_size(b))
    End Sub

    Private Sub New()
    End Sub
End Class
