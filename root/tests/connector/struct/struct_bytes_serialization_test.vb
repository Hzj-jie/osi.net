
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

        Shared Sub New()
            struct(Of c).register()
        End Sub

        Public Sub New()
            a = guid_str()
            b = vector.emplace_of(rnd_int(), rnd_int(), rnd_int())
            c = rnd_bool()
        End Sub
    End Class

    <test>
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
        assertion.not_reference_equal(a.a, c.a)
        assertion.not_reference_equal(a.b, c.b)
    End Sub

    Private Sub New()
    End Sub
End Class
