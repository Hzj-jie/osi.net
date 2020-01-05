
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class rlexer
    'parse the input into a list of words
    Public Function match(ByVal i As String,
                          ByVal start As UInt32,
                          ByVal [end] As UInt32,
                          ByRef o As vector(Of typed_word)) As Boolean
        If start > [end] OrElse strlen(i) < [end] Then
            Return False
        End If
        If start = [end] Then
            Return True
        End If
        If o Is Nothing Then
            o = New vector(Of typed_word)()
        End If
        Dim j As UInt32 = 0
        j = start
        While j < [end]
            Dim matches As vector(Of pair(Of UInt32, UInt32)) = Nothing
            matches = New vector(Of pair(Of UInt32, UInt32))()
            For k As UInt32 = 0 To rs.size() - uint32_1
                assert(Not rs(k) Is Nothing)
                If word_choice = match_choice.greedy Then
                    Dim new_pos As UInt32 = 0
                    If rs(k).longest_match(i, j, new_pos) Then
                        If new_pos <= [end] Then
                            matches.emplace_back(emplace_make_pair(k, new_pos))
                            If new_pos = [end] Then
                                Exit For
                            End If
                        End If
                    End If
                Else
                    Dim v As vector(Of UInt32) = Nothing
                    If rs(k).match(i, j, v) AndAlso assert(Not v.null_or_empty()) Then
                        For l As UInt32 = 0 To v.size() - uint32_1
                            If v(l) <= [end] Then
                                matches.emplace_back(emplace_make_pair(k, v(l)))
                                If word_choice = match_choice.first_defined OrElse v(l) = [end] Then
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
                If type_choice = match_choice.first_defined AndAlso Not matches.empty() Then
                    Exit For
                End If
            Next
            If matches.empty() Then
                Return False
            End If
            If matches.back().second = [end] Then
                o.emplace_back(New typed_word(i,
                                              j,
                                              matches.back().second,
                                              matches.back().first))
                Return True
            End If
            If matches.size() = uint32_1 OrElse type_choice = match_choice.first_defined Then
                o.emplace_back(New typed_word(i, j, matches(0).second, matches(0).first))
                j = matches(0).second
            Else
                If type_choice = match_choice.multipath Then
                    For k As UInt32 = 0 To matches.size() - uint32_1
                        o.emplace_back(New typed_word(i, j, matches(k).second, matches(k).first))
                        If match(i, matches(k).second, [end], o) Then
                            Return True
                        Else
                            o.pop_back()
                        End If
                    Next
                    Return False
                End If
                assert(type_choice = match_choice.greedy)
                Dim max_id As UInt32 = 0
                For k As UInt32 = uint32_1 To matches.size() - uint32_1
                    If matches(k).second > matches(max_id).second Then
                        max_id = k
                    End If
                Next
                o.emplace_back(New typed_word(i, j, matches(max_id).second, matches(max_id).first))
                j = matches(max_id).second
            End If
        End While
        Return j = [end]
    End Function

    Public Function match(ByVal i As String,
                          ByVal start As UInt32,
                          ByRef o As vector(Of typed_word)) As Boolean
        Return match(i, start, strlen(i), o)
    End Function

    Public Function match(ByVal i As String, ByRef o As vector(Of typed_word)) As Boolean
        Return match(i, uint32_0, o)
    End Function
End Class
