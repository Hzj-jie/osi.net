
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.argument
Imports osi.service.device
Imports osi.service.transmitter
Imports osi.service.secure.encrypt

Public Enum encrypt_mode
    bypass
    bypass2
    ring
    [xor]
End Enum

<global_init(global_init_level.services)>
Public Module _create
    Public ReadOnly bypass_mode As String = encrypt_mode.bypass.ToString()

    Public Function create(ByVal mode_str As String,
                           ByVal parameters As var,
                           ByRef o As encryptor) As Boolean
        Dim mode As encrypt_mode = Nothing
        If enum_def.from(mode_str, mode) Then
            Select Case mode
                Case encrypt_mode.bypass
                    Return bypass.create(parameters, o)
                Case encrypt_mode.bypass2
                    Return bypass2.create(parameters, o)
                Case encrypt_mode.ring
                    Return ring.create(parameters, o)
                Case encrypt_mode.xor
                    Return [xor].create(parameters, o)
                Case Else
                    Return False
            End Select
        Else
            Return False
        End If
    End Function

    Public Function create(ByVal parameter As String, ByRef o As encryptor) As Boolean
        Return create(New var(parameter), o)
    End Function

    Public Function create(ByVal parameters As var, ByRef o As encryptor) As Boolean
        Return parameters IsNot Nothing AndAlso
               Not parameters.other_values().empty() AndAlso
               create(parameters.other_values()(0), parameters, o)
    End Function

    Public Function create(ByVal mode_str As String, ByVal parameters As var) As encryptor
        Dim o As encryptor = Nothing
        assert(create(mode_str, parameters, o))
        Return o
    End Function

    Public Function create(ByVal parameter As String) As encryptor
        Dim o As encryptor = Nothing
        assert(create(parameter, o))
        Return o
    End Function

    Public Function create(ByVal parameters As var) As encryptor
        Dim o As encryptor = Nothing
        assert(create(parameters, o))
        Return o
    End Function

    Public Function create_bytes_transformer_block_wrapper(ByVal mode As String,
                                                           ByVal token As String,
                                                           ByVal v As var,
                                                           ByVal i As block,
                                                           ByVal encrypt As Boolean,
                                                           ByRef o As bytes_transformer_block_wrapper) As Boolean
        Dim e As encryptor = Nothing
        If create(mode, v, e) Then
            If String.IsNullOrEmpty(token) Then
                Return False
            Else
                Dim key() As Byte = Nothing
                key.from(token)
                assert(Not isemptyarray(key))
                Return bytes_transformer_block_wrapper.create(i,
                                                              If(encrypt,
                                                                 e.as_encrypt_bytes_transformer(key),
                                                                 e.as_decrypt_bytes_transformer(key)),
                                                              If(encrypt,
                                                                 e.as_decrypt_bytes_transformer(key),
                                                                 e.as_encrypt_bytes_transformer(key)),
                                                              o)
            End If
        Else
            Return False
        End If
    End Function

    Public Function create_bytes_transformer_block_wrapper(ByVal mode As String,
                                                           ByVal v As var,
                                                           ByVal i As block,
                                                           ByVal encrypt As Boolean,
                                                           ByRef o As bytes_transformer_block_wrapper) As Boolean
        Dim s As String = Nothing
        If (v.value("key", s) OrElse v.value("token", s)) Then
            Return create_bytes_transformer_block_wrapper(mode, s, v, i, encrypt, o)
        Else
            Return False
        End If
    End Function

    Public Function create_encrypt_bytes_transformer_block_wrapper(
                        ByVal mode As String,
                        ByVal token As String,
                        ByVal v As var,
                        ByVal i As block,
                        ByRef o As bytes_transformer_block_wrapper) As Boolean
        Return create_bytes_transformer_block_wrapper(mode, token, v, i, True, o)
    End Function

    Public Function create_encrypt_bytes_transformer_block_wrapper(
                        ByVal mode As String,
                        ByVal v As var,
                        ByVal i As block,
                        ByRef o As bytes_transformer_block_wrapper) As Boolean
        Return create_bytes_transformer_block_wrapper(mode, v, i, True, o)
    End Function

    Public Function create_decrypt_bytes_transformer_block_wrapper(
                        ByVal mode As String,
                        ByVal token As String,
                        ByVal v As var,
                        ByVal i As block,
                        ByRef o As bytes_transformer_block_wrapper) As Boolean
        Return create_bytes_transformer_block_wrapper(mode, token, v, i, False, o)
    End Function

    Public Function create_decrypt_bytes_transformer_block_wrapper(
                        ByVal mode As String,
                        ByVal v As var,
                        ByVal i As block,
                        ByRef o As bytes_transformer_block_wrapper) As Boolean
        Return create_bytes_transformer_block_wrapper(mode, v, i, False, o)
    End Function

    Private Sub init()
        assert(wrapper.register(wrapper.parameter(
                   "encryptor",
                   Function(mode As String,
                            v As var,
                            i As block,
                            ByRef o As block) As Boolean
                       Dim r As bytes_transformer_block_wrapper = Nothing
                       If Not create_encrypt_bytes_transformer_block_wrapper(mode, v, i, r) Then
                           Return False
                       End If
                       assert(r IsNot Nothing)
                       o = r
                       Return True
                   End Function)))
        assert(wrapper.register(wrapper.parameter(
                   "decryptor",
                   Function(mode As String,
                            v As var,
                            i As block,
                            ByRef o As block) As Boolean
                       Dim r As bytes_transformer_block_wrapper = Nothing
                       If Not create_decrypt_bytes_transformer_block_wrapper(mode, v, i, r) Then
                           Return False
                       End If
                       assert(r IsNot Nothing)
                       o = r
                       Return True
                   End Function)))
    End Sub
End Module
