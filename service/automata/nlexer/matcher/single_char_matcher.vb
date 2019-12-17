
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public Class single_char_matcher
        Implements matcher

        Private Shared ReadOnly instance As single_char_matcher

        Shared Sub New()
            instance = New single_char_matcher()
        End Sub

        Protected Sub New()
        End Sub

        Public Shared Sub register()
            matchers.register("*", Function() As matcher
                                       Return instance
                                   End Function)
        End Sub

        Protected Overridable Function check(ByVal c As Char) As Boolean
            Return True
        End Function

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            If strlen(i) > pos AndAlso check(i.char_at(pos)) Then
                Return [optional].of(pos + uint32_1)
            End If
            Return [optional].of(Of UInt32)()
        End Function
    End Class
End Class
