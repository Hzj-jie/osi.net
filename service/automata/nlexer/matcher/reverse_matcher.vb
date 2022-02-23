
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class reverse_matcher
        Implements matcher

        Private ReadOnly m As matcher

        Public Sub New(ByVal m As matcher)
            assert(m IsNot Nothing)
            Me.m = m
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            If m.match(i, pos) Then
                Return [optional].empty(Of UInt32)()
            End If
            Return [optional].of(pos)
        End Function
    End Class
End Class
