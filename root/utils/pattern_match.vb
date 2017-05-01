
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants

' TODO: Update the name of functions in this file.
Public Module _pattern_matching
    <Extension()> Public Function match_pattern(ByVal str As String,
                                                ByVal patterns As vector(Of String),
                                                Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return fitfilterSet(patterns, str, case_sensitive)
    End Function

    <Extension()> Public Function match_pattern(ByVal str As String,
                                                ByVal pattern As String,
                                                Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return fitfilter(pattern, str, case_sensitive)
    End Function

    <Extension()> Public Function match_one(ByVal str As String,
                                            ByVal pattern As String,
                                            Optional ByVal case_sensitive As Boolean = False) As Byte
        Return pattern_match.fitfilter(pattern, str, case_sensitive)
    End Function

    <Extension()> Public Function match_all(ByVal str As String,
                                            ByVal patterns As vector(Of String),
                                            Optional ByVal case_sensitive As Boolean = False) As Byte
        Return pattern_match.fit_filter_list(patterns, str, case_sensitive)
    End Function

    Public Function fitfilterSet(ByVal filterSet As vector(Of String),
                                 ByVal str As String,
                                 Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return pattern_match.fitfilterList(filterSet, str, case_sensitive)
    End Function

    Public Function fitfilter(ByVal filter As String,
                              ByVal str As String,
                              Optional ByVal case_sensitive As Boolean = False) As Boolean
        Dim rtn As Int32
        Dim extCode As Boolean = False
        rtn = pattern_match.fitfilter(filter, str, case_sensitive)
        If rtn = pattern_match.fit_undertermind Then
            If strlen(filter) > 0 AndAlso filter(0) = pattern_match.cut_sign Then
                extCode = True
            Else
                extCode = False
            End If
        Else
            extCode = (rtn = pattern_match.fit_true)
        End If

        Return extCode
    End Function
End Module

Public NotInheritable Class pattern_match
    Public Const fit_true As Byte = 1
    Public Const fit_false As Byte = 2
    Public Const fit_undertermind As Byte = 0
    Public Const cut_sign As Char = character.minus_sign
    Public Const add_sign As Char = character.plus_sign
    Public Const multi_char_sign As Char = filesystem.multi_pattern_matching_character
    Public Const single_char_sign As Char = filesystem.single_pattern_matching_character

    Friend Shared Function fit_filter_list(ByVal filters As vector(Of String),
                                           ByVal str As String,
                                           Optional ByVal case_sensitive As Boolean = False) As Byte
        If filters Is Nothing Then
            Return fit_undertermind
        End If
        Dim fitted As Byte = fit_undertermind
        For i As Int32 = 0 To filters.size() - 1
            Dim r As Byte = 0
            r = fitfilter(filters(i), str, case_sensitive)
            If r = fit_true Then
                fitted = fit_true
            ElseIf r = fit_false Then
                fitted = fit_false
            End If
        Next
        Return fitted
    End Function

    Friend Shared Function fitfilterList(ByVal filterSet As vector(Of String),
                                         ByVal name As String,
                                         Optional ByVal case_sensitive As Boolean = False) As Boolean
        If filterSet Is Nothing OrElse filterSet.empty() Then
            Return False
        Else
            Dim fitted As Boolean = False
            For i As Int32 = 0 To filterSet.size() - 1
                Dim result As Byte
                result = fitfilter(filterSet(i), name, case_sensitive)
                If result = fit_false Then
                    fitted = False
                ElseIf result = fit_true Then
                    fitted = True
                End If
            Next

            Return fitted
        End If
    End Function

    Private Shared Function fitfilterLite(ByVal definition As String,
                                          ByVal definition_start As Int32,
                                          ByVal name As String,
                                          ByVal name_start As Int32,
                                          Optional ByVal case_sensitive As Boolean = False) As Boolean
        Dim i As Int32 = definition_start
        Dim j As Int32 = name_start
        Dim definition_len As Int32 = strlen(definition)
        Dim name_len As Int32 = strlen(name)

        While (i < definition_len AndAlso j < name_len)
            If definition(i) = multi_char_sign Then
                i += 1
                While (i < definition_len AndAlso definition(i) = multi_char_sign)
                    i += 1
                End While
                While (j <= name_len)
                    If fitfilterLite(definition,
                                         i,
                                         name,
                                         j,
                                         case_sensitive) Then
                        Return True
                    Else
                        j += 1
                    End If
                End While
                Return False
            ElseIf definition(i) = single_char_sign Then
                i += 1
                j += 1
            Else
                If charsame(definition(i), name(j), case_sensitive) Then
                    i += 1
                    j += 1
                Else
                    Exit While
                End If
            End If
        End While

        While i < definition_len AndAlso definition(i) = multi_char_sign
            i += 1
        End While
        Return i = definition_len AndAlso j = name_len
    End Function

    Private Shared Function fitfilterLite(ByVal definition As String,
                                          ByVal name As String,
                                          Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return fitfilterLite(definition,
                             0,
                             name,
                             0,
                             case_sensitive)
    End Function

    Friend Shared Function fitfilter(ByVal definition As String,
                                     ByVal name As String,
                                     Optional ByVal case_sensitive As Boolean = False) As Byte
        If strlen(definition) > 0 AndAlso definition(0) = cut_sign Then
            If (fitfilterLite(strmid(definition, 1), name, case_sensitive)) Then
                Return fit_false
            Else
                Return fit_undertermind
            End If
        Else
            Dim offside As Byte = 0
            If strlen(definition) > 0 AndAlso definition(0) = add_sign Then
                offside = 1
            End If
            If (fitfilterLite(strmid(definition, offside), name, case_sensitive)) Then
                Return fit_true
            Else
                Return fit_undertermind
            End If
        End If
    End Function

    Private Sub New()
    End Sub
End Class
