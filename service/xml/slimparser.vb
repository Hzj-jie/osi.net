
Imports System.Web
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.xml.constants

Public Module _slimparser
    Public Enum node_type
        tag
        text
        comment
        declaration
        cdata
    End Enum

    Sub New()
        assert(tag_leading_len = 1)
        assert(tag_final_len = 1)
        'make sure all the types are starting with tag_leading
        assert(declaration_leading.StartsWith(tag_leading_str))
        assert(declaration_leading_2.StartsWith(tag_leading_str))
        assert(comment_leading.StartsWith(tag_leading_str))
        assert(cdata_leading.StartsWith(tag_leading_str))
        'make sure all the types are finishing with tag_final
        assert(declaration_final(declaration_final_len - 1) = tag_final)
        assert(declaration_final_2_str(declaration_final_2_len - 1) = tag_final)
        assert(comment_final(comment_final_len - 1) = tag_final)
        assert(cdata_final(cdata_final_len - 1) = tag_final)
    End Sub

    Public Sub unescapse(ByVal v As vector(Of pair(Of String, node_type)))
        If Not v Is Nothing AndAlso Not v.empty() Then
            For i As Int32 = 0 To v.size() - 1
                If v(i).second = node_type.cdata Then
                    v(i).second = node_type.text
                    v(i).first = strmid(v(i).first,
                                        cdata_leading_len,
                                        strlen(v(i).first) - cdata_leading_len - cdata_final_len)
                ElseIf v(i).second = node_type.text Then
                    v(i).first = HttpUtility.HtmlDecode(v(i).first)
                End If
            Next
        End If
    End Sub

    Public Function slim_parse(ByVal text As String) As vector(Of pair(Of String, node_type))
        Dim r As vector(Of pair(Of String, node_type)) = Nothing
        slim_parse(text, r)
        Return r
    End Function

    Public Function slim_parse_select(ByVal text As String,
                                      ByRef result As vector(Of String),
                                      ByVal type As node_type) As Boolean
        Dim v As vector(Of pair(Of String, node_type)) = Nothing
        If slim_parse(text, v) Then
            assert(Not v Is Nothing)
            If result Is Nothing Then
                result = New vector(Of String)()
            Else
                result.clear()
            End If
            For i As Int32 = 0 To v.size() - 1
                If v(i).second = type Then
                    result.push_back(v(i).first)
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Public Function slim_parse_tags(ByVal text As String, ByRef result As vector(Of String)) As Boolean
        Return slim_parse_select(text, result, node_type.tag)
    End Function

    Public Function slim_parse_texts(ByVal text As String, ByRef result As vector(Of String)) As Boolean
        Return slim_parse_select(text, result, node_type.text)
    End Function

    Public Function slim_parse(ByVal text As String, ByRef result As vector(Of pair(Of String, node_type))) As Boolean
        If result Is Nothing Then
            result = New vector(Of pair(Of String, node_type))()
        Else
            result.clear()
        End If

        If String.IsNullOrEmpty(text) Then
            Return False
        Else
            Dim i As UInt32 = 0
            Dim l As UInt32 = 0
            l = strlen(text)
            Dim r As Boolean = True
            While i < l
                Dim j As Int32 = 0
                j = strindexof(text, tag_leading_str, i, uint32_1)
                If j = npos Then
                    j = l
                End If
                If i < j Then
                    result.emplace_back(pair.of(strmid(text, i, j - i), node_type.text))
                End If
                i = j
                assert(i <= l)
                If i = l Then
                    Exit While
                Else
                    Dim t As node_type = Nothing
                    Dim adv As UInt32 = 0
                    If strsame(text, i, comment_leading, uint32_0, comment_leading_len) Then
                        j = strindexof(text, comment_final, i + comment_leading_len, uint32_1)
                        adv = comment_final_len
                        t = node_type.comment
                    ElseIf strsame(text, i, cdata_leading, uint32_0, cdata_leading_len) Then
                        j = strindexof(text, cdata_final, i + cdata_leading_len, uint32_1)
                        adv = cdata_final_len
                        t = node_type.cdata
                    ElseIf strsame(text, i, declaration_leading, uint32_0, declaration_leading_len) Then
                        j = strindexof(text, declaration_final, i + declaration_leading_len, uint32_1)
                        adv = declaration_final_len
                        t = node_type.declaration
                    ElseIf strsame(text, i, declaration_leading_2, uint32_0, declaration_leading_2_len) Then
                        j = strindexof(text, declaration_final_2_str, i + declaration_leading_2_len, uint32_1)
                        adv = declaration_final_2_len
                        t = node_type.declaration
                    Else 'If strsame(text, i, tag_leading, 0, tag_leading_len) Then
                        'not work with <tag key="value>value">, but is it standard?
                        j = strindexof(text, tag_final, i + tag_leading_len, uint32_1)
                        adv = tag_final_len
                        t = node_type.tag
                    End If

                    If j = npos Then
                        j = l
                        r = False
                    Else
                        j += adv
                    End If
                    assert(i < j)
                    result.emplace_back(pair.of(strmid(text, i, j - i), t))
                    i = j
                End If
            End While

            Return r
        End If
    End Function
End Module
