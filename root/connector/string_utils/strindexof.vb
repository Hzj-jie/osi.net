
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Public Module _strindexof
    Private ReadOnly strindexof_impl As _do_val_val_ref(Of String, String, UInt32, Int32)
    Private ReadOnly strlastindexof_impl As _do_val_val_ref(Of String, String, UInt32, Int32)

    Sub New()
        strindexof_impl = AddressOf strindexofImpl
        strlastindexof_impl = AddressOf strlastindexofImpl
    End Sub

    'will fail when s contains some invalid characters
    Private Function strindexofImpl(ByVal s As String, ByVal search As String, ByRef start As UInt32) As Int32
        Dim rtn As Int32 = 0
        rtn = s.IndexOf(search, CInt(start))
        If rtn <> npos Then
            start = rtn + strlen(search)
        End If

        Return rtn
    End Function

    Private Function strlastindexofImpl(ByVal s As String, ByVal search As String, ByRef start As UInt32) As Int32
        Dim rtn As Int32 = 0
        rtn = s.LastIndexOf(search, CInt(start))
        If rtn <> npos Then
            If rtn < strlen(search) Then
                start = max_uint32
            Else
                start = rtn - strlen(search)
            End If
        End If

        Return rtn
    End Function

    Private Function strindexofImpl(ByVal s As String,
                                    ByVal search As String,
                                    ByVal start As UInt32,
                                    ByVal index_of_index As UInt32,
                                    ByVal indexcall As _do_val_val_ref(Of String, String, UInt32, Int32),
                                    ByVal case_sensitive As Boolean) As Int32
        If String.IsNullOrEmpty(search) Then
            Return 0
        ElseIf String.IsNullOrEmpty(s) Then
            Return npos
        ElseIf index_of_index = 0 Then
            Return 0
        ElseIf start >= strlen(s) Then
            Return npos
        Else
            If Not case_sensitive Then
                s = s.ToLower()
                search = search.ToLower()
            End If
            Dim searchlen As UInt32 = 0
            searchlen = strlen(search)
            Dim rtn As Int32 = 0
            Dim nextSearch As UInt32 = 0
            nextSearch = start
            Dim i As UInt32 = 0
            For i = 0 To index_of_index - uint32_1
                rtn = indexcall(s, search, nextSearch)
                If rtn = npos Then
                    Exit For
                ElseIf nextSearch >= strlen(s) AndAlso i < index_of_index - uint32_1 Then
                    'cannot search for one more time, so the rtn should be npos
                    rtn = npos
                    Exit For
                End If
            Next

            Return rtn
        End If
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
        Return strindexofImpl(s, search, start, index_of_index, strindexof_impl, case_sensitive)
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
                              If(String.IsNullOrEmpty(s), max_uint32, strlen(s) - 1),
                              index_of_index,
                              case_sensitive)
    End Function

    <Extension()> Public Function strlastindexof(ByVal s As String,
                                                 ByVal search As String,
                                                 ByVal start As UInt32,
                                                 ByVal index_of_index As UInt32,
                                                 Optional ByVal case_sensitive As Boolean = True) As Int32
        Return strindexofImpl(s, search, start, index_of_index, strlastindexof_impl, case_sensitive)
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a() As Char) As Boolean
        If String.IsNullOrEmpty(s) Then
            Return False
        ElseIf isemptyarray(a) Then
            Return True
        Else
            Return s.IndexOfAny(a) <> npos
        End If
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a()() As Char) As Boolean
        If String.IsNullOrEmpty(s) Then
            Return False
        ElseIf isemptyarray(a) Then
            Return True
        Else
            For i As UInt32 = 0 To array_size(a) - uint32_1
                If Not isemptyarray(a(i)) Then
                    If s.IndexOfAny(a(i)) <> npos Then
                        Return True
                    End If
                End If
            Next
            Return False
        End If
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a() As String) As Boolean
        If String.IsNullOrEmpty(s) Then
            Return False
        ElseIf isemptyarray(a) Then
            Return True
        Else
            For i As UInt32 = 0 To array_size(a) - uint32_1
                If strindexof(s, a(i)) <> npos Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    <Extension()> Public Function contains_any(ByVal s As String, ByVal ParamArray a()() As String) As Boolean
        If String.IsNullOrEmpty(s) Then
            Return False
        ElseIf isemptyarray(a) Then
            Return True
        Else
            For i As UInt32 = 0 To array_size(a) - uint32_1
                If Not isemptyarray(a(i)) Then
                    For j As UInt32 = 0 To array_size(a(i)) - uint32_1
                        If strindexof(s, a(i)(j)) <> npos Then
                            Return True
                        End If
                    Next
                End If
            Next
            Return False
        End If
    End Function
End Module
