
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.convertor

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
            ss = to_string_array(parameter(c, s, key, variants))
            If ss Is Nothing Then
                Return Nothing
            Else
                Dim r As vector(Of section) = Nothing
                r = New vector(Of section)()
                For i As Int32 = 0 To ss.size() - 1
                    Dim t As section = Nothing
                    If gs(c, ss(i), t, variants) Then
                        r.push_back(t)
                    End If
                Next
                Return r
            End If
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

    Private Function secondary_any_array(Of T)(ByVal c As config,
                                               ByVal section As String,
                                               ByVal key As String,
                                               ByVal variants As vector(Of pair(Of String, String)),
                                               ByVal a As Func(Of section,
                                                                  String, 
                                                                  vector(Of pair(Of String, String)), 
                                                                  vector(Of T))) As vector(Of T)
        assert(Not a Is Nothing)
        Dim s As section = Nothing
        If gs(c, section, s, variants) Then
            Return a(s, key, variants)
        Else
            Return Nothing
        End If
    End Function

    <Extension()> Public Function secondary_string_array(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of String)
        Return secondary_any_array(c, section, key, variants, AddressOf _section.secondary_string_array)
    End Function

    <Extension()> Public Function secondary_bool_array(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Boolean)
        Return secondary_any_array(c, section, key, variants, AddressOf _section.secondary_bool_array)
    End Function

    <Extension()> Public Function secondary_int32_array(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Int32)
        Return secondary_any_array(c, section, key, variants, AddressOf _section.secondary_int32_array)
    End Function

    <Extension()> Public Function secondary_double_array(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Double)
        Return secondary_any_array(c, section, key, variants, AddressOf _section.secondary_double_array)
    End Function

    <Extension()> Public Function secondary_uint32_array(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of UInt32)
        Return secondary_any_array(c, section, key, variants, AddressOf _section.secondary_uint32_array)
    End Function

    Private Function parameter_any_list(Of T)(ByVal c As config,
                                              ByVal section As String,
                                              ByVal base_key As String,
                                              ByVal base_index As Int32,
                                              ByVal variants As vector(Of pair(Of String, String)),
                                              ByVal p As Func(Of section,
                                                                 String, 
                                                                 Int32, 
                                                                 vector(Of pair(Of String, String)), 
                                                                 vector(Of T))) As vector(Of T)
        assert(Not p Is Nothing)
        Dim s As section = Nothing
        If gs(c, section, s, variants) Then
            Return p(s, base_key, base_index, variants)
        Else
            Return Nothing
        End If
    End Function

    Private Function parameter_any_list(Of T)(ByVal c As config,
                                              ByVal section As String,
                                              ByVal base_key As String,
                                              ByVal variants As vector(Of pair(Of String, String)),
                                              ByVal p As Func(Of section,
                                                                 String, 
                                                                 vector(Of pair(Of String, String)), 
                                                                 vector(Of T))) As vector(Of T)
        assert(Not p Is Nothing)
        Dim s As section = Nothing
        If gs(c, section, s, variants) Then
            Return p(s, base_key, variants)
        Else
            Return Nothing
        End If
    End Function

    <Extension()> Public Function parameter_string_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of String)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  base_index,
                                  variants,
                                  AddressOf _section.parameter_string_list)
    End Function

    <Extension()> Public Function parameter_string_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of String)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  variants,
                                  AddressOf _section.parameter_string_list)
    End Function

    <Extension()> Public Function parameter_bool_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Boolean)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  base_index,
                                  variants,
                                  AddressOf _section.parameter_bool_list)
    End Function

    <Extension()> Public Function parameter_bool_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Boolean)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  variants,
                                  AddressOf _section.parameter_bool_list)
    End Function

    <Extension()> Public Function parameter_int32_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Int32)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  base_index,
                                  variants,
                                  AddressOf _section.parameter_int32_list)
    End Function

    <Extension()> Public Function parameter_int32_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Int32)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  variants,
                                  AddressOf _section.parameter_int32_list)
    End Function

    <Extension()> Public Function parameter_double_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Double)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  base_index,
                                  variants,
                                  AddressOf _section.parameter_double_list)
    End Function

    <Extension()> Public Function parameter_double_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Double)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  variants,
                                  AddressOf _section.parameter_double_list)
    End Function

    <Extension()> Public Function parameter_uint32_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of UInt32)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  base_index,
                                  variants,
                                  AddressOf _section.parameter_uint32_list)
    End Function

    <Extension()> Public Function parameter_uint32_list(
                                        ByVal c As config,
                                        ByVal section As String,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of UInt32)
        Return parameter_any_list(c,
                                  section,
                                  base_key,
                                  variants,
                                  AddressOf _section.parameter_uint32_list)
    End Function
End Module
