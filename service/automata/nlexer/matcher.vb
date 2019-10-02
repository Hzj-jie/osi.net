
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public Interface matcher
        'start matching string i from pos and return the end of the match.
        Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32)
    End Interface

    Public NotInheritable Class never_matcher
        Implements matcher

        Public Shared ReadOnly instance As never_matcher

        Shared Sub New()
            instance = New never_matcher()
        End Sub

        Private Sub New()
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            Return [optional].of(Of UInt32)()
        End Function
    End Class
End Class
