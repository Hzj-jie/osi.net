
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _levenshtein
    Sub New()
        assert(npos < 0)
    End Sub

    Public Function levenshtein(Of T)(ByVal s1() As T,
                                      ByVal s2() As T,
                                      ByVal same As Func(Of T, T, Boolean)) As Int32
        If same Is Nothing Then
            Return npos
        ElseIf isemptyarray(s1) Then
            Return array_size(s2)
        ElseIf isemptyarray(s2) Then
            Return array_size(s1)
        Else
            Dim v0() As Int32 = Nothing
            Dim v1() As Int32 = Nothing
            ReDim v0(array_size(s1))
            ReDim v1(array_size(s1))
            For i As Int32 = 0 To array_size(v0) - 1
                v0(i) = i
            Next

            For i As Int32 = 0 To array_size(s2) - 1
                v1(0) = i + 1
                For j As Int32 = 0 To array_size(s1) - 1
                    v1(j + 1) = min(v1(j) + 1, v0(j + 1) + 1)
                    v1(j + 1) = min(v1(j + 1), v0(j) + If(same(s2(i), s1(j)), 0, 1))
                Next

                For j As Int32 = 0 To array_size(v0) - 1
                    v0(j) = v1(j)
                Next
            Next

            Return v1(array_size(s1))
        End If
    End Function

    Public Function levenshtein(Of T)(ByVal s1() As T,
                                      ByVal s2() As T,
                                      ByVal cmp As Func(Of T, T, Int32)) As Int32
        If cmp Is Nothing Then
            Return npos
        Else
            Return levenshtein(s1,
                               s2,
                               Function(x As T, y As T) As Boolean
                                   Return cmp(x, y) = 0
                               End Function)
        End If
    End Function

    Public Function levenshtein(Of T)(ByVal s1() As T, ByVal s2() As T) As Int32
        Return levenshtein(s1, s2, AddressOf compare)
    End Function

    <Extension()> Public Function levenshtein(ByVal s1() As Char,
                                              ByVal s2() As Char,
                                              Optional ByVal case_sensitive As Boolean = True) As Int32
        If case_sensitive Then
            Return levenshtein(s1, s2, Function(x, y) x = y)
        Else
            Return levenshtein(s1, s2, Function(x, y) Char.ToLower(x) = Char.ToLower(y))
        End If
    End Function

    <Extension()> Public Function levenshtein(ByVal s1 As String,
                                              ByVal s2 As String,
                                              Optional ByVal case_sensitive As Boolean = True) As Int32
        Return levenshtein(c_str(s1), c_str(s2), case_sensitive)
    End Function
End Module
