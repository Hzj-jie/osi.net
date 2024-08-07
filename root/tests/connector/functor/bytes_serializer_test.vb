
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class bytes_serializer_test
    <test>
    Private Shared Sub decimal_serializer()
        Dim b() As Byte = Nothing
        b = bytes_serializer.to_bytes(New Decimal(1000))
        Dim d As Decimal = 0
        assertion.is_true(bytes_serializer.from_bytes(b, d))
        assertion.equal(d, New Decimal(1000))
    End Sub

    <test>
    Private Shared Sub cannot_parse_invalid_decimal()
        Using ms As MemoryStream = New MemoryStream()
            assertion.is_true(bytes_serializer.append_to(CULng(100), ms))
            assertion.is_true(bytes_serializer.append_to(CULng(1000), ms))
            Dim d As Decimal = 0
            assertion.is_false(bytes_serializer.read_from(ms, d))
        End Using
    End Sub

    <test>
    Private Shared Sub cannot_parse_insufficent_bytes()
        Dim b() As Byte = Nothing
        b = bytes_serializer.to_bytes(uint64_0)
        Dim d As Decimal = 0
        assertion.is_false(bytes_serializer.from_bytes(b, d))
    End Sub

    <test>
    Private Shared Sub from_empty_vector()
        Dim b() As Byte = Nothing
        b = bytes_serializer.to_bytes(New vector(Of String)())
        assertion.is_not_null(b)
        assertion.array_empty(b)
        Dim v As vector(Of String) = Nothing
        assertion.is_true(bytes_serializer.from_bytes(b, v))
        assertions.of(v).empty()
    End Sub

    Private Sub New()
    End Sub
End Class
