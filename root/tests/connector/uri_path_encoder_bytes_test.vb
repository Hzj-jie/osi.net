
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class uri_path_encoder_bytes_test
    Private Shared Sub run_case(ByVal encode As Func(Of Byte(), StringWriter, Boolean),
                                ByVal decode As Func(Of String, MemoryStream, Boolean))
        assert(encode IsNot Nothing)
        assert(decode IsNot Nothing)
        Dim b() As Byte = Nothing
        b = next_bytes(rnd_uint(1024, 4096))
        Using s As StringWriter = New StringWriter()
            assertion.is_true(encode(b, s))
            Using ms As MemoryStream = New MemoryStream()
                assertion.is_true(decode(Convert.ToString(s), ms))
                assertion.array_equal(ms.fit_buffer(), b)
            End Using
        End Using
    End Sub

    <test>
    <repeat(10000)>
    Private Shared Sub shorten_case()
        run_case(AddressOf uri.path_encoder.bytes.shorten.encode, AddressOf uri.path_encoder.bytes.shorten.decode)
    End Sub

    <test>
    <repeat(10000)>
    Private Shared Sub base64_case()
        run_case(AddressOf uri.path_encoder.bytes.base64.encode, AddressOf uri.path_encoder.bytes.base64.decode)
    End Sub

    Private Sub New()
    End Sub
End Class
