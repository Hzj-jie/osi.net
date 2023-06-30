
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Public Module _strindexof
    Private ReadOnly strindexof_impl As _do_val_val_ref(Of String, String, UInt32, Int32) =
        AddressOf _strindexof
    Private ReadOnly strlastindexof_impl As _do_val_val_ref(Of String, String, UInt32, Int32) =
        AddressOf _strlastindexof

    'will fail when s contains some invalid characters
    Private Function _strindexof(ByVal s As String, ByVal search As String, ByRef start As UInt32) As Int32
        Dim rtn As Int32 = 0
        rtn = s.IndexOf(search, CInt(start))
        If rtn <> npos Then
            start = CUInt(rtn) + strlen(search)
        End If

        Return rtn
    End Function

    Private Function _strlastindexof(ByVal s As String, ByVal search As String, ByRef start As UInt32) As Int32
        Dim rtn As Int32 = 0
        rtn = s.LastIndexOf(search, CInt(start))
        If rtn <> npos Then
            If rtn < strlen(search) Then
                start = max_uint32
            Else
                start = CUInt(rtn) - strlen(search)
            End If
        End If

        Return rtn
    End Function

    Private Function _strindexof(ByVal s As String,
                                 ByVal search As String,
                                 ByVal start As UInt32,
                                 ByVal index_of_index As UInt32,
                                 ByVal indexcall As _do_val_val_ref(Of String, String, UInt32, Int32),
                                 ByVal case_sensitive As Boolean) As Int32
        If search.null_or_empty() Then
            Return 0
        End If
        If s.null_or_empty() Then
            Return npos
        End If
        If index_of_index = 0 Then
            Return 0
        End If
        If start >= strlen(s) Then
            Return npos
        End If
        If Not case_sensitive Then
            s = s.ToLower()
            search = search.ToLower()
        End If
        Dim searchlen As UInt32 = 0
        searchlen = strlen(search)
        Dim rtn As Int32 = 0
        Dim next_search As UInt32 = 0
        next_search = start
        For i As UInt32 = 0 To index_of_index - uint32_1
            rtn = indexcall(s, search, next_search)
            If rtn = npos Then
                Exit For
            End If
            If next_search >= strlen(s) AndAlso i < index_of_index - uint32_1 Then
                'cannot search for one more time, so the rtn should be npos
                rtn = npos
                Exit For
            End If
        Next

        Return rtn
    End Function

    <Extension()> Public Function strindexof(ByVal s As String,
                                             ByVal search As String,
                                             Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strindexof(s, search, uint32_1, case_sensitive)
    End Function

    <Extension()> Public Function strindexof(ByVal s As String,
                                             ByVal search As String,
                                             ByVal index_of_index As UInt32,
                                             Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strindexof(s, search, uint32_0, index_of_index, case_sensitive)
    End Function

    <Extension()> Public Function strindexof(ByVal s As String,
                                             ByVal search As String,
                                             ByVal start As UInt32,
                                             ByVal index_of_index As UInt32,
                                             Optional ByVal case_sensitive As Boolean = True) As Int32
        Return _strindexof(s, search, start, index_of_index, strindexof_impl, case_sensitive)
    End Function

    <Extension()> Public Function next_index_of(ByVal s As String,
                                                ByVal search As String,
                                                ByVal start As UInt32,
                                                Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strindexof(s, search, start, uint32_1, case_sensitive)
    End Function

    <Extension()> Public Function strlastindexof(ByVal s As String,
                                                 ByVal search As String,
                                                 Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strlastindexof(s, search, uint32_1, case_sensitive)
    End Function

    <Extension()> Public Function strlastindexof(ByVal s As String,
                                                 ByVal search As String,
                                                 ByVal index_of_index As UInt32,
                                                 Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strlastindexof(s,
                              search,
                              If(s.null_or_empty(), max_uint32, strlen(s) - uint32_1),
                              index_of_index,
                              case_sensitive)
    End Function

    <Extension()> Public Function strlastindexof(ByVal s As String,
                                                 ByVal search As String,
                                                 ByVal start As UInt32,
                                                 ByVal index_of_index As UInt32,
                                                 Optional ByVal case_sensitive As Boolean = True) As Int32
        Return _strindexof(s, search, start, index_of_index, strlastindexof_impl, case_sensitive)
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a() As Char) As Boolean
        If s.null_or_empty() Then
            Return False
        ElseIf isemptyarray(a) Then
            Return True
        Else
            Return s.IndexOfAny(a) <> npos
        End If
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a()() As Char) As Boolean
        If s.null_or_empty() Then
            Return False
        ElseIf isemptyarray(a) Then
            Return True
        Else
            For i As UInt32 = 0 To array_size(a) - uint32_1
                If Not isemptyarray(a(CInt(i))) AndAlso s.IndexOfAny(a(CInt(i))) <> npos Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a() As String) As Boolean
        If s.null_or_empty() Then
            Return False
        End If
        If isemptyarray(a) Then
            Return True
        End If
        For i As UInt32 = 0 To array_size(a) - uint32_1
            If strindexof(s, a(CInt(i))) <> npos Then
                Return True
            End If
        Next
        Return False
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a()() As String) As Boolean
        If s.null_or_empty() Then
            Return False
        End If
        If isemptyarray(a) Then
            Return True
        End If
        For i As UInt32 = 0 To array_size(a) - uint32_1
            If Not isemptyarray(a(CInt(i))) Then
                For j As UInt32 = 0 To array_size(a(CInt(i))) - uint32_1
                    If strindexof(s, a(CInt(i))(CInt(j))) <> npos Then
                        Return True
                    End If
                Next
            End If
        Next
        Return False
    End Function
End Module
