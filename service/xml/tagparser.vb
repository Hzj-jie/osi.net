
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _tagparser
    Private ReadOnly attribute_surrounding() As pair(Of String, String)

    Sub New()
        ReDim attribute_surrounding(1)
        attribute_surrounding = {pair.emplace_of(constants.value_leading_str,
                                                   constants.value_final_str),
                                 pair.emplace_of(constants.value_leading_2_str,
                                                   constants.value_final_2_str)}
    End Sub

    Public Function parse_tag(ByVal line As String,
                              ByRef tag As String,
                              ByRef attributes As vector(Of pair(Of String, String))) As Boolean
        Return parse_tag(line,
                         tag,
                         attributes,
                         Nothing,
                         Nothing,
                         True)
    End Function

    Public Function parse_tag(ByVal line As String,
                              ByRef tag As String,
                              ByRef attributes As vector(Of pair(Of String, String)),
                              ByRef self_close As Boolean,
                              ByRef close_tag As Boolean) As Boolean
        Return parse_tag(line,
                         tag,
                         attributes,
                         self_close,
                         close_tag,
                         True)
    End Function

    Public Function parse_tag(ByVal line As String,
                              ByRef tag As String,
                              ByRef attributes As vector(Of pair(Of String, String)),
                              ByRef self_close As Boolean,
                              ByRef close_tag As Boolean,
                              ByVal remove_attribute_surroundings As Boolean) As Boolean
        If String.IsNullOrEmpty(line) Then
            Return False
        Else
            line = line.Trim()
            If strstartwith(line, constants.tag_leading_str) AndAlso
               strendwith(line, constants.tag_final_str) Then
                line = strmid(line,
                              constants.tag_leading_len,
                              strlen(line) - constants.tag_leading_len - constants.tag_final_len)
                If String.IsNullOrEmpty(line) Then
                    'line = <>
                    Return False
                Else
                    line = line.Trim()
                    If String.IsNullOrEmpty(line) OrElse
                       strstartwith(line, constants.tag_leading_str) OrElse
                       strendwith(line, constants.tag_final_str) Then
                        'line = <    > or line = <<abc ...
                        Return False
                    Else
                        self_close = False
                        close_tag = False
                        If strendwith(line, constants.tag_close_mark_str) Then
                            self_close = True
                            line = strleft(line, strlen(line) - constants.tag_close_mark_len)
                        ElseIf strstartwith(line, constants.tag_close_mark_str) Then
                            close_tag = True
                            line = strright(line, strlen(line) - constants.tag_close_mark_len)
                        End If
                        If strstartwith(line, constants.tag_close_mark_str) OrElse
                           strendwith(line, constants.tag_close_mark_str) Then
                            'line = <// abc> or <abc //>
                            Return False
                        Else
                            Dim v As vector(Of String) = Nothing
                            If strsplit(line, attribute_surrounding, v, True, False, False) AndAlso
                               v IsNot Nothing AndAlso
                               Not v.empty() Then
                                tag = v(0)
                                attributes.renew()
                                For i As Int32 = 1 To v.size() - 1
                                    Dim f As String = Nothing
                                    Dim s As String = Nothing
                                    If strsep(v(i), f, s, constants.attribute_separator_str, False) AndAlso
                                       f IsNot Nothing Then
                                        f = f.Trim()
                                        If s IsNot Nothing Then
                                            s = s.Trim()
                                        End If
                                        If String.IsNullOrEmpty(f) Then
                                            Return False
                                        Else
                                            If remove_attribute_surroundings AndAlso
                                               Not String.IsNullOrEmpty(s) Then
                                                assert(array_size(attribute_surrounding) = 2)
                                                If strstartwith(s, constants.value_leading_str) AndAlso
                                                   strendwith(s, constants.value_final_str) Then
                                                    s = strmid(s,
                                                               constants.value_leading_len,
                                                               strlen(s) -
                                                               constants.value_leading_len -
                                                               constants.value_final_len)
                                                ElseIf strstartwith(s, constants.value_leading_2_str) AndAlso
                                                       strendwith(s, constants.value_final_2_str) Then
                                                    s = strmid(s,
                                                               constants.value_leading_2_len,
                                                               strlen(s) -
                                                               constants.value_leading_2_len -
                                                               constants.value_final_2_len)
                                                End If
                                            End If
                                            attributes.emplace_back(pair.emplace_of(f, s))
                                        End If
                                    Else
                                        Return False
                                    End If
                                Next
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
            Else
                Return False
            End If
        End If
    End Function
End Module
