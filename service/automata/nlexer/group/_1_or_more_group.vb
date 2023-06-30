
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class _1_or_more_group
        Implements matcher

        Private ReadOnly g As matcher

        Public Sub New(ByVal g As matcher)
            assert(Not g Is Nothing)
            Me.g = g
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            Dim r As [optional](Of UInt32) = Nothing
            r = [optional].empty(Of UInt32)()
            While True
                Dim n As [optional](Of UInt32) = Nothing
                n = g.match(i, If(r, +r, pos))
                If n Then
                    r = n
                Else
                    Exit While
                End If
            End While
            Return r
        End Function

        Public Shared Sub register()
            groups.register(characters._1_or_more_suffix,
                            Function(ByVal i As matcher) As matcher
                                Return New _1_or_more_group(i)
                            End Function)
        End Sub
    End Class
End Class
