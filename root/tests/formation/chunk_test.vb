
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class chunk_test
    <test>
    <repeat(1000, 40000)>
    Private Shared Sub random_case()
        Dim c As chunk = Nothing
        c = New chunk()
        For i As Int32 = 0 To rnd_int(20, 100) - 1
            c.emplace(next_bytes(rnd_uint(0, 256)))
        Next

        Dim d As chunk = Nothing
        ' TODO: How to compare null with empty array?
        assert_true(chunk.[New](c.export(), d))
        assert_equal(c, d)
    End Sub

    <test>
    Private Shared Sub predefined_case()
        Dim c As chunk = Nothing
        c = New chunk()
        c.insert(1)
        c.insert(1.0)
        c.insert("abc")
        c.insert(map.of(pair.of("abc", "def"), pair.of("efg", "hij")))
        c.insert(vector.of(1, 2, 3))

        Dim b() As Byte = Nothing
        b = c.export()

        Dim d As chunk = Nothing
        assert_true(chunk.[New](b, d))

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

    Private Sub New()
    End Sub
End Class
