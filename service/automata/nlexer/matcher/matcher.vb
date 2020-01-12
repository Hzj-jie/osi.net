
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _matcher
    <Extension()> Public Function match(ByVal m As nlexer.matcher, ByVal i As String) As [optional](Of UInt32)
        Return throws.not_null(m).match(i, uint32_0)
    End Function
End Module

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
            Return [optional].empty(Of UInt32)()
        End Function
    End Class
End Class
