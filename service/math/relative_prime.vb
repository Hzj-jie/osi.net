
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Module _relative_prime
    Public Function gcd(ByVal a As Int32, ByVal b As Int32) As Int32
        assert(a <> min_int32)
        assert(b <> min_int32)
        If a < 0 Then
            a = -a
        End If
        If b < 0 Then
            b = -b
        End If
        If a = 0 Then
            Return b
        End If
        If b = 0 Then
            Return a
        End If
        If a < b Then
            swap(a, b)
        End If
        Dim c As Int32 = 0
        c = a Mod b
        While c > 0
            a = b
            b = c
            c = a Mod b
        End While
        Return b
    End Function

    Public Function gcd(ByVal a As UInt32, ByVal b As UInt32) As UInt32
        If a = 0 Then
            Return b
        End If
        If b = 0 Then
            Return a
        End If
        If a < b Then
            swap(a, b)
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

    Public Function gcd(ByVal a As Int64, ByVal b As Int64) As Int64
        assert(a <> min_int64)
        assert(b <> min_int64)
        If a < 0 Then
            a = -a
        End If
        If b < 0 Then
            b = -b
        End If
        If a = 0 Then
            Return b
        End If
        If b = 0 Then
            Return a
        End If
        If a < b Then
            swap(a, b)
        End If
        Dim c As Int64 = 0
        c = a Mod b
        While c > 0
            a = b
            b = c
            c = a Mod b
        End While
        Return b
    End Function

    Public Function gcd(ByVal a As UInt64, ByVal b As UInt64) As UInt64
        If a = 0 Then
            Return b
        End If
        If b = 0 Then
            Return a
        End If
        If a < b Then
            swap(a, b)
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

    Public Function relatively_prime(ByVal a As Int32, ByVal b As Int32) As Boolean
        Return gcd(a, b) = 1
    End Function

    Public Function relatively_prime(ByVal a As UInt32, ByVal b As UInt32) As Boolean
        Return gcd(a, b) = 1
    End Function

    Public Function relatively_prime(ByVal a As Int64, ByVal b As Int64) As Boolean
        Return gcd(a, b) = 1
    End Function

    Public Function relatively_prime(ByVal a As UInt64, ByVal b As UInt64) As Boolean
        Return gcd(a, b) = 1
    End Function
End Module
