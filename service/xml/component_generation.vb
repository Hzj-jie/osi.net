
Imports System.Text
Imports System.Web
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.xml.constants

Public Module _component_generation
    Public Function create_start_tag(ByVal tag As String,
                                     ByVal self_close As Boolean,
                                     ByRef output As String,
                                     ByVal ParamArray attrs() As pair(Of String, String)) As Boolean
        If String.IsNullOrEmpty(tag) Then
            Return False
        Else
            Dim r As StringBuilder = Nothing
            r = New StringBuilder()
            r.Append(tag_leading)
            r.Append(HttpUtility.HtmlEncode(tag))
            If Not isemptyarray(attrs) Then
                For i As Int32 = 0 To array_size(attrs) - 1
                    If String.IsNullOrEmpty(attrs(i).first) Then
                        Return False
                    Else
                        r.Append(attributes_separator)
                        r.Append(HttpUtility.HtmlEncode(attrs(i).first))
                        r.Append(attribute_separator)
                        r.Append(value_leading)
                        r.Append(HttpUtility.HtmlEncode(attrs(i).second))
                        r.Append(value_final)
                    End If
                Next
            End If
            If self_close Then
                r.Append(tag_close_mark)
            End If
            r.Append(tag_final)
            output = Convert.ToString(r)
            Return True
        End If
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal self_close As Boolean,
                                     ByVal ParamArray attrs() As pair(Of String, String)) As String
        Dim o As String = Nothing
        assert(create_start_tag(tag, self_close, o, attrs))
        Return o
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal ParamArray attrs() As pair(Of String, String)) As String
        Return create_start_tag(tag, False, attrs)
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal attrs As vector(Of pair(Of String, String)),
                                     ByVal self_close As Boolean,
                                     ByRef output As String) As Boolean
        Return create_start_tag(tag, self_close, output, +attrs)
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal self_close As Boolean,
                                     Optional ByVal attrs As vector(Of pair(Of String, String)) = Nothing) As String
        Dim o As String = Nothing
        assert(create_start_tag(tag, attrs, self_close, o))
        Return o
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     Optional ByVal attrs As vector(Of pair(Of String, String)) = Nothing) As String
        Return create_start_tag(tag, False, attrs)
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal attributes As map(Of String, String),
                                     ByVal self_close As Boolean,
                                     ByRef o As String) As Boolean
        Dim v As vector(Of pair(Of String, String)) = Nothing
        If Not attributes Is Nothing AndAlso Not attributes.empty() Then
            v = New vector(Of pair(Of String, String))()
            Dim it As map(Of String, String).iterator = Nothing
            it = attributes.begin()
            While it <> attributes.end()
                v.emplace_back(make_pair((+it).first, (+it).second))
                it += 1
            End While
        End If
        Return create_start_tag(tag, v, self_close, o)
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal self_close As Boolean,
                                     ByVal attributes As map(Of String, String)) As String
        Dim o As String = Nothing
        assert(create_start_tag(tag, attributes, self_close, o))
        Return o
    End Function

    Public Function create_start_tag(ByVal tag As String,
                                     ByVal attributes As map(Of String, String)) As String
        Return create_start_tag(tag, False, attributes)
    End Function

    Public Function create_end_tag(ByVal tag As String, ByRef o As String) As Boolean
        If String.IsNullOrEmpty(tag) Then
            Return False
        Else
            o = strcat(tag_leading, tag_close_mark, HttpUtility.HtmlEncode(tag), tag_final)
            Return True
        End If
    End Function

    Public Function create_end_tag(ByVal tag As String) As String
        Dim o As String = Nothing
        assert(create_end_tag(tag, o))
        Return o
    End Function

    Public Function create_text(ByVal i As String, ByRef o As String) As Boolean
        If String.IsNullOrEmpty(i) Then
            Return False
        Else
            o = HttpUtility.HtmlEncode(i)
            Return True
        End If
    End Function

    Public Function create_text(ByVal i As String) As String
        Dim o As String = Nothing
        assert(create_text(i, o))
        Return o
    End Function

    Public Function create_comment(ByVal i As String, ByRef o As String) As Boolean
        If String.IsNullOrEmpty(i) Then
            Return False
        ElseIf strindexof(i, invalid_comment_text) <> npos Then
            Return False
        Else
            o = strcat(comment_leading, i, comment_final)
            Return True
        End If
    End Function

    Public Function create_comment(ByVal i As String) As String
        Dim o As String = Nothing
        assert(create_comment(i, o))
        Return o
    End Function

    Public Function create_loosen_comment(ByVal i As String, ByRef o As String) As Boolean
        Return create_comment(strrplc(i, invalid_comment_text, invalid_comment_text_replacement), o)
    End Function

    Public Function create_loosen_comment(ByVal i As String) As String
        Dim o As String = Nothing
        assert(create_loosen_comment(i, o))
        Return o
    End Function

    Public Function create_cdata(ByVal i As String, ByRef o As String) As Boolean
        If String.IsNullOrEmpty(i) Then
            Return False
        ElseIf strindexof(i, invalid_cdata_text) <> npos Then
            Return False
        Else
            o = strcat(cdata_leading, i, cdata_final)
            Return True
        End If
    End Function

    Public Function create_cdata(ByVal i As String) As String
        Dim o As String = Nothing
        assert(create_cdata(i, o))
        Return o
    End Function

    Public Function create_loosen_cdata(ByVal i As String, ByRef o As String) As Boolean
        Return create_cdata(strrplc(i, invalid_cdata_text, invalid_cdata_text_replacement), o)
    End Function

    Public Function create_loosen_cdata(ByVal i As String) As String
        Dim o As String = Nothing
        assert(create_loosen_cdata(i, o))
        Return o
    End Function
End Module
