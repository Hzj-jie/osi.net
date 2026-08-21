
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

    <test>
    Private Shared Sub byte_serializer()
        Dim b() As Byte = Nothing
        b = bytes_serializer.to_bytes(CByte(123))
        assertion.is_not_null(b)
        assertion.equal(array_size(b), uint32_1)
        Dim o As Byte = 0
        assertion.is_true(bytes_serializer.from_bytes(b, o))
        assertion.equal(o, CByte(123))

        Using ms As MemoryStream = New MemoryStream()
            assertion.is_true(bytes_serializer.append_to(CByte(12), ms))
            assertion.is_true(bytes_serializer.append_to(CByte(34), ms))
            assertion.equal(ms.Length(), 2L)
            ms.Position() = 0
            Dim b1 As Byte = 0
            Dim b2 As Byte = 0
            assertion.is_true(bytes_serializer.consume_from(ms, b1))
            assertion.is_true(bytes_serializer.consume_from(ms, b2))
            assertion.equal(b1, CByte(12))
            assertion.equal(b2, CByte(34))
            assertion.equal(ms.Position(), 2L)
        End Using
    End Sub

    <test>
    Private Shared Sub sbyte_serializer()
        Dim b() As Byte = Nothing
        b = bytes_serializer.to_bytes(CSByte(-123))
        assertion.is_not_null(b)
        assertion.equal(array_size(b), uint32_1)
        Dim o As SByte = 0
        assertion.is_true(bytes_serializer.from_bytes(b, o))
        assertion.equal(o, CSByte(-123))

        Using ms As MemoryStream = New MemoryStream()
            assertion.is_true(bytes_serializer.append_to(CSByte(-12), ms))
            assertion.is_true(bytes_serializer.append_to(CSByte(34), ms))
            assertion.equal(ms.Length(), 2L)
            ms.Position() = 0
            Dim s1 As SByte = 0
            Dim s2 As SByte = 0
            assertion.is_true(bytes_serializer.consume_from(ms, s1))
            assertion.is_true(bytes_serializer.consume_from(ms, s2))
            assertion.equal(s1, CSByte(-12))
            assertion.equal(s2, CSByte(34))
            assertion.equal(ms.Position(), 2L)
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
