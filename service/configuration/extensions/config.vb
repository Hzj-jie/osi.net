
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Public Module _config
    Private Function gs(ByVal c As config,
                        ByVal s As String,
                        ByRef o As section,
                        ByVal variants As vector(Of pair(Of String, String))) As Boolean
        Return Not c Is Nothing AndAlso c.get(s, o, variants) AndAlso assert(Not s Is Nothing)
    End Function

    <Extension()> Public Function sections(ByVal c As config,
                                           ByVal base_section As String,
                                           ByVal base_index As Int32,
                                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                          As vector(Of section)
        If c Is Nothing Then
            Return Nothing
        Else
            Dim r As vector(Of section) = Nothing
            r = New vector(Of section)()
            Dim s As section = Nothing
            While gs(c, strcat(base_section, base_index), s, variants)
                r.push_back(s)
                base_index += 1
            End While
            Return r
        End If
    End Function

    <Extension()> Public Function sections(ByVal c As config,
                                           ByVal base_section As String,
                                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                          As vector(Of section)
        Return sections(c, base_section, 0, variants)
    End Function

    <Extension()> Public Function secondary_sections(
                                        ByVal c As config,
                                        ByVal s As String,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                       As vector(Of section)
        If s Is Nothing Then
            Return Nothing
        Else
            Dim ss As vector(Of String) = Nothing
            If Not ss.split_from(parameter(c, s, key, variants)) Then
                Return Nothing
            End If
            assert(Not ss.null_or_empty())
            Dim r As vector(Of section) = Nothing
            r = New vector(Of section)()
            For i As UInt32 = 0 To ss.size() - uint32_1
                Dim t As section = Nothing
                If gs(c, ss(i), t, variants) Then
                    r.push_back(t)
                End If
            Next
            Return r
        End If
    End Function

    <Extension()> Public Function parameter(ByVal c As config,
                                            ByVal section As String,
                                            ByVal key As String,
                                            Optional ByVal default_value As String = Nothing) As String
        Return parameter(c, section, key, Nothing, default_value)
    End Function

    <Extension()> Public Function parameter(ByVal c As config,
                                            ByVal section As String,
                                            ByVal key As String,
                                            ByVal variants As vector(Of pair(Of String, String)),
                                            Optional ByVal default_value As String = Nothing) As String
        Dim s As section = Nothing
        If gs(c, section, s, variants) Then
            Return s(key, variants, default_value)
        Else
            Return default_value
        End If
    End Function

    Private Function secondary_array(Of T)(ByVal c As config,
                                           ByVal section As String,
                                           ByVal key As String,
                                           Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing,
                                           Optional ByVal str_T As string_serializer(Of T) = Nothing) _
                                          As vector(Of T)
        Dim s As section = Nothing
        If gs(c, section, s, variants) Then
            Return _section.secondary_array(s, key, variants, str_T)
        End If
        Return Nothing
    End Function

    Public Function parameter_list(Of T)(ByVal c As config,
                                         ByVal section As String,
                                         ByVal base_key As String,
                                         ByVal base_index As Int32,
                                         Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing,
                                         Optional ByVal str_T As string_serializer(Of T) = Nothing) _
                                        As vector(Of T)
        Dim s As section = Nothing
        If gs(c, section, s, variants) Then
            Return _section.parameter_list(s, base_key, base_index, variants, str_T)
        End If
        Return Nothing
    End Function

    Public Function parameter_list(Of T)(ByVal c As config,
                                         ByVal section As String,
                                         ByVal base_key As String,
                                         Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing,
                                         Optional ByVal str_T As string_serializer(Of T) = Nothing) _
                                        As vector(Of T)
        Return parameter_list(c, section, base_key, 0, variants, str_T)
    End Function
End Module
