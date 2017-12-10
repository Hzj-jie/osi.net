
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Public Module _request_method
    <Extension()> Public Function str(ByVal i As constants.request_method) As String
        If i < 0 OrElse i >= array_size(constants.request_method_str) Then
            Return Nothing
        Else
            Return constants.request_method_str(i)
        End If
    End Function
End Module

Public NotInheritable Class constants
    Public Const commander_content_type As String = "text/commander;"
    Public Shared ReadOnly dev_enc As Text.Encoding = default_encoding
    Public Shared ReadOnly request_method_str() As String = [Enum].GetNames(GetType(request_method))

    Public NotInheritable Class default_value
        Public Const buff_size As UInt32 = 4096
        Public Const max_connection_count As UInt32 = 1024
        Public Const connect_timeout_ms As Int64 = 30 * second_milli
        Public Const rate_sec As UInt32 = 1
        Public Const response_timeout_ms As Int64 = 30 * second_milli
        Public Const max_content_length As UInt64 = 1024 * 1024
        Public Const port As UInt16 = 80
        Public Shared ReadOnly encoder As Encoding = Encoding.UTF8

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class interval_ms
        Public Const over_max_connection_count_wait_time As Int64 = 1 * second_milli

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class protocol
        Public Const http As String = "http"
        Public Const https As String = "https"
        Public Const mailto As String = "mailto"
        Public Const ftp As String = "ftp"

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class uri
        Public Const protocol_mark As Char = character.colon
        Public Const user_mark As Char = character.at
        Public Const user_password_separator As Char = character.colon
        Public Const domain_separator As Char = character.dot
        Public Const port_mark As Char = character.colon
        Public Const path_separator As Char = root.constants.uri.path_separator
        Public Const query_mark As Char = character.question_mark
        Public Const argument_separator As Char = character.and_mark
        Public Const argument_name_value_separator As Char = character.equal_sign
        Public Const parent_path_mark As String = filesystem.parent_level_path_mark
        Public Const this_level_mark As Char = filesystem.this_level_path_mark

        Private Sub New()
        End Sub
    End Class

    Public Enum request_method
        OPTIONS
        [GET]
        HEAD
        POST
        PUT
        DELETE
        TRACE
        CONNECT
    End Enum

    Public NotInheritable Class protocol_address_head
        Private Const address_head As String = uri.protocol_mark + uri.path_separator + uri.path_separator
        Public Const http As String = protocol.http + address_head
        Public Const https As String = protocol.https + address_head
        Public Const mailto As String = protocol.mailto + uri.protocol_mark
        Public Const ftp As String = protocol.ftp + address_head

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class headers
        Public NotInheritable Class patterns
            Public NotInheritable Class range
                Public Const unit_range_set_separator As String = character.equal_sign
                Public Const range_separator As String = character.comma
                Public Shared ReadOnly range_separator_array() As String = {range_separator}
                Public Const range_value_separator As String = character.minus_sign

                Private Sub New()
                End Sub
            End Class

            Private Sub New()
            End Sub
        End Class

        Public NotInheritable Class values
            Public NotInheritable Class content_type
                Public Const charset_prefix As String = content_type_charset_prefix

                Public NotInheritable Class response
                    Private Sub New()
                    End Sub
                End Class

                Public NotInheritable Class request
                    Public Const multipart_form_data As String = "multipart/form-data"
                    Public Const www_form_urlencoded As String = "application/x-www-form-urlencoded"

                    Private Sub New()
                    End Sub
                End Class

                Private Sub New()
                End Sub
            End Class

            Public NotInheritable Class content_encoding
                Public Const gzip As String = "gzip"
                Public Const deflate As String = "deflate"

                Private Sub New()
                End Sub
            End Class

            Public NotInheritable Class transfer_encoding
                Public Const chunked As String = "chunked"

                Private Sub New()
                End Sub
            End Class

            Public NotInheritable Class range
                Public Const not_presented As Int64 = -1
                Public Const bytes_unit As String = "bytes"

                Private Sub New()
                End Sub
            End Class

            Public NotInheritable Class connection
                Public Const keep_alive As String = "keep-alive"

                Private Sub New()
                End Sub
            End Class

            Public NotInheritable Class keep_alive
                Public Const [true] As String = "true"

                Private Sub New()
                End Sub
            End Class

            Private Sub New()
            End Sub
        End Class

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
