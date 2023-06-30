
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class rlexer
    Public Class matching_group_creator
        Private Sub New()
        End Sub

        Public Shared Function create(ByVal i As String, ByRef o As matching_group) As Boolean
            Dim pos As UInt32 = uint32_0
            Return create(i, pos, o) AndAlso pos = strlen(i)
        End Function

        'the input string i should be pre-processed, i.e. all the characters have been escaped.
        Public Shared Function create(ByVal i As String, ByRef pos As UInt32, ByRef o As matching_group) As Boolean
            If strlen(i) <= pos Then
                Return False
            Else
                If i(pos) = characters.matching_group_start Then
                    pos += uint32_1
                    Dim e As Int32 = 0
                    e = strindexof(i, characters.matching_group_end, pos, uint32_1)
                    If e = npos Then
                        Return False
                    Else
                        assert(e >= pos)
                        o = Nothing
                        If (e <> pos + uint32_1 OrElse i(pos) <> characters.any_matching) AndAlso
                           Not string_matching_group.create_as_multi_matches(i, pos, CUInt(e) - pos, o) Then
                            Return False
                        Else
                            If o Is Nothing Then
                                o = New any_character_matching_group()
                            End If
                            pos = e + 1
                            While pos < strlen(i)
                                If i(pos) = characters.multi_matching Then
                                    o = New multi_matching_group(o)
                                    pos += uint32_1
                                ElseIf i(pos) = characters.any_matching Then
                                    o = New any_matching_group(o)
                                    pos += uint32_1
                                ElseIf i(pos) = characters.optional_matching Then
                                    o = New optional_matching_group(o)
                                    pos += uint32_1
                                ElseIf i(pos) = characters.reverse_matching Then
                                    o = New reverse_matching_group(o)
                                    pos += uint32_1
                                ElseIf i(pos) = characters.unmatched_matching Then
                                    o = New unmatched_matching_group(o)
                                    pos += uint32_1
                                Else
                                    Exit While
                                End If
                            End While
                            Return True
                        End If
                    End If
                Else
                    Dim e As Int32 = 0
                    e = strindexof(i, characters.matching_group_start, pos, uint32_1)
                    If e = npos Then
                        e = strlen(i)
                    End If
                    o = New string_matching_group(strmid(i, pos, CUInt(e) - pos))
                    pos = e
                    Return True
                End If
            End If
        End Function
    End Class

End Class
