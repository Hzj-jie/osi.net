
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    ' TODO: Merge with matching_group
    Public NotInheritable Class or_matcher
        Implements matcher

        Private ReadOnly m As vector(Of matcher)

        Public Sub New(ByVal m As vector(Of matcher))
            assert(m IsNot Nothing)
            assert(Not m.empty())
            Me.m = m
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            Dim j As UInt32 = 0
            Dim max As [optional](Of UInt32) = Nothing
            max = [optional].empty(Of UInt32)()
            While j < m.size()
                Dim r As [optional](Of UInt32) = Nothing
                r = m(j).match(i, pos)
                If (Not r.empty()) AndAlso (+r > pos) AndAlso ((Not max) OrElse (+max) < (+r)) Then
                    max = r
                End If
                j += uint32_1
            End While
            Return max
        End Function
    End Class
End Class
