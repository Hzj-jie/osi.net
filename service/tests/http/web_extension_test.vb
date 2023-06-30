
Imports System.IO
Imports System.Net
Imports System.Text
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.service.http

Public Class web_extension_test
    Inherits [case]

    Private Shared Sub write_stream(ByVal i As MemoryStream, ByVal s As String)
        assert(Not i Is Nothing)
        assert(Not s.null_or_empty())
        i.Seek(0, SeekOrigin.Begin)
        Dim b() As Byte = Nothing
        b = Encoding.UTF8().GetBytes(s)
        assert(array_size(b) > 0)
        i.Write(b, 0, array_size(b))
    End Sub

    Private Shared Function encoding_detection_case(
                                ByVal prepare As Action(Of WebHeaderCollection, MemoryStream, EncodingInfo),
                                ByVal gzip As Boolean) As Boolean
        assert(Not prepare Is Nothing)
        Dim h As WebHeaderCollection = Nothing
        Dim ms As MemoryStream = Nothing
        For Each enc As EncodingInfo In Encoding.GetEncodings()
            assert(strsame(enc.Name(), enc.GetEncoding().WebName(), False))
            h = New WebHeaderCollection()
            ms = New MemoryStream()
            prepare(h, ms, enc)
            Dim e As Encoding = Nothing
            Dim g As Boolean = False
            If assertion.is_true(parse_response_encoding(h, ms, e, g)) AndAlso
               assertion.is_not_null(e) Then
                assertion.is_true(strsame(e.WebName(), enc.Name(), False))
            End If
            assertion.equal(g, gzip)
        Next
        Return True
    End Function

    Private Shared Function encoding_detection_from_header_case() As Boolean
        Return encoding_detection_case(Sub(h, ms, e) h(HttpResponseHeader.ContentEncoding) = e.Name(), False)
    End Function

    Private Shared Function some_dumy_data() As String
        Dim o As StringBuilder = Nothing
        o = New StringBuilder()
        o.Append("<DATA>")
        For i As Int32 = 0 To 512 - 1
            If rnd_bool() Then
                o.Append(root.constants.character.sbc0)
            Else
                o.Append("H")
            End If
        Next
        o.Append("</DATA>")
        Return Convert.ToString(o)
    End Function

    Private Shared Function encoding_detection_from_meta_case() As Boolean
        Return encoding_detection_case(
                    Sub(h, ms, e)
                        h(HttpResponseHeader.ContentEncoding) = _
                            constants.headers.values.content_encoding.gzip
                        write_stream(
                            ms,
                            "<meta http-equiv=""Content-Type"" content=""text/html;charset=" +
                            e.Name() +
                            """ />" +
                            some_dumy_data())
                    End Sub,
                    True) AndAlso
                encoding_detection_case(
                    Sub(h, ms, e)
                        h(HttpResponseHeader.ContentEncoding) = _
                            constants.headers.values.content_encoding.gzip
                        write_stream(
                            ms,
                            "<meta http-equiv=""Content-Type"" content=""text/html;charset=" +
                            e.Name() +
                            """>" +
                            some_dumy_data())
                    End Sub,
                    True)
    End Function

    Private Shared Function encoding_detection_from_pseduo_case() As Boolean
        Return encoding_detection_case(
                    Sub(h, ms, e)
                        h(HttpResponseHeader.ContentEncoding) = _
                            constants.headers.values.content_encoding.gzip
                        write_stream(
                            ms,
                            "<?xml version=""1.0"" encoding=""" + e.Name() + """?>")
                    End Sub,
                    True)
    End Function

    Private Shared Function encoding_detection_case() As Boolean
        Return encoding_detection_from_header_case() AndAlso
               encoding_detection_from_meta_case() AndAlso
               encoding_detection_from_pseduo_case()
    End Function

    Public Overrides Function run() As Boolean
        Return encoding_detection_case()
    End Function
End Class
