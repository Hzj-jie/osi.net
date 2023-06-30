
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_bytes_serializer_test
    <test>
    Private Shared Sub int_case()
        Dim s As bytes_serializer(Of Object) = Nothing
        assertion.is_true(type_bytes_serializer.serializer(GetType(Int32), s))
        assertion.is_not_null(s)
        Dim b() As Byte = Nothing
        assertion.array_size(s.to_bytes(100), sizeof_int32)
        assertion.equal(s.from_bytes(s.to_bytes(1000)), 1000)
    End Sub

    <test>
    Private Shared Sub string_case()
        Dim s As bytes_serializer(Of Object) = Nothing
        assertion.is_true(type_bytes_serializer.serializer(GetType(String), s))
        assertion.is_not_null(s)
        Dim b() As Byte = Nothing
        assertion.array_size(s.to_bytes("hello"), 5)
        assertion.equal(s.from_bytes(s.to_bytes("hawaii")), "hawaii")
    End Sub

    Private Sub New()
    End Sub
End Class
