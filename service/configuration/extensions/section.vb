
Imports System.Runtime.CompilerServices
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.convertor

Public Module _section
    <Extension()> Public Function secondary_any_array(Of T)(ByVal s As section,
                                                            ByVal key As String,
                                                            ByVal variants As vector(Of pair(Of String, String)),
                                                            ByVal c As Func(Of String, T)) _
                                                           As vector(Of T)
        assert(Not c Is Nothing)
        If s Is Nothing Then
            Return Nothing
        Else
            Dim ss As vector(Of String) = Nothing
            ss = to_string_array(s(key, variants))
            If ss Is Nothing Then
                Return Nothing
            Else
                Dim r As vector(Of T) = Nothing
                r = New vector(Of T)()
                For i As Int32 = 0 To ss.size() - 1
                    r.push_back(c(s(ss(i), variants)))
                Next
                Return r
            End If
        End If
    End Function

    <Extension()> Public Function secondary_string_array(
                                        ByVal s As section,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of String)
        Return secondary_any_array(s, key, variants, AddressOf _string_others.to_string)
    End Function

    <Extension()> Public Function secondary_bool_array(
                                        ByVal s As section,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Boolean)
        Return secondary_any_array(s, key, variants, AddressOf _string_others.to_bool)
    End Function

    <Extension()> Public Function secondary_int32_array(
                                        ByVal s As section,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Int32)
        Return secondary_any_array(s, key, variants, AddressOf _string_others.to_int32)
    End Function

    <Extension()> Public Function secondary_double_array(
                                        ByVal s As section,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Double)
        Return secondary_any_array(s, key, variants, AddressOf _string_others.to_double)
    End Function

    <Extension()> Public Function secondary_uint32_array(
                                        ByVal s As section,
                                        ByVal key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of UInt32)
        Return secondary_any_array(s, key, variants, AddressOf _string_others.to_uint32)
    End Function

    Private Function parameter_any_list(Of T)(ByVal s As section,
                                              ByVal base_key As String,
                                              ByVal base_index As Int32,
                                              ByVal variants As vector(Of pair(Of String, String)),
                                              ByVal c As Func(Of String, T)) As vector(Of T)
        assert(Not c Is Nothing)
        If s Is Nothing Then
            Return Nothing
        Else
            Dim r As vector(Of T) = Nothing
            r = New vector(Of T)()
            Dim v As String = Nothing
            While s.get(strcat(base_key, base_index), v, variants)
                r.push_back(c(v))
                base_index += 1
            End While
            Return r
        End If
    End Function

    Private Function parameter_any_list(Of T)(ByVal s As section,
                                              ByVal base_key As String,
                                              ByVal variants As vector(Of pair(Of String, String)),
                                              ByVal c As Func(Of String, T)) As vector(Of T)
        Return parameter_any_list(s, base_key, 0, variants, c)
    End Function

    <Extension()> Public Function parameter_string_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of String)
        Return parameter_any_list(s, base_key, base_index, variants, AddressOf _string_others.to_string)
    End Function

    <Extension()> Public Function parameter_string_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of String)
        Return parameter_any_list(s, base_key, variants, AddressOf _string_others.to_string)
    End Function

    <Extension()> Public Function parameter_bool_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Boolean)
        Return parameter_any_list(s, base_key, base_index, variants, AddressOf _string_others.to_bool)
    End Function

    <Extension()> Public Function parameter_bool_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Boolean)
        Return parameter_any_list(s, base_key, variants, AddressOf _string_others.to_bool)
    End Function

    <Extension()> Public Function parameter_int32_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Int32)
        Return parameter_any_list(s, base_key, base_index, variants, AddressOf _string_others.to_int32)
    End Function

    <Extension()> Public Function parameter_int32_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Int32)
        Return parameter_any_list(s, base_key, variants, AddressOf _string_others.to_int32)
    End Function

    <Extension()> Public Function parameter_double_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Double)
        Return parameter_any_list(s, base_key, base_index, variants, AddressOf _string_others.to_double)
    End Function

    <Extension()> Public Function parameter_double_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of Double)
        Return parameter_any_list(s, base_key, variants, AddressOf _string_others.to_double)
    End Function

    <Extension()> Public Function parameter_uint32_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        ByVal base_index As Int32,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of UInt32)
        Return parameter_any_list(s, base_key, base_index, variants, AddressOf _string_others.to_uint32)
    End Function

    <Extension()> Public Function parameter_uint32_list(
                                        ByVal s As section,
                                        ByVal base_key As String,
                                        Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) _
                                        As vector(Of UInt32)
        Return parameter_any_list(s, base_key, variants, AddressOf _string_others.to_uint32)
    End Function
End Module
