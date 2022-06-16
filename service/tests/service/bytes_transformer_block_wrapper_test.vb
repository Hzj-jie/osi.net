
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.device
Imports osi.service.secure
Imports osi.service.transmitter
Imports osi.service.zip

Public Class bytes_transformer_block_wrapper_test
    Inherits case_wrapper

    Private Shared Function create_cases() As vector(Of [case])
        Dim zippers() As String = {"gzip", "deflate", "bypass"}
        Dim encryptors() As String = {"xor", "ring", "bypass"}
        Dim r As vector(Of [case]) = Nothing
        r = New vector(Of [case])()
        For i As Int32 = 0 To array_size_i(zippers) - 1
            For j As Int32 = 0 To array_size_i(encryptors) - 1
                r.emplace_back(New bytes_transformer_block_wrapper_case(guid_str(), zippers(i), encryptors(j)))
                r.emplace_back(New bytes_transformer_block_wrapper_case(Nothing, zippers(i), encryptors(j)))
            Next
        Next
        Return r
    End Function

    Public Sub New()
        MyBase.New(chained(+create_cases()))
    End Sub

    Private Class bytes_transformer_block_wrapper_case
        Inherits [case]

        Private ReadOnly token As String
        Private ReadOnly zipper As String
        Private ReadOnly encryptor As String
        Private ReadOnly v As var
        Private ReadOnly block_dev As block

        Public Sub New(ByVal token As String,
                       ByVal zipper As String,
                       ByVal encryptor As String)
            Me.token = token
            Me.zipper = zipper
            Me.encryptor = encryptor
            Me.v = New var()
            Me.block_dev = New block_impl()
        End Sub

        Private Class block_impl
            Implements block

            Public Function send(ByVal buff() As Byte,
                                 ByVal offset As UInt32,
                                 ByVal count As UInt32) As event_comb Implements block.send
                assert(False)
                Return Nothing
            End Function

            Public Function receive(ByVal result As ref(Of Byte())) As event_comb Implements block.receive
                assert(False)
                Return Nothing
            End Function

            Public Function sense(ByVal pending As ref(Of Boolean),
                                  ByVal timeout_ms As Int64) As event_comb Implements block.sense
                assert(False)
                Return Nothing
            End Function
        End Class

        Private Function run_case(ByVal p As bytes_transformer_block_wrapper) As Boolean
            For i As Int32 = 0 To 128 - 1
                Dim b() As Byte = Nothing
                b = next_bytes(rnd_uint(4096, 16384))
                Dim o() As Byte = Nothing
                Dim offset As Int32 = 0
                Dim count As Int32 = 0
                offset = rnd_int(0, 5)
                count = rnd_int(1, array_size_i(b) - offset + 1)
                assert(offset >= 0)
                assert(count > 0)
                assert(count + offset <= array_size(b))
                assertion.is_true(p.send_transformer().transform_forward(b, CUInt(offset), CUInt(count), o))
                Dim o2() As Byte = Nothing
                assertion.is_true(p.receive_transformer().transform_backward(o, o2))
                assertion.equal(memcmp(o2, 0, b, CUInt(offset), CUInt(count)),
                                0,
                                "token = ",
                                token,
                                ", zipper = ",
                                zipper,
                                ", encryptor = ",
                                encryptor)

                assertion.is_false(p.send_transformer().transform_forward(b, 1, array_size(b), o))
            Next
            Return True
        End Function

        Public Overrides Function run() As Boolean
            If token.null_or_empty() Then
                assertion.is_false(create_encrypt_bytes_transformer_block_wrapper(encryptor, token, v, block_dev, Nothing))
            Else
                Dim p As bytes_transformer_block_wrapper = Nothing
                If assertion.is_true(create_encrypt_bytes_transformer_block_wrapper(encryptor,
                                                                              token,
                                                                              v,
                                                                              block_dev,
                                                                              p)) AndAlso
                   assertion.is_true(create_zip_bytes_transformer_block_wrapper(zipper, v, p, p)) Then
                    If Not run_case(p) Then
                        Return False
                    End If
                End If
                If assertion.is_true(create_zip_bytes_transformer_block_wrapper(zipper, v, block_dev, p)) AndAlso
                   assertion.is_true(create_encrypt_bytes_transformer_block_wrapper(encryptor, token, v, p, p)) Then
                    If Not run_case(p) Then
                        Return False
                    End If
                End If
            End If
            Return True
        End Function
    End Class
End Class
