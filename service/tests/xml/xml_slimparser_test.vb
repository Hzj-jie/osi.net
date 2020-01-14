
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.formation
Imports osi.service.xml

Public Class xml_slimparser_test
    Inherits [case]

    Private Const xml1 As String = "<?xml charset=""utf-8""?>"
    Private Const xml2 As String = "<some_tag key=""value"" key='value'>" +
                                       "some_text" +
                                   "</some_tag>" +
                                   "<some_tag2>" +
                                       "<some_tag3>some_text2</some_tag3>" +
                                   "</some_tag2>"

    Private Shared Sub create(ByVal r As vector(Of pair(Of String, node_type)))
        assert(Not r Is Nothing)
        Dim selector As Int32 = 0
        selector = rnd_int(0, 5)
        Dim s As String = Nothing
        Dim t As node_type = Nothing
        Select Case selector
            Case 0
                s = rnd_start_tag()
                t = node_type.tag
            Case 1
                s = rnd_end_tag()
                t = node_type.tag
            Case 2
                s = rnd_loosen_comment()
                t = node_type.comment
            Case 3
                If r.empty() OrElse r.back().second <> node_type.text Then
                    s = rnd_text()
                    t = node_type.text
                Else
                    s = rnd_loosen_comment()
                    t = node_type.comment
                End If
            Case 4
                s = rnd_loosen_cdata()
                t = node_type.cdata
            Case Else
                assert(False)
        End Select
        r.emplace_back(pair.of(s, t))
    End Sub

    Private Shared Function auto_test() As Boolean
        Dim v1 As vector(Of pair(Of String, node_type)) = Nothing
        Dim v2 As vector(Of pair(Of String, node_type)) = Nothing
        v1 = New vector(Of pair(Of String, node_type))()
        For i As Int32 = 0 To (rnd(500, 1000) * If(isdebugbuild(), 1, 10)) - 1
            create(v1)
        Next
        Dim s As Text.StringBuilder = Nothing
        s = New Text.StringBuilder()
        For i As Int32 = 0 To v1.size() - 1
            s.Append(v1(i).first)
        Next
        assertion.is_true(slim_parse(Convert.ToString(s), v2))
        If assertion.equal(v1.size(), v2.size()) Then
            For i As Int32 = 0 To v1.size() - 1
                assertion.equal(v1(i).second, v2(i).second)
                assertion.is_true(strsame(v1(i).first, v2(i).first))
            Next
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim r As vector(Of pair(Of String, node_type)) = Nothing
        slim_parse(xml1, r)
        If assertion.equal(r.size(), CUInt(1)) Then
            assertion.is_true(strsame(r(0).first, "<?xml charset=""utf-8""?>"))
            assertion.is_true(r(0).second = node_type.declaration)
        End If

        slim_parse(xml2, r)
        If assertion.equal(r.size(), CUInt(8)) Then
            assertion.is_true(r(0).second = node_type.tag)
            assertion.is_true(strsame(r(0).first, "<some_tag key=""value"" key='value'>"))

            assertion.is_true(r(1).second = node_type.text)
            assertion.is_true(strsame(r(1).first, "some_text"))

            assertion.is_true(r(2).second = node_type.tag)
            assertion.is_true(strsame(r(2).first, "</some_tag>"))

            assertion.is_true(r(3).second = node_type.tag)
            assertion.is_true(strsame(r(3).first, "<some_tag2>"))

            assertion.is_true(r(4).second = node_type.tag)
            assertion.is_true(strsame(r(4).first, "<some_tag3>"))

            assertion.is_true(r(5).second = node_type.text)
            assertion.is_true(strsame(r(5).first, "some_text2"))

            assertion.is_true(r(6).second = node_type.tag)
            assertion.is_true(strsame(r(6).first, "</some_tag3>"))

            assertion.is_true(r(7).second = node_type.tag)
            assertion.is_true(strsame(r(7).first, "</some_tag2>"))
        End If

        For i As Int32 = 0 To 1024 * If(isdebugbuild(), 1, 8) - 1
            If Not auto_test() Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
