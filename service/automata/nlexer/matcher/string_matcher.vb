
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class string_matcher
        Implements matcher

        Private ReadOnly s As String
        Private ReadOnly l As UInt32

        Public Sub New(ByVal s As String)
            Me.s = unescape(s)
            l = strlen(Me.s)
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            If strsame(i, pos, s, uint32_0, l) Then
                Return [optional].of(pos + l)
            End If
            Return [optional].of(Of UInt32)()
        End Function
    End Class
End Class
