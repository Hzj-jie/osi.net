
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _simhash
    Private ReadOnly hash_size As Int32 = CInt(bit_count_in_byte * sizeof_int32)

    Public Function simhash(Of T)(ByVal i() As T) As UInt32
        If isemptyarray(i) Then
            Return 0
        Else
            Dim v() As Int32 = Nothing
            ReDim v(hash_size - 1)
            arrays.clear(v)
            For j As Int32 = 0 To array_size_i(i) - 1
                Dim h As UInt32 = 0
                h = signing(i(j))
                For k As Int32 = 0 To hash_size - 1
                    If ((h And (1 << k)) = 1) Then
                        v(k) += 1
                    Else
                        v(k) -= 1
                    End If
                Next
            Next

            Dim o As UInt32 = 0
            For j As Int32 = 0 To hash_size - 1
                If v(j) > 0 Then
                    o += (uint32_1 << j)
                End If
            Next
            Return o
        End If
    End Function

    <Extension()> Public Function simhash(ByVal i As String) As UInt32
        Return simhash(c_str(i))
    End Function

    <Extension()> Public Function hamming_dist(ByVal i As UInt32, ByVal j As UInt32) As Int32
        Dim b As UInt32 = 0
        b = (i Xor j)
        Dim o As Int32 = 0
        For k As Int32 = 0 To hash_size - 1
            If ((b And (1 << k)) = 1) Then
                o += 1
            End If
        Next
        Return o
    End Function

    Public Function hamming_dist(Of T)(ByVal i() As T, ByVal j() As T) As Int32
        Dim h1 As UInt32 = 0
        Dim h2 As UInt32 = 0
        h1 = simhash(i)
        h2 = simhash(j)
        Return hamming_dist(h1, h2)
    End Function

    <Extension()> Public Function hamming_dist(ByVal i As String, ByVal j As String) As Int32
        Return hamming_dist(c_str(i), c_str(j))
    End Function

    Public Function similarity(Of T)(ByVal i() As T, ByVal j() As T) As Double
        Return (hash_size - hamming_dist(i, j)) / hash_size
    End Function

    <Extension()> Public Function similarity(ByVal i As String, ByVal j As String) As Double
        Return similarity(c_str(i), c_str(j))
    End Function
End Module
