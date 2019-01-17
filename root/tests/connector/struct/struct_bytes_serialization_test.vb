
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class struct_bytes_serialization_test
    Private NotInheritable Class c
        Public ReadOnly a As String
        Public ReadOnly b As vector(Of Int32)
        Public ReadOnly c As Boolean
        Public ReadOnly d() As Byte
        Public ReadOnly e As UInt64

        Shared Sub New()
            struct(Of c).register()
        End Sub

        Public Sub New()
            a = rnd_utf8_chars(rnd_int(16, 32))
            b = vector.emplace_of(rnd_ints(rnd_int(16, 32)))
            c = rnd_bool()
            d = rnd_bytes(rnd_uint(16, 32))
            e = rnd_uint64()
        End Sub
    End Class

    <test>
    <repeat(100000)>
    Private Shared Sub run()
        Dim a As c = Nothing
        a = New c()
        Dim b() As Byte = Nothing
        b = bytes_serializer.to_bytes(a)
        assertion.is_not_null(b)
        assertion.array_not_empty(b)
        Dim c As c = Nothing
        assertion.is_true(bytes_serializer.from_bytes(b, c))
        assertion.equal(a.a, c.a)
        assertion.equal(a.b, c.b)
        assertion.equal(a.c, c.c)
        assertion.array_equal(a.d, c.d)
        assertion.equal(a.e, c.e)
        assertion.not_reference_equal(a.a, c.a)
        assertion.not_reference_equal(a.b, c.b)
        assertion.not_reference_equal(a.d, c.d)
    End Sub

    Private Sub New()
    End Sub
End Class
