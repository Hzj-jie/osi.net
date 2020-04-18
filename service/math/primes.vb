
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class primes
    Public Const max_precalculated_prime As UInt32 = 100000
    Public Shared ReadOnly precalculated_count As UInt32
    Private Shared ReadOnly is_pr() As Boolean
    Private Shared ReadOnly prs() As UInt32

    Shared Sub New()
        ReDim is_pr(CInt(max_precalculated_prime) - 1)
        arrays.fill(is_pr, True)
        is_pr(0) = False
        is_pr(1) = False
        For i As Int32 = 0 To CInt(max_precalculated_prime).sqrt()
            If is_pr(i) Then
                Dim j As Int32 = 0
                j = i + i
                While j < max_precalculated_prime
                    is_pr(j) = False
                    j += i
                End While
            End If
        Next
        For i As Int32 = 0 To CInt(max_precalculated_prime) - 1
            If is_pr(i) Then
                precalculated_count += uint32_1
            End If
        Next
        ReDim prs(CInt(precalculated_count - uint32_1))
        precalculated_count = 0
        For i As Int32 = 0 To CInt(max_precalculated_prime) - 1
            If is_pr(i) Then
                prs(CInt(precalculated_count)) = CUInt(i)
                precalculated_count += uint32_1
            End If
        Next
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function precalculated(ByVal i As UInt32) As UInt32
        assert(i < precalculated_count)
        Return prs(CInt(i))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function select_precalculated(ByVal i As UInt32) As UInt32
        Return precalculated(i Mod precalculated_count)
    End Function

    Public Shared Function contain(ByVal i As UInt32) As Boolean
        If precalculated_contain(i) Then
            Return True
        End If

        Dim r As UInt32 = 0
        r = i.sqrt()
        assert(r < max_precalculated_prime)
        If r * r = i Then
            Return False
        End If
        For j As Int32 = 0 To CInt(precalculated_count) - 1
            If prs(j) > r Then
                Return True
            End If
            If i Mod prs(j) = 0 Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Shared Function precalculated_contain(ByVal i As UInt32) As Boolean
        If i <= 1 Then
            Return False
        End If
        If i < max_precalculated_prime Then
            Return is_pr(CInt(i))
        End If
        Return False
    End Function

    Private Sub New()
    End Sub
End Class

Public Module _prime
    <Extension()> Public Function is_prime(ByVal i As Int32) As Boolean
        If i < 0 Then
            Return False
        End If
        Return CUInt(i).is_prime()
    End Function

    <Extension()> Public Function is_prime(ByVal i As UInt32) As Boolean
        Return primes.contain(i)
    End Function
End Module
