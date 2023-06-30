
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.secure
Imports osi.service.secure.sign

Public MustInherit Class token_info(Of COLLECTION, CONNECTION)
    Public MustOverride Function identity(ByVal p As COLLECTION) As String
    Public MustOverride Function identity(ByVal c As CONNECTION) As String

    Public Overridable Function token(ByVal p As COLLECTION) As Byte()
        Return str_bytes(token_str(p))
    End Function

    Protected Overridable Function token_str(ByVal p As COLLECTION) As String
        assert(False)
        Return Nothing
    End Function

    Public Function token_as_str(ByVal p As COLLECTION) As String
        Return token_to_str(token(p))
    End Function

    Public Overridable Function token_to_str(ByVal b() As Byte) As String
        Return bytes_str(b)
    End Function

    Public Overridable Function create_questioner_herald(ByVal p As COLLECTION,
                                                         ByVal c As CONNECTION,
                                                         ByRef o As herald) As Boolean
        Return create_herald(p, c, o)
    End Function

    Public Overridable Function create_responder_herald(ByVal p As COLLECTION,
                                                        ByVal c As CONNECTION,
                                                        ByRef o As herald) As Boolean
        Return create_herald(p, c, o)
    End Function

    Protected Overridable Function create_herald(ByVal p As COLLECTION,
                                                 ByVal c As CONNECTION,
                                                 ByRef o As herald) As Boolean
        o = create_herald(p, c)
        Return True
    End Function

    Protected Overridable Function create_herald(ByVal p As COLLECTION, ByVal c As CONNECTION) As herald
        assert(False)
        Return Nothing
    End Function

    Public MustOverride Function response_timeout_ms(ByVal p As COLLECTION) As Int64

    Public MustOverride Sub shutdown(ByVal c As CONNECTION)

    Public Overridable Function trace(ByVal p As COLLECTION) As Boolean
        Return trace()
    End Function

    Public Overridable Function trace() As Boolean
        Return False
    End Function

    Public Overridable Function challenge_code_length(ByVal p As COLLECTION) As UInt32
        Return 32
    End Function

    Public Function challenge_code(ByVal p As COLLECTION) As Byte()
        Return rnd_bytes(challenge_code_length(p))
    End Function

    Public Overridable Function signer(ByVal p As COLLECTION) As signer
        Return md5_merge_hasher.concat
    End Function

    Public Function sign(ByVal p As COLLECTION,
                         ByVal code As piece,
                         ByRef o As piece) As Boolean
        Dim s As signer = Nothing
        s = signer(p)
        assert(Not s Is Nothing)
        Dim b() As Byte = Nothing
        If code.size() > uint32_0 AndAlso
           s.sign(token(p), code, b) AndAlso
           assert(Not isemptyarray(b)) Then
            o = New piece(b)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function sign_match(ByVal p As COLLECTION,
                               ByVal code As piece,
                               ByVal response As piece) As Boolean
        Dim o As piece = Nothing
        Return Not response Is Nothing AndAlso
               sign(p, code, o) AndAlso
               response.compare(o) = 0
    End Function

    Public Function forge_signature(ByVal p As COLLECTION,
                                    ByVal code As piece,
                                    ByRef o As piece) As Boolean
        Do
            o = New piece(challenge_code(p))
            assert(Not o.empty())
        Loop While sign_match(p, code, o)
        Return True
    End Function
End Class
