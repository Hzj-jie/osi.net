
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class strsplitter
    Private Shared ReadOnly default_separators() As String =
        Function() As String()
            assert(npos < 0)
            Dim default_separators(strlen_i(space_chars) - 1) As String
            For i As Int32 = 0 To strlen_i(space_chars) - 1
                default_separators(i) = Convert.ToString(space_chars(i))
            Next
            Return default_separators
        End Function()
    Private Shared ReadOnly default_surround_strs() As pair(Of String, String) =
        {pair.emplace_of(Convert.ToString(character.quote),
                         Convert.ToString(character.quote)),
         pair.emplace_of(Convert.ToString(character.single_quotation),
                         Convert.ToString(character.single_quotation)),
         pair.emplace_of(Convert.ToString(character.backquote),
                         Convert.ToString(character.backquote))}

    Public Shared Function with_default_separators(ParamArray ByVal separators() As String) As String()
        If isemptyarray(separators) Then
            Return default_separators
        End If
        Return array_concat(separators, default_separators)
    End Function

    Public Shared Function with_default_surround_strs(ByVal ParamArray surround_strs() As pair(Of String, String)) _
                                                     As pair(Of String, String)()
        If isemptyarray(surround_strs) Then
            Return default_surround_strs
        End If
        Return array_concat(surround_strs, default_surround_strs)
    End Function

    Private Shared Function is_inarray(ByVal s As String,
                                       ByVal i As UInt32,
                                       ByVal a() As String,
                                       ByRef l As UInt32,
                                       ByRef hit As String,
                                       ByVal case_sensitive As Boolean) As Boolean
        For j As Int32 = 0 To array_size_i(a) - 1
            l = strlen(a(j))
            If l > 0 AndAlso strsame(s, i, a(j), uint32_0, l, case_sensitive) Then
                hit = a(j)
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function is_inarray(ByVal s As String,
                                       ByVal i As UInt32,
                                       ByVal a() As pair(Of String, String),
                                       ByRef index As Int32,
                                       ByRef l As UInt32,
                                       ByRef hit As String,
                                       ByVal case_sensitive As Boolean) As Boolean
        For j As Int32 = 0 To array_size_i(a) - 1
            l = strlen(a(j).first)
            If l > 0 AndAlso strsame(s, i, a(j).first, 0, l, case_sensitive) Then
                hit = a(j).first
                index = j
                Return True
            End If
        Next
        Return False
    End Function

    Public Shared Function split(ByVal s As String, ByRef result As vector(Of String)) As Boolean
        Return split(s,
                     default_separators,
                     default_surround_strs,
                     result,
                     True,
                     True,
                     False)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean) As Boolean
        Return split(s,
                     default_separators,
                     default_surround_strs,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     False)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal surround_strs() As String,
                                 ByRef result As vector(Of String)) As Boolean
        Return split(s,
                     default_separators,
                     surround_strs,
                     result,
                     True,
                     True,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal surround_strs() As String,
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean) As Boolean
        Return split(s,
                     default_separators,
                     surround_strs,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal separators() As String,
                                 ByVal surround_strs() As String,
                                 ByRef result As vector(Of String)) As Boolean
        Return split(s,
                     separators,
                     surround_strs,
                     result,
                     True,
                     True,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal separators() As String,
                                 ByVal surround_strs() As String,
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean) As Boolean
        Return split(s,
                     separators,
                     surround_strs,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal separators() As String,
                                 ByVal surround_strs() As String,
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean,
                                 ByVal case_sensitive As Boolean) As Boolean
        Dim p() As pair(Of String, String) = Nothing
        ReDim p(array_size_i(surround_strs) - 1)
        For i As Int32 = 0 To array_size_i(surround_strs) - 1
            p(i) = pair.emplace_of(surround_strs(i), surround_strs(i))
        Next
        Return split(s,
                     separators,
                     p,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     case_sensitive)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal surround_strs() As pair(Of String, String),
                                 ByRef result As vector(Of String)) As Boolean
        Return split(s,
                     default_separators,
                     surround_strs,
                     result,
                     True,
                     True,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal surround_strs() As pair(Of String, String),
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean) As Boolean
        Return split(s,
                     default_separators,
                     surround_strs,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal surround_strs() As pair(Of String, String),
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean,
                                 ByVal case_sensitive As Boolean) As Boolean
        Return split(s,
                     default_separators,
                     surround_strs,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     case_sensitive)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal separators() As String,
                                 ByVal surround_strs() As pair(Of String, String),
                                 ByRef result As vector(Of String)) As Boolean
        Return split(s,
                     separators,
                     surround_strs,
                     result,
                     True,
                     True,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal separators() As String,
                                 ByVal surround_strs() As pair(Of String, String),
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean) As Boolean
        Return split(s,
                     separators,
                     surround_strs,
                     result,
                     ignore_empty_entity,
                     ignore_surround_strs,
                     True)
    End Function

    Public Shared Function split(ByVal s As String,
                                 ByVal separators() As String,
                                 ByVal surround_strs() As pair(Of String, String),
                                 ByRef result As vector(Of String),
                                 ByVal ignore_empty_entity As Boolean,
                                 ByVal ignore_surround_strs As Boolean,
                                 ByVal case_sensitive As Boolean) As Boolean
        If s.null_or_empty() Then
            Return False
        End If

        result.renew()
        If isemptyarray(separators) Then
            result.emplace_back(s)
            Return True
        End If

        If isemptyarray(surround_strs) Then
            Dim ss() As String = Nothing
            ss = s.Split(separators,
                         If(ignore_empty_entity,
                            StringSplitOptions.RemoveEmptyEntries,
                            StringSplitOptions.None))
            'should have at least one entity, so false should not be returned
            Return assert(result.emplace_back(ss))
        End If

        Dim surrounded As Int32 = 0
        surrounded = npos
        Dim l As StringBuilder = Nothing
        l = New StringBuilder()
        Dim i As UInt32 = 0
        While i < strlen(s)
            Dim len As UInt32 = 0
            Dim hit As String = Nothing
            If surrounded >= 0 Then
                If strsame(s,
                           i,
                           surround_strs(surrounded).second,
                           uint32_0,
                           strlen(surround_strs(surrounded).second),
                           case_sensitive) Then
                    i += strlen(surround_strs(surrounded).second)
                    If Not ignore_surround_strs Then
                        l.Append(surround_strs(surrounded).second)
                    End If
                    surrounded = npos
                Else
                    l.Append(s(CInt(i)))
                    i += uint32_1
                End If
            ElseIf is_inarray(s, i, separators, len, hit, case_sensitive) Then
                If Not ignore_empty_entity OrElse strlen(l) > 0 Then
                    result.emplace_back(Convert.ToString(l))
                    l.Clear()
                End If
                i += len
            ElseIf is_inarray(s, i, surround_strs, surrounded, len, hit, case_sensitive) Then
                i += len
                If Not ignore_surround_strs Then
                    l.Append(hit)
                End If
            Else
                l.Append(s(CInt(i)))
                i += uint32_1
            End If
        End While
        If Not ignore_empty_entity OrElse strlen(l) > 0 Then
            result.emplace_back(Convert.ToString(l))
        End If
        Return True
    End Function

    Private Sub New()
    End Sub
End Class

Public Module _strsplit
    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplitter.split(s, result)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplitter.split(s, result, ignore_empty_entity, ignore_surround_strs)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplitter.split(s, surround_strs, result)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplitter.split(s, surround_strs, result, ignore_empty_entity, ignore_surround_strs)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplitter.split(s, separators, surround_strs, result)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplitter.split(s,
                                 separators,
                                 surround_strs,
                                 result,
                                 ignore_empty_entity,
                                 ignore_surround_strs)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean,
                                           ByVal case_sensitive As Boolean) As Boolean
        Return strsplitter.split(s,
                                 separators,
                                 surround_strs,
                                 result,
                                 ignore_empty_entity,
                                 ignore_surround_strs,
                                 case_sensitive)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplitter.split(s, surround_strs, result)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplitter.split(s,
                                 surround_strs,
                                 result,
                                 ignore_empty_entity,
                                 ignore_surround_strs)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean,
                                           ByVal case_sensitive As Boolean) As Boolean
        Return strsplitter.split(s,
                                 surround_strs,
                                 result,
                                 ignore_empty_entity,
                                 ignore_surround_strs,
                                 case_sensitive)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplitter.split(s, separators, surround_strs, result)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplitter.split(s,
                                 separators,
                                 surround_strs,
                                 result,
                                 ignore_empty_entity,
                                 ignore_surround_strs)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean,
                                           ByVal case_sensitive As Boolean) As Boolean
        Return strsplitter.split(s,
                                 separators,
                                 surround_strs,
                                 result,
                                 ignore_empty_entity,
                                 ignore_surround_strs,
                                 case_sensitive)
    End Function

    <Extension()> Public Function split_from(ByRef o As vector(Of String), ByVal s As String) As Boolean
        Return strsplit(s, o)
    End Function

    <Extension()> Public Function split_from_or_null(ByRef o As vector(Of String),
                                                     ByVal s As String) As vector(Of String)
        If split_from(o, s) Then
            Return o
        End If
        Return Nothing
    End Function

    <Extension()> Public Function split_from(Of T)(ByRef o As vector(Of T), ByVal s As String) As Boolean
        Dim r As vector(Of String) = Nothing
        If Not strsplit(s, r) Then
            Return False
        End If
        o.renew()
        Dim i As UInt32 = 0
        While i < r.size()
            Dim v As T = Nothing
            If Not string_serializer.from_str(r(i), v) Then
                Return False
            End If
            o.emplace_back(v)
            i += uint32_1
        End While
        Return True
    End Function

    <Extension()> Public Function split_from_or_null(Of T)(ByRef o As vector(Of T), ByVal s As String) As vector(Of T)
        If split_from(o, s) Then
            Return o
        End If
        Return Nothing
    End Function
End Module