﻿
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _set
    Private Function insert(Of T)(ByVal this As bbst(Of T),
                                  ByVal v As vector(Of T),
                                  ByVal emplace As Boolean) As Boolean
        If this Is Nothing OrElse v Is Nothing Then
            Return False
        ElseIf v.empty() Then
            Return True
        Else
            If emplace Then
                For i As UInt32 = 0 To v.size() - uint32_1
                    this.emplace(v(i))
                Next
            Else
                For i As UInt32 = 0 To v.size() - uint32_1
                    this.insert(v(i))
                Next
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function insert(Of T)(ByVal this As bbst(Of T), ByVal v As vector(Of T)) As Boolean
        Return insert(this, v, False)
    End Function

    <Extension()> Public Function emplace(Of T)(ByVal this As bbst(Of T), ByVal v As vector(Of T)) As Boolean
        Return insert(this, v, True)
    End Function

    <Extension()> Public Function null_or_empty(Of T)(ByVal this As [set](Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function
End Module
