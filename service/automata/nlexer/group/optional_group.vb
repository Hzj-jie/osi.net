
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class optional_group
        Implements matcher

        Private ReadOnly g As group

        Public Sub New(ByVal g As group)
            assert(Not g Is Nothing)
            Me.g = g
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            Dim r As [optional](Of UInt32) = Nothing
            r = g.match(i, pos)
            If r Then
                Return r
            End If
            Return [optional].of(pos)
        End Function
    End Class
End Class
