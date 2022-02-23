
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Text
Imports System.Web
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class url_query
    Private Shared ReadOnly argument_separators() As Char = {constants.uri.argument_separator}
    Private Shared ReadOnly argument_name_value_separator As String = constants.uri.argument_name_value_separator

    ' Parse an input string without question mark, e.g. a=b&c=d.
    ' If result is nothing, this function creates a new object.
    Public Shared Function parse(ByVal input As String,
                                 ByRef result As map(Of String, vector(Of String)),
                                 Optional ByVal e As Encoding = Nothing) As Boolean
        If String.IsNullOrEmpty(input) Then
            Return False
        Else
            If result Is Nothing Then
                result = New map(Of String, vector(Of String))()
            End If
            If e Is Nothing Then
                e = default_encoding
            End If
            Dim ss() As String = Nothing
            ss = input.Split(argument_separators, StringSplitOptions.RemoveEmptyEntries)
            If isemptyarray(ss) Then
                'odd, should not happen
                Return False
            Else
                For i As Int32 = 0 To array_size_i(ss) - 1
                    If Not String.IsNullOrEmpty(ss(i)) Then
                        Dim k As String = Nothing
                        Dim v As String = Nothing
                        If strsep(ss(i), k, v, argument_name_value_separator, False) Then
                            v = HttpUtility.UrlDecode(v, e)
                        Else
                            k = ss(i)
                            v = Nothing
                        End If
                        assert(Not String.IsNullOrEmpty(k))
                        k = HttpUtility.UrlDecode(k, e)
                        result(k).emplace_back(v)
                    End If
                Next
                Return True
            End If
        End If
    End Function

    Public Shared Function parse(ByVal request As HttpListenerRequest,
                                 ByRef result As map(Of String, vector(Of String)),
                                 Optional ByVal e As Encoding = Nothing) As Boolean
        If request Is Nothing Then
            Return False
        ElseIf String.IsNullOrEmpty(request.Url().Query()) Then
            Return True
        Else
            Return parse(strmid(request.Url().Query(), strlen(constants.uri.query_mark)), result, e)
        End If
    End Function

    Public Shared Function parse_www_form_urlencoded_request_body(
                               ByVal request As HttpListenerRequest,
                               ByVal request_body As piece,
                               ByRef result As map(Of String, vector(Of String)),
                               Optional ByRef charset As String = Nothing,
                               Optional ByRef encoder As Encoding = Nothing) As Boolean
        If request Is Nothing OrElse request_body.null_or_empty() Then
            Return False
        End If

        If Not request.is_www_form_urlencoded(charset, encoder) Then
            Return False
        End If

        assert(encoder IsNot Nothing)
        Dim s As String = Nothing
        Try
            s = encoder.GetString(+request_body)
        Catch ex As Exception
            raise_error(error_type.user, "Cannot decode input byte array with ", encoder.WebName(), ", ", ex.details())
            Return False
        End Try

        Return parse(s, result, encoder)
    End Function
End Class
