
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.secure

Public Class token_info_sign_test
    Inherits [case]

    Private Class mock_ppt
    End Class

    Private Class mock_conn
    End Class

    Private Class info
        Inherits token_info(Of mock_ppt, mock_conn)

        Private ReadOnly k() As Byte
        Private ReadOnly s As signer

        Public Sub New(ByVal s As signer)
            assert(Not s Is Nothing)
            Me.s = s
            Me.k = rnd_bytes(rnd_uint(10, 100))
        End Sub

        Public Overrides Function identity(ByVal c As mock_conn) As String
            assert(False)
            Return Nothing
        End Function

        Public Overrides Function identity(ByVal p As mock_ppt) As String
            assert(False)
            Return Nothing
        End Function

        Public Overrides Function response_timeout_ms(ByVal p As mock_ppt) As Int64
            assert(False)
            Return int64_0
        End Function

        Public Overrides Sub shutdown(ByVal c As mock_conn)
            assert(False)
        End Sub

        Public Overrides Function signer(ByVal p As mock_ppt) As signer
            Return s
        End Function

        Public Overrides Function token(ByVal p As mock_ppt) As Byte()
            Return k
        End Function
    End Class

    Private Shared Function sign_case1(ByVal f As info) As Boolean
        assert(Not f Is Nothing)
        Dim code() As Byte = Nothing
        code = rnd_bytes(rnd_uint(100, 200))
        Dim o As piece = Nothing
        If assertion.is_true(f.sign(Nothing, code, o)) Then
            assertion.is_true(f.sign_match(Nothing, code, o))
        End If
        Dim t As piece = Nothing
        assertion.is_true(f.forge_signature(Nothing, code, t))
        assertion.not_equal(o, t)
        Return True
    End Function

    Private Shared Function sign_case2(ByVal f As info) As Boolean
        Dim prefix() As Byte = Nothing
        Dim prefix_len As UInt32 = uint32_0
        prefix_len = rnd_uint(10, 20)
        prefix = rnd_bytes(prefix_len)
        assert(array_size(prefix) = prefix_len)
        Dim code() As Byte = Nothing
        code = rnd_bytes(rnd_uint(100, 200))
        Dim o As piece = Nothing
        If assertion.is_true(f.sign(Nothing, New piece(array_concat(prefix, code)).consume(prefix_len), o)) Then
            assertion.is_true(f.sign_match(Nothing, code, o))
        End If
        Return True
    End Function

    Private Shared Function sign_case(ByVal s As signer) As Boolean
        If s Is Nothing Then
            Return False
        End If
        For i As UInt32 = 0 To 1000
            If Not sign_case1(New info(s)) OrElse Not sign_case2(New info(s)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim ss() As signer = Nothing
        ss = sign.all_signers()
        For i As Int32 = 0 To array_size_i(ss) - 1
            If Not sign_case(ss(i)) Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
