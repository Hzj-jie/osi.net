
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

<global_init(global_init_level.runtime_checkers)>
Public Module _levenshtein
    Private Sub init()
        assert(npos < 0)
    End Sub

    Public Function levenshtein(Of T)(ByVal s1() As T,
                                      ByVal s2() As T,
                                      ByVal same As Func(Of T, T, Boolean)) As Int32
        If same Is Nothing Then
            Return npos
        End If
        If isemptyarray(s1) Then
            Return array_size_i(s2)
        End If
        If isemptyarray(s2) Then
            Return array_size_i(s1)
        End If
        Dim v0() As Int32 = Nothing
        Dim v1() As Int32 = Nothing
        ReDim v0(array_size_i(s1))
        ReDim v1(array_size_i(s1))
        For i As Int32 = 0 To array_size_i(v0) - 1
            v0(i) = i
        Next

        For i As Int32 = 0 To array_size_i(s2) - 1
            v1(0) = i + 1
            For j As Int32 = 0 To array_size_i(s1) - 1
                v1(j + 1) = min(v1(j) + 1, v0(j + 1) + 1)
                v1(j + 1) = min(v1(j + 1), v0(j) + If(same(s2(i), s1(j)), 0, 1))
            Next

            For j As Int32 = 0 To array_size_i(v0) - 1
                v0(j) = v1(j)
            Next
        Next

        Return v1(array_size_i(s1))
    End Function

    Public Function levenshtein(Of T)(ByVal s1() As T,
                                      ByVal s2() As T,
                                      ByVal cmp As Func(Of T, T, Int32)) As Int32
        If cmp Is Nothing Then
            Return npos
        End If
        Return levenshtein(s1,
                           s2,
                           Function(x As T, y As T) As Boolean
                               Return cmp(x, y) = 0
                           End Function)
    End Function

    Public Function levenshtein(Of T)(ByVal s1() As T, ByVal s2() As T) As Int32
        Return levenshtein(s1, s2, AddressOf compare)
    End Function

    <Extension()> Public Function levenshtein(ByVal s1() As Char,
                                              ByVal s2() As Char,
                                              Optional ByVal case_sensitive As Boolean = True) As Int32
        If case_sensitive Then
            Return levenshtein(s1, s2, Function(x, y) x = y)
        End If
        Return levenshtein(s1, s2, Function(x, y) Char.ToLower(x) = Char.ToLower(y))
    End Function

    <Extension()> Public Function levenshtein(ByVal s1 As String,
                                              ByVal s2 As String,
                                              Optional ByVal case_sensitive As Boolean = True) As Int32
        Return levenshtein(c_str(s1), c_str(s2), case_sensitive)
    End Function
End Module
