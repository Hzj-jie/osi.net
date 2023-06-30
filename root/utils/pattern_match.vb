
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' TODO: Update the name of functions in this file.
Public Module _pattern_matching
    <Extension()> Public Function match_patterns(ByVal str As String,
                                                 ByVal patterns As vector(Of String),
                                                 Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return pattern_match.match_filters(patterns, str, case_sensitive)
    End Function

    <Extension()> Public Function match_pattern(ByVal str As String,
                                                ByVal pattern As String,
                                                Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return pattern_match.match_filter(pattern, str, case_sensitive)
    End Function

    <Extension()> Public Function fit_pattern(ByVal str As String,
                                              ByVal pattern As String,
                                              Optional ByVal case_sensitive As Boolean = False) As Byte
        Return pattern_match.fit_filter(pattern, str, case_sensitive)
    End Function

    <Extension()> Public Function fit_patterns(ByVal str As String,
                                               ByVal patterns As vector(Of String),
                                               Optional ByVal case_sensitive As Boolean = False) As Byte
        Return pattern_match.fit_filters(patterns, str, case_sensitive)
    End Function
End Module

Public NotInheritable Class pattern_match
    Public Const fit_true As Byte = 1
    Public Const fit_false As Byte = 2
    Public Const fit_undertermined As Byte = 0
    Public Const cut_sign As Char = character.minus_sign
    Public Const add_sign As Char = character.plus_sign
    Public Const multi_char_sign As Char = filesystem.multi_pattern_matching_character
    Public Const single_char_sign As Char = filesystem.single_pattern_matching_character

    Friend Shared Function fit_filters(ByVal filters As vector(Of String),
                                       ByVal str As String,
                                       Optional ByVal case_sensitive As Boolean = False) As Byte
        If filters.null_or_empty() Then
            Return fit_undertermined
        End If
        Dim fitted As Byte = fit_undertermined
        For i As UInt32 = 0 To filters.size() - uint32_1
            Dim r As Byte = 0
            r = fit_filter(filters(i), str, case_sensitive)
            If r = fit_true Then
                fitted = fit_true
            ElseIf r = fit_false Then
                fitted = fit_false
            End If
        Next
        Return fitted
    End Function

    Friend Shared Function match_filters(ByVal filters As vector(Of String),
                                         ByVal name As String,
                                         Optional ByVal case_sensitive As Boolean = False) As Boolean
        If filters.null_or_empty() Then
            Return False
        Else
            Dim fitted As Boolean = False
            For i As UInt32 = 0 To filters.size() - uint32_1
                Dim result As Byte
                result = fit_filter(filters(i), name, case_sensitive)
                If result = fit_false Then
                    fitted = False
                ElseIf result = fit_true Then
                    fitted = True
                End If
            Next

            Return fitted
        End If
    End Function

    Private Shared Function match_filter_without_prefix(ByVal definition As String,
                                                        ByVal definition_start As Int32,
                                                        ByVal name As String,
                                                        ByVal name_start As Int32,
                                                        Optional ByVal case_sensitive As Boolean = False) As Boolean
        Dim i As Int32 = 0
        i = definition_start
        Dim j As Int32 = 0
        j = name_start
        Dim definition_len As UInt32 = 0
        definition_len = strlen(definition)
        Dim name_len As UInt32 = 0
        name_len = strlen(name)

        While (i < +definition_len AndAlso j < +name_len)
            If definition(i) = multi_char_sign Then
                i += 1
                While (i < +definition_len AndAlso definition(i) = multi_char_sign)
                    i += 1
                End While
                While (j <= +name_len)
                    If match_filter_without_prefix(definition,
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

        While i < +definition_len AndAlso definition(i) = multi_char_sign
            i += 1
        End While
        Return i = +definition_len AndAlso j = +name_len
    End Function

    Private Shared Function match_filter_without_prefix(ByVal definition As String,
                                                        ByVal name As String,
                                                        Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return match_filter_without_prefix(definition,
                                           0,
                                           name,
                                           0,
                                           case_sensitive)
    End Function

    Private Shared Function execute_filter(Of T)(ByVal definition As String,
                                                 ByVal name As String,
                                                 ByVal match_cut As T,
                                                 ByVal match_add As T,
                                                 ByVal unmatch_cut As T,
                                                 ByVal unmatch_add As T,
                                                 Optional ByVal case_sensitive As Boolean = False) As T
        If strlen(definition) > 0 AndAlso definition(0) = cut_sign Then
            If (match_filter_without_prefix(strmid(definition, 1), name, case_sensitive)) Then
                Return match_cut
            Else
                Return unmatch_cut
            End If
        Else
            Dim offside As Byte = 0
            If strlen(definition) > 0 AndAlso definition(0) = add_sign Then
                offside = 1
            End If
            If (match_filter_without_prefix(strmid(definition, offside), name, case_sensitive)) Then
                Return match_add
            Else
                Return unmatch_add
            End If
        End If
    End Function

    Friend Shared Function fit_filter(ByVal definition As String,
                                      ByVal name As String,
                                      Optional ByVal case_sensitive As Boolean = False) As Byte
        Return execute_filter(definition,
                              name,
                              fit_false,
                              fit_true,
                              fit_undertermined,
                              fit_undertermined,
                              case_sensitive)
    End Function

    Friend Shared Function match_filter(ByVal definition As String,
                                        ByVal name As String,
                                        Optional ByVal case_sensitive As Boolean = False) As Boolean
        Return execute_filter(definition,
                              name,
                              False,
                              True,
                              True,
                              False,
                              case_sensitive)
    End Function

    Private Sub New()
    End Sub
End Class
