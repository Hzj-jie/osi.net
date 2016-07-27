
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class rlexer
    Public Interface matching_group
        'start matching string i from pos, output all the potential candidates
        'if no matches, output a null or empty vector
        Function match(ByVal i As String, ByVal pos As UInt32) As vector(Of UInt32)
    End Interface
End Class

Public Module _rlexer_matching_group
    <Extension()> Public Function match(ByVal g As rlexer.matching_group,
                                        ByVal i As String,
                                        ByVal pos As UInt32,
                                        ByRef o As vector(Of UInt32)) As Boolean
        If g Is Nothing Then
            Return False
        Else
            o = g.match(i, pos)
            Return Not o.null_or_empty()
        End If
    End Function

    <Extension()> Public Function match(ByVal g As rlexer.matching_group, ByVal i As String) As vector(Of UInt32)
        Return If(g Is Nothing, Nothing, g.match(i, uint32_0))
    End Function

    <Extension()> Public Function match(ByVal g As rlexer.matching_group,
                                        ByVal i As String,
                                        ByRef o As vector(Of UInt32)) As Boolean
        Return match(g, i, uint32_0, o)
    End Function

    <Extension()> Public Function match_inplace(ByVal g As rlexer.matching_group,
                                                ByVal i As String,
                                                ByVal pos As UInt32) As Boolean
        Dim v As vector(Of UInt32) = Nothing
        If g.match(i, pos, v) Then
            Return v.find(pos) <> npos
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function match_inplace(ByVal g As rlexer.matching_group, ByVal i As String) As Boolean
        Return match_inplace(g, i, uint32_0)
    End Function

    <Extension()> Public Function match_to_end(ByVal g As rlexer.matching_group,
                                               ByVal i As String,
                                               ByVal pos As UInt32) As Boolean
        Dim v As vector(Of UInt32) = Nothing
        Return match(g, i, pos, v) AndAlso
               v.find(strlen(i)) <> npos
    End Function

    <Extension()> Public Function match_to_end(ByVal g As rlexer.matching_group,
                                               ByVal i As String) As Boolean
        Return match_to_end(g, i, uint32_0)
    End Function

    <Extension()> Public Function longest_match(ByVal g As rlexer.matching_group,
                                                ByVal i As String,
                                                ByVal pos As UInt32,
                                                ByRef new_pos As UInt32) As Boolean
        Dim v As vector(Of UInt32) = Nothing
        If g.match(i, pos, v) AndAlso assert(Not v.null_or_empty()) Then
            new_pos = 0
            For j As UInt32 = 0 To v.size() - uint32_1
                If new_pos < v(j) Then
                    new_pos = v(j)
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function
End Module
