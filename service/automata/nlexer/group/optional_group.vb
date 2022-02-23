
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class optional_group
        Implements matcher

        Private ReadOnly g As matcher

        Public Sub New(ByVal g As matcher)
            assert(g IsNot Nothing)
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

        Public Shared Sub register()
            groups.register(characters.optional_suffix,
                            Function(ByVal i As matcher) As matcher
                                Return New optional_group(i)
                            End Function)
        End Sub
    End Class
End Class
