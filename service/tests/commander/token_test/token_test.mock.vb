
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.commander

Partial Public Class token_test
    Protected Class mock_conn
        Public ReadOnly token As String
        Public ReadOnly id As String
        Private _closed As Boolean

        Private Sub New(ByVal token As String, ByVal match As Boolean)
            Me.token = token
            Me.id = strcat("mock_conn ", type_counter(Of mock_conn).next_id(), " - ", If(match, "good", "bad"))
        End Sub

        Public Sub shutdown()
            _closed = True
        End Sub

        Public Function closed() As Boolean
            Return _closed
        End Function

        Public Shared Function create_good(ByVal p As mock_ppt) As mock_conn
            assert(Not p Is Nothing)
            Return New mock_conn(p.token, True)
        End Function

        Private Shared Function has_token(ByVal s As String, ByVal p() As mock_ppt) As Boolean
            Dim i As UInt32 = uint32_0
            While i < array_size(p)
                If strsame(s, p(i).token) Then
                    Return True
                End If
                i += uint32_1
            End While
            Return False
        End Function

        Public Shared Function create_bad(ByVal ParamArray p() As mock_ppt) As mock_conn
            Dim s As String = Nothing
            While s Is Nothing OrElse has_token(s, p)
                s = guid_str()
            End While
            Return New mock_conn(s, False)
        End Function
    End Class

    Protected Class mock_ppt
        Public ReadOnly token As String
        Public ReadOnly id As String

        Public Sub New()
            Me.New(guid_str())
        End Sub

        Private Sub New(ByVal token As String)
            Me.token = token
            id = strcat("mock_ppt ", type_counter(Of mock_ppt).next_id())
        End Sub

        Public Shared Function create(ByVal count As UInt32,
                                      Optional ByVal with_empty_token As Boolean = False,
                                      Optional ByVal with_null_token As Boolean = False) As mock_ppt()
            assert(count > uint32_0)
            Dim tokens() As String = Nothing
            tokens = guid_strs(count)
            Dim r() As mock_ppt = Nothing
            ReDim r(count - uint32_1)
            For i As UInt32 = uint32_0 To count - uint32_1
                r(i) = New mock_ppt(tokens(i))
            Next

            If with_empty_token Then
                r(rnd_int(0, count)) = New mock_ppt(String.Empty)
            End If
            If with_null_token Then
                r(rnd_int(0, count)) = New mock_ppt(Nothing)
            End If
            Return r
        End Function
    End Class
End Class
