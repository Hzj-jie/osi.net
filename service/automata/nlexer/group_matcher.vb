
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class group_matcher
        Implements matcher

        Private ReadOnly m As vector(Of matcher)

        Public Sub New(ByVal m As vector(Of matcher))
            assert(Not m Is Nothing)
            assert(Not m.empty())
            Me.m = m
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            Dim j As UInt32 = 0
            While j < m.size()
                Dim r As [optional](Of UInt32) = Nothing
                r = m(j).match(i, pos)
                If r Then
                    Return r
                End If
                j += uint32_1
            End While
            Return [optional].of(Of UInt32)()
        End Function
    End Class
End Class
