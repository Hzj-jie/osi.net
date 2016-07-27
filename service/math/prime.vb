
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.constants

Public Module _prime
    Public Const max_prime As Int32 = 100000
    Public ReadOnly prime_count As Int32
    Private ReadOnly is_pr() As Boolean
    Private ReadOnly prs() As Int32

    Sub New()
        ReDim is_pr(max_prime - 1)
        memset(is_pr, True)
        is_pr(0) = False
        is_pr(1) = False
        For i As Int32 = 0 To max_prime.sqrt()
            If is_pr(i) Then
                Dim j As Int32 = 0
                j = i + i
                While j < max_prime
                    is_pr(j) = False
                    j += i
                End While
            End If
        Next
        For i As Int32 = 0 To max_prime - 1
            If is_pr(i) Then
                prime_count += 1
            End If
        Next
        ReDim prs(prime_count - 1)
        prime_count = 0
        For i As Int32 = 0 To max_prime - 1
            If is_pr(i) Then
                prs(prime_count) = i
                prime_count += 1
            End If
        Next
    End Sub

    <Extension()> Public Function is_prime(ByVal i As Int32) As Boolean
        If i < min_uint32 Then
            Return False
        Else
            Return CUInt(i).is_prime()
        End If
    End Function

    <Extension()> Public Function is_prime(ByVal i As UInt32) As Boolean
        If i <= 1 Then
            Return False
        ElseIf i < max_prime Then
            Return is_pr(i)
        Else
            Dim r As UInt32 = 0
            r = i.sqrt()
            assert(r < max_prime)
            If r * r = i Then
                Return False
            Else
                For j As Int32 = 0 To prime_count - 1
                    If prs(j) > r Then
                        Return True
                    ElseIf i Mod prs(j) = 0 Then
                        Return False
                    End If
                Next
                Return True
            End If
        End If
    End Function

    Public Function prime(ByVal i As Int32) As Int32
        assert(i < prime_count AndAlso i >= 0)
        Return prs(i)
    End Function
End Module
