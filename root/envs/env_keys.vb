
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _env_keys
    <Extension()> Public Function env_keys(ByVal piece As String,
                                           ByVal ParamArray other_pieces() As String) As String()
        Dim k() As String = Nothing
        ReDim k(array_size(other_pieces))
        k(0) = piece
        For i As Int32 = 0 To array_size(other_pieces) - 1
            k(i + 1) = other_pieces(i)
        Next
        Return env_keys(k)
    End Function

    Public Function env_keys(ByVal pieces() As String) As String()
        assert(Not isemptyarray(pieces))
        pieces.strtolower()
        Dim r() As String = Nothing
        ReDim r(9 - 1)
        Dim i As Int32 = 0
        r(inc_(i)) = strcat(copy(pieces).strtolower())
        r(inc_(i)) = strcat(copy(pieces).strtoupper())
        r(inc_(i)) = strcat(copy(pieces).str_initial_upper())
        r(inc_(i)) = strjoin(copy(pieces).strtolower(), "_")
        r(inc_(i)) = strjoin(copy(pieces).strtoupper(), "_")
        r(inc_(i)) = strjoin(copy(pieces).str_initial_upper(), "_")
        r(inc_(i)) = strjoin(copy(pieces).strtolower(), "-")
        r(inc_(i)) = strjoin(copy(pieces).strtoupper(), "-")
        r(inc_(i)) = strjoin(copy(pieces).str_initial_upper(), "-")
        Return r
    End Function
End Module
