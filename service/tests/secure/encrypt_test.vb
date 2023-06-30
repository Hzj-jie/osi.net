
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.secure
Imports osi.service.secure.encrypt

Public Class encrypt_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New encrypt_case(), 1024)
    End Sub

    Private Class encrypt_case
        Inherits [case]

        Private Shared Function run_case(ByVal parameter As String) As Boolean
            Dim e As encryptor = Nothing
            If assertion.is_true(osi.service.secure.create(parameter, e)) AndAlso
               assertion.is_not_null(e) AndAlso
               assertion.is_true(Not e.bypass() OrElse
                           strsame(parameter, bypass_mode)) Then
                Dim key() As Byte = Nothing
                key = rnd_bytes(rnd_int(1, 256))
                If array_size(key) = uint32_1 Then
                    While key(0) = 0
                        key(0) = rnd_byte()
                    End While
                End If
                assert(Not isemptyarray(key))
                Dim ori() As Byte = Nothing
                ori = rnd_bytes(rnd_int(4096, 65536))
                assert(Not isemptyarray(ori))
                Dim encrypted() As Byte = Nothing
                Dim decrypted() As Byte = Nothing
                If assertion.is_true(e.encrypt(key, ori, encrypted)) AndAlso
                   assertion.is_true(e.decrypt(key, encrypted, decrypted)) Then
                    assertion.array_equal(ori, decrypted)
                    If e.bypass() OrElse e.bypass2() Then
                        assertion.array_equal(encrypted, ori)
                    Else
                        assertion.array_not_equal(encrypted, ori)
                    End If
                End If
            End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return run_case("bypass") AndAlso
                   run_case("bypass2") AndAlso
                   run_case("xor") AndAlso
                   run_case("ring")
        End Function
    End Class
End Class
