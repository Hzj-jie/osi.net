
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.secure

Partial Public Class token_test
    Protected NotInheritable Class info
        Inherits token_info(Of mock_ppt, mock_conn)

        Private ReadOnly f As forward_questioner_responder
        Private ReadOnly id As UInt32
        Private ReadOnly c As mock_conn
        Private ReadOnly s As signer

        Public Sub New(ByVal f As forward_questioner_responder,
                       ByVal id As UInt32,
                       ByVal c As mock_conn,
                       ByVal s As signer)
            assert(Not f Is Nothing)
            Me.f = f
            Me.id = id
            Me.c = c
            Me.s = s
        End Sub

        Public Sub New(ByVal f As forward_questioner_responder, ByVal id As UInt32, ByVal s As signer)
            Me.New(f, id, Nothing, s)
        End Sub

        Public Sub New(ByVal f As forward_questioner_responder, ByVal id As UInt32, ByVal c As mock_conn)
            Me.New(f, id, c, Nothing)
        End Sub

        Public Sub New(ByVal f As forward_questioner_responder, ByVal id As UInt32)
            Me.New(f, id, Nothing, Nothing)
        End Sub

        Public Overrides Function identity(ByVal c As mock_conn) As String
            If assertion.is_not_null(c) Then
                Return c.id
            Else
                Return Nothing
            End If
        End Function

        Public Overrides Function identity(ByVal p As mock_ppt) As String
            If assertion.is_not_null(p) Then
                Return p.id
            Else
                Return Nothing
            End If
        End Function

        Public Overrides Sub shutdown(ByVal c As mock_conn)
            If assertion.is_not_null(c) Then
                c.shutdown()
                ' f.reset(id)
                ' Data before shutdown should be sent to other end to simulate TCP, so we cannot simply reset here.
            End If
        End Sub

        Protected Overrides Function token_str(ByVal p As mock_ppt) As String
            If assertion.is_not_null(p) Then
                ' ** Note, this is not a bug. **
                ' We are using a same mock_ppt in both challenger and defender. But to simulate a failure case, we will
                ' use a 'bad' token, which stores in the mock_conn instead of mock_ppt. So always return the token from
                ' mock_conn instead of mock_ppt.
                If c Is Nothing Then
                    Return p.token
                Else
                    Return c.token
                End If
            Else
                Return Nothing
            End If
        End Function

        Public Overrides Function create_questioner_herald(ByVal p As mock_ppt,
                                                           ByVal c As mock_conn,
                                                           ByRef o As herald) As Boolean
            o = f.sender(id)
            Return True
        End Function

        Public Overrides Function create_responder_herald(ByVal p As mock_ppt,
                                                          ByVal c As mock_conn,
                                                          ByRef o As herald) As Boolean
            o = f.receiver(id)
            Return True
        End Function

        Public Overrides Function trace() As Boolean
            Return isdebugmode()
        End Function

        Public Overrides Function response_timeout_ms(ByVal p As mock_ppt) As Int64
            Return seconds_to_milliseconds(5)
        End Function

        Public Overrides Function signer(ByVal p As mock_ppt) As signer
            assertion.is_not_null(p)
            Return s
        End Function
    End Class
End Class
