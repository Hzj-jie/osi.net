
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class relatively_prime
    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function gcd(ByVal a As Int32, ByVal b As Int32) As Int32
        assert(a <> min_int32)
        assert(b <> min_int32)
        Return CInt(gcd(CUInt(a.abs()), CUInt(b.abs())))
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function gcd(ByVal a As UInt32, ByVal b As UInt32) As UInt32
        If a = b Then
            Return a
        End If
        If a = 0 OrElse b = 0 Then
            Return 0
        End If
        If a = 1 OrElse b = 1 Then
            Return 1
        End If

        If a < b Then
            swap(a, b)
        End If

        If a Mod b = 0 Then
            Return b
        End If

        If primes.precalculated_contain(a) OrElse primes.precalculated_contain(b) Then
            Return 1
        End If

        Dim c As UInt32 = 0
        c = a Mod b
        While c > 0
            a = b
            b = c
            c = a Mod b
        End While
        Return b
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function gcd(ByVal a As Int64, ByVal b As Int64) As Int64
        assert(a <> min_int64)
        assert(b <> min_int64)
        Return CLng(gcd(CULng(a.abs()), CULng(b.abs())))
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function gcd(ByVal a As UInt64, ByVal b As UInt64) As UInt64
        If a <= max_uint32 AndAlso b <= max_uint32 Then
            Return gcd(CUInt(a), CUInt(b))
        End If

        If a = b Then
            Return a
        End If
        If a = 0 OrElse b = 0 Then
            Return 0
        End If
        If a = 1 OrElse b = 1 Then
            Return 1
        End If

        If a < b Then
            swap(a, b)
        End If

        If a Mod b = 0 Then
            Return b
        End If

        Dim c As UInt64 = 0
        c = a Mod b
        While c > 0
            a = b
            b = c
            c = a Mod b
        End While
        Return b
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function between(ByVal a As Int32, ByVal b As Int32) As Boolean
        Return gcd(a, b) = 1
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function between(ByVal a As UInt32, ByVal b As UInt32) As Boolean
        Return gcd(a, b) = 1
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function between(ByVal a As Int64, ByVal b As Int64) As Boolean
        Return gcd(a, b) = 1
    End Function

    <MethodImpl(math_debug.aggressive_inlining)>
    Public Shared Function between(ByVal a As UInt64, ByVal b As UInt64) As Boolean
        Return gcd(a, b) = 1
    End Function

    Private Sub New()
    End Sub
End Class
