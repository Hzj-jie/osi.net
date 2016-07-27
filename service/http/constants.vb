
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Namespace constants
    Public Module _constants
        Public Const commander_content_type As String = "text/commander;"
        Public ReadOnly dev_enc As Text.Encoding

        Sub New()
            dev_enc = default_encoding
        End Sub
    End Module

    Namespace default_value
        Public Module _default_value
            Public Const buff_size As UInt32 = 4096
            Public Const max_connection_count As UInt32 = 0
            Public Const connect_timeout_ms As Int64 = 30 * second_milli
            Public Const rate_sec As UInt32 = 1
            Public Const response_timeout_ms As Int64 = 30 * second_milli
            Public Const max_content_length As UInt64 = 1024 * 1024
            Public Const port As UInt16 = 80
        End Module
    End Namespace

    Namespace interval_ms
        Public Module _interval_ms
            Public Const over_max_connection_count_wait_time As Int64 = 1 * second_milli
        End Module
    End Namespace

    Namespace protocol
        Public Module _protocol
            Public Const http As String = "http"
            Public Const https As String = "https"
            Public Const mailto As String = "mailto"
            Public Const ftp As String = "ftp"
        End Module
    End Namespace

    Namespace uri
        Public Module _uri
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
        End Module
    End Namespace

    Namespace request_method
        'http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html
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

        Public Module _request_method
            Public ReadOnly request_method_str() As String

            Sub New()
                request_method_str = [Enum].GetNames(GetType(request_method))
            End Sub

            <Extension()> Public Function str(ByVal i As request_method) As String
                If i < 0 OrElse i >= array_size(request_method_str) Then
                    Return Nothing
                Else
                    Return request_method_str(i)
                End If
            End Function
        End Module
    End Namespace

    Namespace protocol_address_head
        Public Module _protocol_address_head
            Private Const address_head As String = uri.protocol_mark + uri.path_separator + uri.path_separator
            Public Const http As String = protocol.http + address_head
            Public Const https As String = protocol.https + address_head
            Public Const mailto As String = protocol.mailto + uri.protocol_mark
            Public Const ftp As String = protocol.ftp + address_head
        End Module
    End Namespace

    Namespace headers
        Namespace patterns
            Namespace range
                Public Module _range
                    Public Const unit_range_set_separator As String = character.equal_sign
                    Public Const range_separator As String = character.comma
                    Public ReadOnly range_separator_array() As String = {range_separator}
                    Public Const range_value_separator As String = character.minus_sign
                End Module
            End Namespace
        End Namespace

        Namespace values
            Namespace content_type
                Public Module _content_type
                    Public Const charset_prefix As String = content_type_charset_prefix
                End Module

                Namespace response
                    Public Module _response
                    End Module
                End Namespace

                Namespace request
                    Public Module _request
                        Public Const multipart_form_data As String = "multipart/form-data"
                        Public Const www_form_urlencoded As String = "application/x-www-form-urlencoded"
                    End Module
                End Namespace
            End Namespace

            Namespace content_encoding
                Public Module _content_encoding
                    Public Const gzip As String = "gzip"
                    Public Const deflate As String = "deflate"
                End Module
            End Namespace

            Namespace transfer_encoding
                Public Module _transfer_encoding
                    Public Const chunked As String = "chunked"
                End Module
            End Namespace

            Namespace range
                Public Module _range
                    Public Const not_presented As Int64 = -1
                    Public Const bytes_unit As String = "bytes"
                End Module
            End Namespace

            Namespace connection
                Public Module _connection
                    Public Const keep_alive As String = "keep-alive"
                End Module
            End Namespace

            Namespace keep_alive
                Public Module _keep_alive
                    Public Const [true] As String = "true"
                End Module
            End Namespace
        End Namespace
    End Namespace
End Namespace
