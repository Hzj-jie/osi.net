
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports variants_type = osi.root.formation.vector(Of osi.root.formation.pair(Of System.String, System.String))

Public Module _section
    Private Function convert(Of T)(ByVal s As String, ByRef o As T) As Boolean
        If string_serializer.from_str(s, o) Then
            Return True
        End If
        raise_error(error_type.user, "Failed to convert parameter ", s, " to ", GetType(T).full_name())
        Return False
    End Function

    <Extension()> Public Function secondary_array(Of T)(ByVal s As section,
                                                        ByVal key As String,
                                                        Optional ByVal variants As variants_type = Nothing) _
                                                       As vector(Of T)
        If s Is Nothing Then
            Return Nothing
        End If
        Dim ss As vector(Of String) = Nothing
        If Not ss.split_from(s(key, variants)) Then
            Return Nothing
        End If
        assert(Not ss.null_or_empty())
        Dim r As vector(Of T) = Nothing
        r = New vector(Of T)()
        For i As UInt32 = 0 To ss.size() - uint32_1
            Dim v As T = Nothing
            If convert(s(ss(i), variants), v) Then
                r.emplace_back(v)
            End If
        Next
        Return r
    End Function

    <Extension()> Public Function parameter_list(Of T)(ByVal s As section,
                                                       ByVal base_key As String,
                                                       ByVal base_index As Int32,
                                                       Optional ByVal variants As variants_type = Nothing) _
                                                      As vector(Of T)
        If s Is Nothing Then
            Return Nothing
        End If
        Dim r As vector(Of T) = Nothing
        r = New vector(Of T)()
        Dim v As String = Nothing
        While s.get(strcat(base_key, base_index), v, variants)
            Dim o As T = Nothing
            If convert(v, o) Then
                r.emplace_back(o)
            End If
            base_index += 1
        End While
        Return r
    End Function

    <Extension()> Public Function parameter_list(Of T)(ByVal s As section,
                                                       ByVal base_key As String,
                                                       Optional ByVal variants As variants_type = Nothing) _
                                                      As vector(Of T)
        Return parameter_list(Of T)(s, base_key, 0, variants)
    End Function
End Module
