
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Public Module _strsplit
    Private ReadOnly default_separators() As String
    Private ReadOnly default_surround_strs() As pair(Of String, String)

    Sub New()
        assert(npos < 0)
        ReDim default_separators(strlen(space_chars) - 1)
        For i As Int32 = 0 To strlen(space_chars) - 1
            default_separators(i) = Convert.ToString(space_chars(i))
        Next
        default_surround_strs = {emplace_make_pair(Convert.ToString(character.quote),
                                                   Convert.ToString(character.quote)),
                                 emplace_make_pair(Convert.ToString(character.single_quotation),
                                                   Convert.ToString(character.single_quotation)),
                                 emplace_make_pair(Convert.ToString(character.backquote),
                                                   Convert.ToString(character.backquote))}
    End Sub

    Private Function is_inarray(ByVal s As String,
                                ByVal i As UInt32,
                                ByVal a() As String,
                                ByRef l As UInt32,
                                ByRef hit As String,
                                ByVal case_sensitive As Boolean) As Boolean
        For j As Int32 = 0 To array_size(a) - 1
            l = strlen(a(j))
            If l > 0 AndAlso
               strsame(s, i, a(j), uint32_0, l, case_sensitive) Then
                hit = a(j)
                Return True
            End If
        Next
        Return False
    End Function

    Private Function is_inarray(ByVal s As String,
                                ByVal i As UInt32,
                                ByVal a() As pair(Of String, String),
                                ByRef index As Int32,
                                ByRef l As UInt32,
                                ByRef hit As String,
                                ByVal case_sensitive As Boolean) As Boolean
        For j As Int32 = 0 To array_size(a) - 1
            l = strlen(a(j).first)
            If l > 0 AndAlso
               strsame(s, i, a(j).first, 0, l, case_sensitive) Then
                hit = a(j).first
                index = j
                Return True
            End If
        Next
        Return False
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplit(s,
                        default_separators,
                        default_surround_strs,
                        result,
                        True,
                        True,
                        False)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplit(s,
                        default_separators,
                        default_surround_strs,
                        result,
                        ignore_empty_entity,
                        ignore_surround_strs,
                        False)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplit(s,
                        default_separators,
                        surround_strs,
                        result,
                        True,
                        True,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplit(s,
                        default_separators,
                        surround_strs,
                        result,
                        ignore_empty_entity,
                        ignore_surround_strs,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplit(s,
                        separators,
                        surround_strs,
                        result,
                        True,
                        True,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplit(s,
                        separators,
                        surround_strs,
                        result,
                        ignore_empty_entity,
                        ignore_surround_strs,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As String,
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean,
                                           ByVal case_sensitive As Boolean) As Boolean
        Dim p() As pair(Of String, String) = Nothing
        ReDim p(array_size(surround_strs) - 1)
        For i As Int32 = 0 To array_size(surround_strs) - 1
            p(i) = emplace_make_pair(surround_strs(i), surround_strs(i))
        Next
        Return strsplit(s,
                        separators,
                        p,
                        result,
                        ignore_empty_entity,
                        ignore_surround_strs,
                        case_sensitive)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String)) As Boolean
        Return strsplit(s,
                        default_separators,
                        surround_strs,
                        result,
                        True,
                        True,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplit(s,
                        default_separators,
                        surround_strs,
                        result,
                        ignore_empty_entity,
                        ignore_surround_strs,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean,
                                           ByVal case_sensitive As Boolean) As Boolean
        Return strsplit(s,
                        default_separators,
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
        Return strsplit(s,
                        separators,
                        surround_strs,
                        result,
                        True,
                        True,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean) As Boolean
        Return strsplit(s,
                        separators,
                        surround_strs,
                        result,
                        ignore_empty_entity,
                        ignore_surround_strs,
                        True)
    End Function

    <Extension()> Public Function strsplit(ByVal s As String,
                                           ByVal separators() As String,
                                           ByVal surround_strs() As pair(Of String, String),
                                           ByRef result As vector(Of String),
                                           ByVal ignore_empty_entity As Boolean,
                                           ByVal ignore_surround_strs As Boolean,
                                           ByVal case_sensitive As Boolean) As Boolean
        If String.IsNullOrEmpty(s) Then
            Return False
        Else
            result.renew()
            If isemptyarray(separators) Then
                result.emplace_back(s)
                Return True
            ElseIf isemptyarray(surround_strs) Then
                Dim ss() As String = Nothing
                ss = s.Split(separators, If(ignore_empty_entity,
                                            StringSplitOptions.RemoveEmptyEntries,
                                            StringSplitOptions.None))
                'should have at least one entity, so false should not be returned
                Return assert(result.emplace_back(ss))
            Else
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
                            l.Append(s(i))
                            i += 1
                        End If
                    ElseIf is_inarray(s, i, separators, len, hit, case_sensitive) Then
                        If Not ignore_empty_entity OrElse strlen(l) > 0 Then
                            result.emplace_back(Convert.ToString(l))
                            l.clear()
                        End If
                        i += len
                    ElseIf is_inarray(s, i, surround_strs, surrounded, len, hit, case_sensitive) Then
                        i += len
                        If Not ignore_surround_strs Then
                            l.Append(hit)
                        End If
                    Else
                        l.Append(s(i))
                        i += 1
                    End If
                End While
                If Not ignore_empty_entity OrElse strlen(l) > 0 Then
                    result.emplace_back(Convert.ToString(l))
                End If
                Return True
            End If
        End If
    End Function
End Module
